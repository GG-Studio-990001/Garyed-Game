using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class VHSScanlines_RLPRO : ScriptableRendererFeature
{
	VHSScanlines_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


	public override void Create()
	{
		RetroPass = new VHSScanlines_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
#if UNITY_2019 || UNITY_2020
		RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
		renderer.EnqueuePass(RetroPass);
	}
	public class VHSScanlines_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Renderr Glitch1 Effect";
		static readonly int MainTexId = Shader.PropertyToID("_MainTex");
		static readonly int TimeV = Shader.PropertyToID("Time");

		static readonly int _ScanLinesV = Shader.PropertyToID("_ScanLines");
		static readonly int speedV = Shader.PropertyToID("speed");
		static readonly int fadeV = Shader.PropertyToID("fade");
		static readonly int _OffsetDistortionV = Shader.PropertyToID("_OffsetDistortion");
		static readonly int sfericalV = Shader.PropertyToID("sferical");
		static readonly int barrelV = Shader.PropertyToID("barrel");
		static readonly int scaleV = Shader.PropertyToID("scale");
		static readonly int _ScanLinesColorV = Shader.PropertyToID("_ScanLinesColor");
		static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");
		static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");
		static readonly int _Mask = Shader.PropertyToID("_Mask");

		private float T;
		VHSScanlines retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;

		public VHSScanlines_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/VHSScanlinesEffect_RLPRO");
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
			retroEffect = stack.GetComponent<VHSScanlines>();
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
			int pass;
			cmd.SetGlobalTexture(MainTexId, source);

			cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);


			T += Time.deltaTime;



			RetroEffectMaterial.SetFloat(TimeV, T);
			RetroEffectMaterial.SetFloat(_ScanLinesV, retroEffect.scanLines.value);
			RetroEffectMaterial.SetFloat(speedV, retroEffect.speed.value);
			RetroEffectMaterial.SetFloat(_OffsetDistortionV, retroEffect.distortion.value);
			RetroEffectMaterial.SetFloat(fadeV, retroEffect.fade.value);
			RetroEffectMaterial.SetFloat(sfericalV, retroEffect.distortion1.value);
			RetroEffectMaterial.SetFloat(barrelV, retroEffect.distortion2.value);
			RetroEffectMaterial.SetFloat(scaleV, retroEffect.scale.value);
			RetroEffectMaterial.SetColor(_ScanLinesColorV, retroEffect.scanLinesColor.value);
			if (retroEffect.horizontal.value)
			{
				if (retroEffect.distortion.value != 0)
					pass = 1;
				else
					pass = 0;
			}
			else
			{
				if (retroEffect.distortion.value != 0)
					pass = 3;
				else
					pass = 2;
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
			cmd.Blit(source, destination);

			cmd.Blit(destination, source, RetroEffectMaterial, pass);
		}
		private void ParamSwitch(Material mat, bool paramValue, string paramName)
		{
			if (paramValue) mat.EnableKeyword(paramName);
			else mat.DisableKeyword(paramName);
		}

	}

}


