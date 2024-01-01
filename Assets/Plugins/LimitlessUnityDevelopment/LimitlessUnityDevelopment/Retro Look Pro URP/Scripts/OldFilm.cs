using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Retro Look Pro/OldFilm")]

public class OldFilm : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	[Range(0f, 60f), Tooltip("Frames per second.")]
	public ClampedFloatParameter fps = new ClampedFloatParameter(1f, 0f, 60f);
	[Range(0f, 5f), Tooltip(".")]
	public ClampedFloatParameter contrast = new ClampedFloatParameter(1f, 0f, 5f);

	[Range(-2f, 4f), Tooltip("Image burn.")]
	public ClampedFloatParameter burn = new ClampedFloatParameter(0.88f, -2f, 4f);
	[Range(0f, 16f), Tooltip("Scene cut off.")]
	public ClampedFloatParameter sceneCut = new ClampedFloatParameter(0.88f, 0f, 16f);
	[Range(0f, 1f), Tooltip("Effect fade.")]
	public ClampedFloatParameter fade = new ClampedFloatParameter(0.88f, 0f, 1f);
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