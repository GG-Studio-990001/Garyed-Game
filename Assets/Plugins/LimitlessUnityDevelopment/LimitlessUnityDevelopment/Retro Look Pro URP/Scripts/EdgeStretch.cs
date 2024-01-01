using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Retro Look Pro/Edge Stretch")]

public class EdgeStretch : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	public BoolParameter left = new BoolParameter(false);
	public BoolParameter right = new BoolParameter(false);
	public BoolParameter top = new BoolParameter(false);
	public BoolParameter bottom = new BoolParameter(true);

	[Tooltip("Height of Noise.")]
	public ClampedFloatParameter height = new ClampedFloatParameter(0.2f, 0.01f, 0.5f);
	[Space]
	[Tooltip("Stretch noise distortion.")]
	public BoolParameter distort = new BoolParameter(true);
	[Tooltip("Noise distortion frequency.")]
	public ClampedFloatParameter frequency = new ClampedFloatParameter(0.2f, 0.1f, 100f);
	[Tooltip("Noise distortion amplitude.")]
	public ClampedFloatParameter amplitude = new ClampedFloatParameter(0.2f, 0.0f, 0.5f);	
	[Tooltip("Noise distortion speed.")]
	public ClampedFloatParameter speed = new ClampedFloatParameter(0.2f, 0.0f, 50f);
	[Tooltip("Enable noise distortion random frequency.")]
	public BoolParameter distortRandomly = new BoolParameter(true);
    [Space]
    [Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
    public BoolParameter GlobalPostProcessingSettings = new BoolParameter(false);

    public bool IsActive() => (bool)enable;

    public bool IsTileCompatible() => false;
}