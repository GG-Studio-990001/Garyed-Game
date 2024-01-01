using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Retro Look Pro/Cinematic Bars")]

public class CinematicBars : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	[Range(0.5f, 0.01f), Tooltip("Black bars amount (width)")]
	public ClampedFloatParameter amount = new ClampedFloatParameter(0.5f, 0.01f, 0.51f);
	[Range(0f, 1f), Tooltip("Fade black bars.")]
	public ClampedFloatParameter fade = new ClampedFloatParameter(1f, 0f, 1f);
    [Space]
    [Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
    public BoolParameter GlobalPostProcessingSettings = new BoolParameter(false);

    public bool IsActive() => (bool)enable;

    public bool IsTileCompatible() => false;
}