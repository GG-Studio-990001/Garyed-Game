using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
[VolumeComponentMenu("Retro Look Pro/Glitch3")]
public class LimitlessGlitch3 : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);

    [Range(0f, 50f), Tooltip("Effect speed.")]
    public ClampedFloatParameter speed = new ClampedFloatParameter(1f, 0f, 50f);
    [Range(0f, 505f), Tooltip("block size (higher value = smaller blocks).")]
    public ClampedFloatParameter blockSize = new ClampedFloatParameter(1f, 0f, 505f);

    [Range(0f, 25f), Tooltip("maximum color shift on X axis.")]
    public ClampedFloatParameter maxOffsetX = new ClampedFloatParameter(1f, 0f, 25f);
    [Range(0f, 25f), Tooltip("maximum color shift on Y axis.")]
    public ClampedFloatParameter maxOffsetY = new ClampedFloatParameter(1f, 0f, 25f);
    [Space]
    [Tooltip("Mask texture")]
    public TextureParameter mask = new TextureParameter(null);
    public maskChannelModeParameter maskChannel = new maskChannelModeParameter();
    [Space]
    [Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
    public BoolParameter GlobalPostProcessingSettings = new BoolParameter(false);


    public bool IsActive() => (bool)enable;

    public bool IsTileCompatible() => false;
}