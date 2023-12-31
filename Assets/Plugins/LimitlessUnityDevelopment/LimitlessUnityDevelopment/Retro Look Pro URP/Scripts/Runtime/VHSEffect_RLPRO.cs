using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class VHSEffect_RLPRO : ScriptableRendererFeature
{
	VHSEffect_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


	public override void Create()
	{
		RetroPass = new VHSEffect_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
#if UNITY_2019 || UNITY_2020
		RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
		renderer.EnqueuePass(RetroPass);
	}
	public class VHSEffect_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Render VHS Effect";
		static readonly int MainTexId = Shader.PropertyToID("_MainTex");
		static readonly int TimeV = Shader.PropertyToID("Time");

		static readonly int _OffsetPosY = Shader.PropertyToID("_OffsetPosY");
		static readonly int smoothSize = Shader.PropertyToID("smoothSize");
		static readonly int _StandardDeviation = Shader.PropertyToID("_StandardDeviation");
		static readonly int iterations = Shader.PropertyToID("iterations");
		static readonly int tileX = Shader.PropertyToID("tileX");
		static readonly int smooth = Shader.PropertyToID("smooth1");
		static readonly int tileY = Shader.PropertyToID("tileY");
		static readonly int _OffsetDistortion = Shader.PropertyToID("_OffsetDistortion");
		static readonly int _Stripes = Shader.PropertyToID("_Stripes");

		static readonly int _OffsetColorAngle = Shader.PropertyToID("_OffsetColorAngle");
		static readonly int _OffsetColor = Shader.PropertyToID("_OffsetColor");
		static readonly int _OffsetNoiseX = Shader.PropertyToID("_OffsetNoiseX");
		static readonly int _SecondaryTex = Shader.PropertyToID("_SecondaryTex");
		static readonly int _OffsetNoiseY = Shader.PropertyToID("_OffsetNoiseY");
		static readonly int _TexIntensity = Shader.PropertyToID("_TexIntensity");
		static readonly int _TexCut = Shader.PropertyToID("_TexCut");
		static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");
		static readonly int _Mask = Shader.PropertyToID("_Mask");

		static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");
		private float T;
		VHSEffect retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;

		public VHSEffect_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/VHSEffect_RLPRO");
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
			retroEffect = stack.GetComponent<VHSEffect>();
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
			cmd.SetGlobalTexture(MainTexId, source);
			cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			if (!retroEffect.unscaledTime.value)
				T += Time.deltaTime;
			else
				T += Time.unscaledDeltaTime;
			RetroEffectMaterial.SetFloat(TimeV, T);
			if (UnityEngine.Random.Range(0, 100 - retroEffect.verticalOffsetFrequency.value) <= 5)
			{
				if (retroEffect.verticalOffset == 0.0f)
				{
					RetroEffectMaterial.SetFloat(_OffsetPosY, retroEffect.verticalOffset.value);
				}
				if (retroEffect.verticalOffset.value > 0.0f)
				{
					RetroEffectMaterial.SetFloat(_OffsetPosY, retroEffect.verticalOffset.value - UnityEngine.Random.Range(0f, retroEffect.verticalOffset.value));
				}
				else if (retroEffect.verticalOffset.value < 0.0f)
				{
					RetroEffectMaterial.SetFloat(_OffsetPosY, retroEffect.verticalOffset.value + UnityEngine.Random.Range(0f, -retroEffect.verticalOffset.value));
				}
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
			RetroEffectMaterial.SetFloat(iterations, retroEffect.iterations.value);
			RetroEffectMaterial.SetFloat(smoothSize, retroEffect.smoothSize.value);
			RetroEffectMaterial.SetFloat(_StandardDeviation, retroEffect.deviation.value);

			RetroEffectMaterial.SetFloat(tileX, retroEffect.tile.value.x);
			RetroEffectMaterial.SetFloat(smooth, retroEffect.smoothCut.value ? 1 : 0);
			RetroEffectMaterial.SetFloat(tileY, retroEffect.tile.value.y);
			RetroEffectMaterial.SetFloat(_OffsetDistortion, retroEffect.offsetDistortion.value);
			RetroEffectMaterial.SetFloat(_Stripes, 0.51f - retroEffect.stripes.value);

			RetroEffectMaterial.SetVector(_OffsetColorAngle, new Vector2(Mathf.Sin(retroEffect.colorOffsetAngle.value),
					Mathf.Cos(retroEffect.colorOffsetAngle.value)));
			RetroEffectMaterial.SetFloat(_OffsetColor, retroEffect.colorOffset.value * 0.001f);

			RetroEffectMaterial.SetFloat(_OffsetNoiseX, UnityEngine.Random.Range(-0.4f, 0.4f));
			if (retroEffect.noiseTexture.value != null)
				RetroEffectMaterial.SetTexture(_SecondaryTex, retroEffect.noiseTexture.value);

			if (RetroEffectMaterial.HasProperty(_OffsetNoiseY))
			{
				float offsetNoise = RetroEffectMaterial.GetFloat(_OffsetNoiseY);
				RetroEffectMaterial.SetFloat(_OffsetNoiseY, offsetNoise + UnityEngine.Random.Range(-0.03f, 0.03f));
			}
			RetroEffectMaterial.SetFloat(_TexIntensity, retroEffect._textureIntensity.value);
			RetroEffectMaterial.SetFloat(_TexCut, retroEffect._textureCutOff.value);
			cmd.Blit(source, destination);
			cmd.Blit(destination, source, RetroEffectMaterial, (int)retroEffect.blendMode.value);
		}
		private void ParamSwitch(Material mat, bool paramValue, string paramName)
		{
			if (paramValue) mat.EnableKeyword(paramName);
			else mat.DisableKeyword(paramName);
		}

	}

}