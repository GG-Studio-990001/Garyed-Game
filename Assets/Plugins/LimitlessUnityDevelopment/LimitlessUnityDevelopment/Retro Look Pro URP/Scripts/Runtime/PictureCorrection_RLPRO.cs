using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class PictureCorrection_RLPRO : ScriptableRendererFeature
{
	PictureCorrection_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


	public override void Create()
	{
		RetroPass = new PictureCorrection_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
#if UNITY_2019 || UNITY_2020
		RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
		renderer.EnqueuePass(RetroPass);
	}
	public class PictureCorrection_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Renderr Glitch1 Effect";
		static readonly int MainTexId = Shader.PropertyToID("_MainTex");
		static readonly int signalAdjustY = Shader.PropertyToID("signalAdjustY");
		static readonly int signalAdjustI = Shader.PropertyToID("signalAdjustI");
		static readonly int signalAdjustQ = Shader.PropertyToID("signalAdjustQ");
		static readonly int signalShiftY = Shader.PropertyToID("signalShiftY");
		static readonly int signalShiftI = Shader.PropertyToID("signalShiftI");
		static readonly int signalShiftQ = Shader.PropertyToID("signalShiftQ");
		static readonly int gammaCorection = Shader.PropertyToID("gammaCorection");
		static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");
		static readonly int _Mask = Shader.PropertyToID("_Mask");
		static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");


		PictureCorrection retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;

		public PictureCorrection_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/PictureCorrectionEffect_RLPRO");
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
			retroEffect = stack.GetComponent<PictureCorrection>();
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

			RetroEffectMaterial.SetFloat(signalAdjustY, retroEffect.signalAdjustY.value);
			RetroEffectMaterial.SetFloat(signalAdjustI, retroEffect.signalAdjustI.value);
			RetroEffectMaterial.SetFloat(signalAdjustQ, retroEffect.signalAdjustQ.value);
			RetroEffectMaterial.SetFloat(signalShiftY, retroEffect.signalShiftY.value);
			RetroEffectMaterial.SetFloat(signalShiftI, retroEffect.signalShiftI.value);
			RetroEffectMaterial.SetFloat(signalShiftQ, retroEffect.signalShiftQ.value);
			RetroEffectMaterial.SetFloat(gammaCorection, retroEffect.gammaCorection.value);

			cmd.SetGlobalTexture(MainTexId, source);
			cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.Blit(source, destination);
			cmd.Blit(destination, source, RetroEffectMaterial, 0);
		}
		private void ParamSwitch(Material mat, bool paramValue, string paramName)
		{
			if (paramValue) mat.EnableKeyword(paramName);
			else mat.DisableKeyword(paramName);
		}

	}

}


