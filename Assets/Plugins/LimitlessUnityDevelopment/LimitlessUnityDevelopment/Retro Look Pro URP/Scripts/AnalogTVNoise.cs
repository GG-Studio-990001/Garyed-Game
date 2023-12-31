using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;
using System;

[Serializable]
public sealed class maskChannelModeParameter : VolumeParameter<maskChannelMode> { };
[VolumeComponentMenu("Retro Look Pro/Analog TV Noise")]
public class AnalogTVNoise : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	[Tooltip("Option enables static noise (without movement).")]
	public BoolParameter staticNoise = new BoolParameter(false);
	[Tooltip("Horizontal/Vertical Noise lines.")]
	public BoolParameter Horizontal = new BoolParameter(true);
	[Range(0f, 1f), Tooltip("Effect Fade.")]
	public ClampedFloatParameter Fade = new ClampedFloatParameter(1f, 0f, 1f);
	[Range(0f, 60f), Tooltip("Noise bar width.")]
	public ClampedFloatParameter barWidth = new ClampedFloatParameter(21f, 0f, 60f);
	[Range(0f, 60f), Tooltip("Noise tiling.")]
	public Vector2Parameter tile = new Vector2Parameter(new Vector2(1, 1));
	[Range(0f, 1f), Tooltip("Noise texture angle.")]
	public ClampedFloatParameter textureAngle = new ClampedFloatParameter(1f, 0f, 1f);
	[Range(0f, 100f), Tooltip("Noise bar edges cutoff.")]
	public ClampedFloatParameter edgeCutOff = new ClampedFloatParameter(0f, 0f, 100f);
	[Range(-1f, 1f), Tooltip("Noise cutoff.")]
	public ClampedFloatParameter CutOff = new ClampedFloatParameter(1f, -1f, 1f);
	[Range(-10f, 10f), Tooltip("Noise bars speed.")]
	public ClampedFloatParameter barSpeed = new ClampedFloatParameter(1f, -60f, 60f);
	[Tooltip("Noise texture.")]
	public TextureParameter texture = new TextureParameter(null);
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