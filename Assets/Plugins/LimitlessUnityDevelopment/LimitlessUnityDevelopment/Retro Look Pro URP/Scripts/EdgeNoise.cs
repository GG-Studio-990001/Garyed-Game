using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
[VolumeComponentMenu("Retro Look Pro/Edge Noise")]

public class EdgeNoise : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	public BoolParameter left = new BoolParameter(false);
	public BoolParameter right = new BoolParameter(false);
	public BoolParameter top = new BoolParameter(false);
	public BoolParameter bottom = new BoolParameter(true);

	[Range(0.01f, 0.5f), Tooltip("Noise Height.")]
	public ClampedFloatParameter height = new ClampedFloatParameter(0.2f, 0.01f, 0.5f);
	[Tooltip("Noise tiling.")]
	public Vector2Parameter tile = new Vector2Parameter(new Vector2(1, 1));
	[Range(0f, 3f), Tooltip("Noise intensity.")]
	public ClampedFloatParameter intencity = new ClampedFloatParameter(1.5f, 0f, 3f);

	public TextureParameter noiseTexture = new TextureParameter(null);
    [Space]
    [Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
    public BoolParameter GlobalPostProcessingSettings = new BoolParameter(false);

    public bool IsActive() => (bool)enable;

    public bool IsTileCompatible() => false;
}