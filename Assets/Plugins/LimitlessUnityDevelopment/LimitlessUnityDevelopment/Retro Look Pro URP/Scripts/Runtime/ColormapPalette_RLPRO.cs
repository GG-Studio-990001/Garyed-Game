using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class ColormapPalette_RLPRO : ScriptableRendererFeature
{
    ColormapPalette_RLPROPass RetroPass;
    public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


    public override void Create()
    {
        RetroPass = new ColormapPalette_RLPROPass(Event);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_2019 || UNITY_2020
        RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
        renderer.EnqueuePass(RetroPass);
    }
    public class ColormapPalette_RLPROPass : ScriptableRenderPass
    {
        static readonly string k_RenderTag = "Renderr Glitch1 Effect";
        static readonly int MainTexId = Shader.PropertyToID("_MainTex");
        static readonly int heightV = Shader.PropertyToID("height");
        static readonly int widthV = Shader.PropertyToID("width");
        static readonly int _DitherV = Shader.PropertyToID("_Dither");
        static readonly int _OpacityV = Shader.PropertyToID("_Opacity");
        static readonly int _BlueNoiseV = Shader.PropertyToID("_BlueNoise");
        static readonly int _PaletteV = Shader.PropertyToID("_Palette");
        static readonly int _ColormapV = Shader.PropertyToID("_Colormap");
        static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");
        static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");
        static readonly int _Mask = Shader.PropertyToID("_Mask");

        ColormapPalette retroEffect;
        Material RetroEffectMaterial;
        RenderTargetIdentifier currentTarget;
        public int tempPresetIndex = 0;
        private bool m_Init;
        Texture2D colormapPalette;
        Texture3D colormapTexture;
        private Vector2 m_Res;
        private int m_TempPixelSize;

        public ColormapPalette_RLPROPass(RenderPassEvent evt)
        {
            renderPassEvent = evt;
            var shader = Shader.Find("Hidden/Shader/ColormapPaletteEffect_RLPRO");
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
            retroEffect = stack.GetComponent<ColormapPalette>();
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
            ApplyMaterialVariables(RetroEffectMaterial, out m_Res);

            if (m_Init || intHasChanged(tempPresetIndex, retroEffect.presetIndex.value) || m_TempPixelSize != retroEffect.pixelSize.value)
            {
                tempPresetIndex = retroEffect.presetIndex.value;
                ApplyColormapToMaterial(RetroEffectMaterial);
                m_Init = false;
                m_TempPixelSize = retroEffect.pixelSize.value;
            }

            float ratio = ((float)cameraData.camera.scaledPixelWidth) / (float)cameraData.camera.scaledPixelHeight;

            var w = cameraData.camera.scaledPixelWidth;
            var h = cameraData.camera.scaledPixelHeight;

            RetroEffectMaterial.SetInt(heightV, (int)retroEffect.pixelSize.value);
            RetroEffectMaterial.SetInt(widthV, Mathf.RoundToInt((int)retroEffect.pixelSize.value * ratio));

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
            cmd.GetTemporaryRT(destination, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);


            cmd.Blit(source, destination);
            cmd.Blit(destination, source, RetroEffectMaterial, shaderPass);
        }
        private void ParamSwitch(Material mat, bool paramValue, string paramName)
        {
            if (paramValue) mat.EnableKeyword(paramName);
            else mat.DisableKeyword(paramName);
        }

        public void ApplyMaterialVariables(Material bl, out Vector2 res)
        {

            res.x = Screen.width / retroEffect.pixelSize.value;
            res.y = Screen.height / retroEffect.pixelSize.value;

            retroEffect.opacity.value = Mathf.Clamp01(retroEffect.opacity.value);
            retroEffect.dither.value = Mathf.Clamp01(retroEffect.dither.value);

            bl.SetFloat(_DitherV, retroEffect.dither.value);
            bl.SetFloat(_OpacityV, retroEffect.opacity.value);
        }
        public void ApplyColormapToMaterial(Material bl)
        {

            if (retroEffect.presetsList.value != null)
            {
                if (retroEffect.bluenoise.value != null)
                {
                    bl.SetTexture(_BlueNoiseV, retroEffect.bluenoise.value);

                }
                ApplyPalette(bl);
                ApplyMap(bl);
            }
        }
        void ApplyPalette(Material bl)
        {
            colormapPalette = new Texture2D(256, 1, TextureFormat.RGB24, false);
            colormapPalette.filterMode = FilterMode.Point;
            colormapPalette.wrapMode = TextureWrapMode.Clamp;

            for (int i = 0; i < retroEffect.presetsList.value.presetsList[retroEffect.presetIndex.value].preset.numberOfColors; ++i)
            {
                colormapPalette.SetPixel(i, 0, retroEffect.presetsList.value.presetsList[retroEffect.presetIndex.value].preset.palette[i]);
            }

            colormapPalette.Apply();

            bl.SetTexture(_PaletteV, colormapPalette);
        }
        public void ApplyMap(Material bl)
        {
            int colorsteps = 64;
            colormapTexture = new Texture3D(colorsteps, colorsteps, colorsteps, TextureFormat.RGB24, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            colormapTexture.SetPixels32(retroEffect.presetsList.value.presetsList[retroEffect.presetIndex.value].preset.pixels);
            colormapTexture.Apply();
            bl.SetTexture(_ColormapV, colormapTexture);

        }
        public bool intHasChanged(int A, int B)
        {
            bool result = false;
            if (B != A)
            {
                A = B;
                result = true;
            }
            return result;
        }
    }

}


