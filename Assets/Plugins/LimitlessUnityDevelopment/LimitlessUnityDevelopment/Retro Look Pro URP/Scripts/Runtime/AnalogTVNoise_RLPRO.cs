using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class AnalogTVNoise_RLPRO : ScriptableRendererFeature
{
	AnalogTVNoise_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


	public override void Create()
	{
		RetroPass = new AnalogTVNoise_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(RetroPass);
#if UNITY_2019 || UNITY_2020
		RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
	}

	public class AnalogTVNoise_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Render Analog TV Noise Effect";
		static readonly int MainTexId = Shader.PropertyToID("_MainTex");
		static readonly int TimeXV = Shader.PropertyToID("TimeX");
		static readonly int _PatternV = Shader.PropertyToID("_Pattern");
		static readonly int barHeightV = Shader.PropertyToID("barHeight");
		static readonly int barSpeedV = Shader.PropertyToID("barSpeed");
		static readonly int cutV = Shader.PropertyToID("cut");
		static readonly int edgeCutOffV = Shader.PropertyToID("edgeCutOff");
		static readonly int angleV = Shader.PropertyToID("angle");
		static readonly int tileXV = Shader.PropertyToID("tileX");
		static readonly int tileYV = Shader.PropertyToID("tileY");
		static readonly int horizontalV = Shader.PropertyToID("horizontal");
		static readonly int _OffsetNoiseXV = Shader.PropertyToID("_OffsetNoiseX");
		static readonly int _OffsetNoiseYV = Shader.PropertyToID("_OffsetNoiseY");
		static readonly int _FadeV = Shader.PropertyToID("_Fade");
		static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");
		static readonly int _Mask = Shader.PropertyToID("_Mask");
		static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");

		AnalogTVNoise retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;

		float TimeX;

		public AnalogTVNoise_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/AnalogTVNoiseEffect_RLPRO");
			if (shader == null)
			{
				Debug.LogError("Shader not found.");
				return;
			}
			RetroEffectMaterial = CoreUtils.CreateEngineMaterial(shader);
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
			retroEffect = stack.GetComponent<AnalogTVNoise>();
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

			int shaderPass = 0;

			TimeX += Time.deltaTime;
			if (TimeX > 100) TimeX = 0;

			RetroEffectMaterial.SetFloat(TimeXV, TimeX);
			RetroEffectMaterial.SetFloat(_FadeV, retroEffect.Fade.value);
			if (retroEffect.texture.value != null)
				RetroEffectMaterial.SetTexture(_PatternV, retroEffect.texture.value);
			RetroEffectMaterial.SetFloat(barHeightV, retroEffect.barWidth.value);
			RetroEffectMaterial.SetFloat(barSpeedV, retroEffect.barSpeed.value);
			RetroEffectMaterial.SetFloat(cutV, retroEffect.CutOff.value);
			RetroEffectMaterial.SetFloat(edgeCutOffV, retroEffect.edgeCutOff.value);
			RetroEffectMaterial.SetFloat(angleV, retroEffect.textureAngle.value);
			RetroEffectMaterial.SetFloat(tileXV, retroEffect.tile.value.x);
			RetroEffectMaterial.SetFloat(tileYV, retroEffect.tile.value.y);
			RetroEffectMaterial.SetFloat(horizontalV, retroEffect.Horizontal.value ? 1 : 0);
			if (!retroEffect.staticNoise.value)
			{
				RetroEffectMaterial.SetFloat(_OffsetNoiseXV, UnityEngine.Random.Range(0f, 0.6f));
				RetroEffectMaterial.SetFloat(_OffsetNoiseYV, UnityEngine.Random.Range(0f, 0.6f));
			}
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


			cmd.SetGlobalTexture(MainTexId, source);

			cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);


			cmd.Blit(source, destination);
			cmd.Blit(destination, source, RetroEffectMaterial, shaderPass);
		}
		private void ParamSwitch(Material mat, bool paramValue, string paramName)
		{
			if (paramValue) mat.EnableKeyword(paramName);
			else mat.DisableKeyword(paramName);
		}

	}

}


