using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
[VolumeComponentMenu("Retro Look Pro/Glitch1")]

public class LimitlessGlitch1 : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
    [Header("Random Seed")]
    [Tooltip("seed x")]
    public ClampedFloatParameter x = new ClampedFloatParameter(127.1f, -2f, 200f);
    [Tooltip("seed y")]
    public ClampedFloatParameter y = new ClampedFloatParameter(43758.5453123f, -2f, 10002f);
    [Tooltip("seed z")]
    public ClampedFloatParameter z = new ClampedFloatParameter(311.7f, -2f, 200f);
    [Space]
    [Tooltip("Effect fade.")]
    public ClampedFloatParameter fade = new ClampedFloatParameter(1f, 0f, 1f);
    [Tooltip("Effect amount")]
    public ClampedFloatParameter amount = new ClampedFloatParameter(1f, 0f, 2f);
    [Tooltip("Stretch on X axes")]
    public ClampedFloatParameter stretch = new ClampedFloatParameter(0.02f, 0f, 4f);
    [Tooltip("Effect speed.")]
    public ClampedFloatParameter speed = new ClampedFloatParameter(0.5f, 0f, 1f);
    [Space]
    [Tooltip("Red.")]
    public ClampedFloatParameter rMultiplier = new ClampedFloatParameter(1f, -1f, 2f);
    [Tooltip("Green.")]
    public ClampedFloatParameter gMultiplier = new ClampedFloatParameter(1f, -1f, 2f);
    [Tooltip("Blue.")]
    public ClampedFloatParameter bMultiplier = new ClampedFloatParameter(0f, -1f, 2f);
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