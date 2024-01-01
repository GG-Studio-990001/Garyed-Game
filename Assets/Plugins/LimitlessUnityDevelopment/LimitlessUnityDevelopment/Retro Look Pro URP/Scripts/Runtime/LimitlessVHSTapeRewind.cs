using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LimitlessVHSTapeRewind : ScriptableRendererFeature
{
    LimitlessVHSTapeRewindPass Pass;
    public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


    public override void Create()
    {
        Pass = new LimitlessVHSTapeRewindPass(Event);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_2019 || UNITY_2020
        Pass.Setup(renderer.cameraColorTarget);
#else

#endif
        renderer.EnqueuePass(Pass);
    }
    public class LimitlessVHSTapeRewindPass : ScriptableRenderPass
    {
        static readonly string k_RenderTag = "Render Glitch1 Effect";
        static readonly int MainTexId = Shader.PropertyToID("_MainTex");

		static readonly int TempTargetId = Shader.PropertyToID("Glitch1");

        static readonly int NOISE_STATIC = Shader.PropertyToID("NOISE_STATIC");
        static readonly int intencity = Shader.PropertyToID("intencity");
        static readonly int fade = Shader.PropertyToID("fade");



        VHSTapeRewind m_VHSNoise;
        Material VHSNoiseMaterial;
        RenderTargetIdentifier currentTarget;


        public LimitlessVHSTapeRewindPass(RenderPassEvent evt)
        {
            renderPassEvent = evt;
            var shader = Shader.Find("Hidden/Shader/VHS_Tape_Rewind");
            if (shader == null)
            {
                Debug.LogError("Shader not found.");
                return;
            }
            VHSNoiseMaterial = CoreUtils.CreateEngineMaterial(shader);

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
            if (VHSNoiseMaterial == null)
            {
                Debug.LogError("Material not created.");
                return;
            }
            var stack = VolumeManager.instance.stack;
            m_VHSNoise = stack.GetComponent<VHSTapeRewind>();
            if (!renderingData.cameraData.postProcessEnabled && m_VHSNoise.GlobalPostProcessingSettings.value) return;

            if (m_VHSNoise == null) { return; }
            if (!m_VHSNoise.IsActive()) { return; }

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

            VHSNoiseMaterial.SetFloat(intencity, m_VHSNoise.intencity.value);
            VHSNoiseMaterial.SetFloat(fade, m_VHSNoise.fade.value);
            
            cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
            
			cmd.SetGlobalTexture(MainTexId, source);
  
            cmd.Blit(source, destination);
            cmd.Blit(destination, source, VHSNoiseMaterial, 0);

        }

    }

}


