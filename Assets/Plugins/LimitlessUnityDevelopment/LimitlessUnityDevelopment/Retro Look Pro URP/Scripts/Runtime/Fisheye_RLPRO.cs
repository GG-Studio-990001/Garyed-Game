using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Fisheye_RLPRO : ScriptableRendererFeature
{
	Fisheye_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


	public override void Create()
	{
		RetroPass = new Fisheye_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
#if UNITY_2019 || UNITY_2020
		RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
		renderer.EnqueuePass(RetroPass);
	}
	public class Fisheye_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Renderr Glitch1 Effect";
		static readonly int MainTexId = Shader.PropertyToID("_InputTexture");
		static readonly int cutoffXV = Shader.PropertyToID("cutoffX");
		static readonly int cutoffYV = Shader.PropertyToID("cutoffY");
		static readonly int cutoffFadeXV = Shader.PropertyToID("cutoffFadeX");
		static readonly int cutoffFadeYV = Shader.PropertyToID("cutoffFadeY");
		static readonly int fisheyeBendV = Shader.PropertyToID("fisheyeBend");
		static readonly int fisheyeSizeV = Shader.PropertyToID("fisheyeSize");
		static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		Fisheye retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;

		public Fisheye_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/FisheyeEffect_RLPRO");
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
			retroEffect = stack.GetComponent<Fisheye>();
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
			ParamSwitch(RetroEffectMaterial, true, "VHS_FISHEYE_ON");
			RetroEffectMaterial.SetFloat(cutoffXV, retroEffect.cutOffX.value);
			RetroEffectMaterial.SetFloat(cutoffYV, retroEffect.cutOffY.value);
			RetroEffectMaterial.SetFloat(cutoffFadeXV, retroEffect.fadeX.value);
			RetroEffectMaterial.SetFloat(cutoffFadeYV, retroEffect.fadeY.value);
			ParamSwitch(RetroEffectMaterial, retroEffect.fisheyeType.value == FisheyeTypeEnum.Hyperspace, "VHS_FISHEYE_HYPERSPACE");
			RetroEffectMaterial.SetFloat(fisheyeBendV, retroEffect.bend.value);
			RetroEffectMaterial.SetFloat(fisheyeSizeV, retroEffect.size.value);

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


