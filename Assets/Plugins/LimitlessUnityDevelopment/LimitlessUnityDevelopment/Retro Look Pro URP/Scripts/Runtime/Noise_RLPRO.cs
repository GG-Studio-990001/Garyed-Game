using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class Noise_RLPRO : ScriptableRendererFeature
{
	Noise_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


	public override void Create()
	{
		RetroPass = new Noise_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{

#if UNITY_2019 || UNITY_2020
		RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
		renderer.EnqueuePass(RetroPass);
	}
	public class Noise_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Renderr Glitch1 Effect";
		static readonly int MainTexId = Shader.PropertyToID("_MainTex");
		static readonly int alphaTexV = Shader.PropertyToID("alphaTex");
		static readonly int _AlphaMapTexV = Shader.PropertyToID("_AlphaMapTex");
		static readonly int tapeLinesAmountV = Shader.PropertyToID("tapeLinesAmount");
		static readonly int time_V = Shader.PropertyToID("time_");
		static readonly int screenLinesNumV = Shader.PropertyToID("screenLinesNum");
		static readonly int noiseLinesNumV = Shader.PropertyToID("noiseLinesNum");
		static readonly int noiseQuantizeXV = Shader.PropertyToID("noiseQuantizeX");
		static readonly int signalNoisePowerV = Shader.PropertyToID("signalNoisePower");
		static readonly int signalNoiseAmountV = Shader.PropertyToID("signalNoiseAmount");
		static readonly int filmGrainAmountV = Shader.PropertyToID("filmGrainAmount");
		static readonly int tapeNoiseTHV = Shader.PropertyToID("tapeNoiseTH");
		static readonly int tapeNoiseAmountV = Shader.PropertyToID("tapeNoiseAmount");
		static readonly int tapeNoiseSpeedV = Shader.PropertyToID("tapeNoiseSpeed");
		static readonly int lineNoiseAmountV = Shader.PropertyToID("lineNoiseAmount");
		static readonly int lineNoiseSpeedV = Shader.PropertyToID("lineNoiseSpeed");
		static readonly int _TapeTexV = Shader.PropertyToID("_TapeTex");
		static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");
		static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");
		static readonly int _Mask = Shader.PropertyToID("_Mask");

		Noise retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;
		private float _time;
		private RenderTexture texTape;

		public Noise_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/NoiseEffects_RLPRO");
			if (shader == null)
			{
				Debug.LogError("Shader not found.");
				return;
			}
			RetroEffectMaterial = CoreUtils.CreateEngineMaterial(shader);
			//texTape = RTHandles.Alloc(Vector2.one, TextureXR.slices, colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.B10G11R11_UFloatPack32, dimension: TextureDimension.Tex2DArray, enableRandomWrite: true, useDynamicScale: true, name: "texLast");
		}
#if UNITY_2019 || UNITY_2020

#elif UNITY_2021
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			var renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTarget;
		}
#else
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			var renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTargetHandle;
		}
#endif

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (RetroEffectMaterial == null)
			{
				Debug.LogError("Material not created.");
				return;
			}
            var stack = VolumeManager.instance.stack;
			retroEffect = stack.GetComponent<Noise>();
            if (!renderingData.cameraData.postProcessEnabled && retroEffect.GlobalPostProcessingSettings.value) return;

            if (retroEffect == null) { return; }
			if (!retroEffect.IsActive()) { return; }

			var cmd = CommandBufferPool.Get(k_RenderTag);
			Render(cmd, ref renderingData);
			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}

		public void Setup(in RenderTargetIdentifier currentTarget)
		{
			this.currentTarget = currentTarget;
		}

		void Render(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ref var cameraData = ref renderingData.cameraData;
			var source = currentTarget;
			int destination = TempTargetId;
			//
			if (retroEffect.unscaledTime.value) { _time = Time.unscaledTime; }
			else _time = Time.time;

			cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			float screenLinesNum_ = retroEffect.stretchResolution.value;
			if (screenLinesNum_ <= 0) screenLinesNum_ = Screen.height;

			if (texTape == null || (texTape.height != Mathf.Min(retroEffect.VerticalResolution.value, screenLinesNum_)))
			{
				int texHeight = (int)Mathf.Min(retroEffect.VerticalResolution.value, screenLinesNum_);
				int texWidth = (int)(
					  (float)texHeight * (float)Screen.width / (float)Screen.height);

#if UNITY_EDITOR
				UnityEngine.Object.DestroyImmediate(texTape);
#else
            UnityEngine.Object.Destroy(texTape);
#endif
				texTape = new RenderTexture(texWidth, texHeight, 0);
				texTape.hideFlags = HideFlags.HideAndDontSave;
				texTape.filterMode = FilterMode.Point;
				texTape.Create();
				cmd.Blit(destination, texTape, RetroEffectMaterial, 0);
			}

			cmd.SetGlobalTexture(MainTexId, source);

			cmd.Blit(source, destination);



			if (retroEffect.mask.value != null)
			{
				RetroEffectMaterial.SetTexture(_Mask, retroEffect.mask.value);
				RetroEffectMaterial.SetFloat(_FadeMultiplier, 1);
				ParamSwitch(RetroEffectMaterial, retroEffect.maskChannel.value == maskChannelMode.alphaChannel ? true : false, "ALPHA_CHANNEL");
			}
			else
			{
				RetroEffectMaterial.SetFloat(_FadeMultiplier, 0);
			}
			RetroEffectMaterial.SetFloat(tapeLinesAmountV, 1 - retroEffect.tapeLinesAmount.value);
			RetroEffectMaterial.SetFloat(time_V, _time);
			RetroEffectMaterial.SetFloat(screenLinesNumV, screenLinesNum_);
			RetroEffectMaterial.SetFloat(noiseLinesNumV, retroEffect.VerticalResolution.value);
			RetroEffectMaterial.SetFloat(noiseQuantizeXV, retroEffect.TapeNoiseSignalProcessing.value);
			ParamSwitch(RetroEffectMaterial, retroEffect.Granularity.value, "VHS_FILMGRAIN_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.TapeNoise.value, "VHS_TAPENOISE_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.LineNoise.value, "VHS_LINENOISE_ON");
			ParamSwitch(RetroEffectMaterial, retroEffect.SignalNoise.value, "VHS_YIQNOISE_ON");

			RetroEffectMaterial.SetFloat(signalNoisePowerV, retroEffect.SignalNoisePower.value);
			RetroEffectMaterial.SetFloat(signalNoiseAmountV, retroEffect.SignalNoiseAmount.value);
			RetroEffectMaterial.SetFloat(filmGrainAmountV, retroEffect.GranularityAmount.value);
			RetroEffectMaterial.SetFloat(tapeNoiseTHV, retroEffect.TapeNoiseAmount.value);
			RetroEffectMaterial.SetFloat(tapeNoiseAmountV, retroEffect.TapeNoiseFade.value);
			RetroEffectMaterial.SetFloat(tapeNoiseSpeedV, retroEffect.TapeNoiseSpeed.value);

			RetroEffectMaterial.SetFloat(lineNoiseAmountV, retroEffect.LineNoiseAmount.value);
			RetroEffectMaterial.SetFloat(lineNoiseSpeedV, retroEffect.LineNoiseSpeed.value);
			cmd.Blit(texTape, texTape, RetroEffectMaterial, 1);
			RetroEffectMaterial.SetTexture(_TapeTexV, texTape);
			cmd.Blit(texTape, texTape, RetroEffectMaterial, 1);
			cmd.Blit(destination, source, RetroEffectMaterial, 0);
		}
		private void ParamSwitch(Material mat, bool paramValue, string paramName)
		{
			if (paramValue) mat.EnableKeyword(paramName);
			else mat.DisableKeyword(paramName);
		}
	}

}


