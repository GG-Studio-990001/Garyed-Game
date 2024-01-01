using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class Glitch3 : ScriptableRendererFeature
{
    Glitch3Pass GlitchPass;
    public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


    public override void Create()
    {
        GlitchPass = new Glitch3Pass(Event);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_2019 || UNITY_2020
        GlitchPass.Setup(renderer.cameraColorTarget);
#else

#endif
        renderer.EnqueuePass(GlitchPass);
    }
    public class Glitch3Pass : ScriptableRenderPass
    {
        static readonly string k_RenderTag = "Render Glitch3 Effect";
        static readonly int MainTexId = Shader.PropertyToID("_MainTex");
        static readonly int maxOffsetY = Shader.PropertyToID("maxOffsetY");
        static readonly int maxOffsetX = Shader.PropertyToID("maxOffsetX");
        static readonly int blockSize = Shader.PropertyToID("blockSize");
        static readonly int speed = Shader.PropertyToID("speed");
        static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");
        static readonly int _Mask = Shader.PropertyToID("_Mask");
        static readonly int TempTargetId = Shader.PropertyToID("Glitch3");
        LimitlessGlitch3 Glitch3;
        Material Glitch3Material;
        RenderTargetIdentifier currentTarget;


        public Glitch3Pass(RenderPassEvent evt)
        {
            renderPassEvent = evt;
            var shader = Shader.Find("RetroLookPro/Glitch3");
            if (shader == null)
            {
                Debug.LogError("Shader not found.");
                return;
            }
            Glitch3Material = CoreUtils.CreateEngineMaterial(shader);

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
            if (Glitch3Material == null)
            {
                Debug.LogError("Material not created.");
                return;
            }
            var stack = VolumeManager.instance.stack;
            Glitch3 = stack.GetComponent<LimitlessGlitch3>();
            if (!renderingData.cameraData.postProcessEnabled && Glitch3.GlobalPostProcessingSettings.value) return;

            if (Glitch3 == null) { return; }
            if (!Glitch3.IsActive()) { return; }

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
            if (Glitch3.mask.value != null)
            {
                Glitch3Material.SetTexture(_Mask, Glitch3.mask.value);
                Glitch3Material.SetFloat(_FadeMultiplier, 1);
                ParamSwitch(Glitch3Material, Glitch3.maskChannel.value == maskChannelMode.alphaChannel ? true : false, "ALPHA_CHANNEL");
            }
            else
            {
                Glitch3Material.SetFloat(_FadeMultiplier, 0);
            }
            Glitch3Material.SetFloat(speed, Glitch3.speed.value);
            Glitch3Material.SetFloat(blockSize, Glitch3.blockSize.value);
            Glitch3Material.SetFloat(maxOffsetX, Glitch3.maxOffsetX.value);
            Glitch3Material.SetFloat(maxOffsetY, Glitch3.maxOffsetY.value);
            cmd.SetGlobalTexture(MainTexId, source);

            cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);


            cmd.Blit(source, destination);
            cmd.Blit(destination, source, Glitch3Material, shaderPass);
        }
        private void ParamSwitch(Material mat, bool paramValue, string paramName)
        {
            if (paramValue) mat.EnableKeyword(paramName);
            else mat.DisableKeyword(paramName);
        }

    }

}


