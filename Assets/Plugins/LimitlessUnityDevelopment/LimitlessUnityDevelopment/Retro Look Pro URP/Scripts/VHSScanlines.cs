using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Retro Look Pro/VHS Scanlines")]

public class VHSScanlines : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
    [Tooltip("Lines color.")]
    public ColorParameter scanLinesColor = new ColorParameter(new Color());
    [Tooltip("Amount of scanlines.")]
    public FloatParameter scanLines = new FloatParameter(1.5f);
    [Tooltip("Lines speed.")]
    public FloatParameter speed = new FloatParameter(0);
    [Tooltip("Effect fade.")]
    public ClampedFloatParameter fade = new ClampedFloatParameter(1f, 0f, 1f);
    [Tooltip("Enable horizontal lines.")]
    public BoolParameter horizontal = new BoolParameter(true);
    [Tooltip("distortion.")]
    public ClampedFloatParameter distortion = new ClampedFloatParameter(0.2f, 0f, 0.5f);
    [Tooltip("distortion1.")]
    public FloatParameter distortion1 = new FloatParameter(0);
    [Tooltip("distortion2.")]
    public FloatParameter distortion2 = new FloatParameter(0);
    [Tooltip("Scale lines size.")]
    public FloatParameter scale = new FloatParameter(1);
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