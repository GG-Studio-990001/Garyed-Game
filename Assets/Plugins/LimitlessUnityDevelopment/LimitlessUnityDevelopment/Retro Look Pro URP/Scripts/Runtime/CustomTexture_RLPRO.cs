using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomTexture_RLPRO : ScriptableRendererFeature
{
	CustomTexture_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;
	public override void Create()
	{
		RetroPass = new CustomTexture_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
#if UNITY_2019 || UNITY_2020
		RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
		renderer.EnqueuePass(RetroPass);
	}
	public class CustomTexture_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Renderr Glitch1 Effect";
		static readonly int MainTexId = Shader.PropertyToID("_MainTex");
		static readonly int _CustomTextureV = Shader.PropertyToID("_CustomTex");
		static readonly int fadeV = Shader.PropertyToID("fade");
		static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");

		CustomTexture retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;

		public CustomTexture_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/CustomTextureEffect_RLPRO");
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
			retroEffect = stack.GetComponent<CustomTexture>();
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
			if (retroEffect.texture.value != null)
				RetroEffectMaterial.SetTexture(_CustomTextureV, retroEffect.texture.value);
			RetroEffectMaterial.SetFloat(fadeV, retroEffect.fade.value);


			cmd.SetGlobalTexture(MainTexId, source);

			cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);


			cmd.Blit(source, destination);
			cmd.Blit(destination, source, RetroEffectMaterial, shaderPass);
		}
	}
}


