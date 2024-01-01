using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using RetroLookPro.Enums;
[Serializable]
public sealed class BlendModeParameter : VolumeParameter<BlendingMode> { };

[VolumeComponentMenu("Retro Look Pro/VHS Effect")]

public class VHSEffect : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	[Tooltip("Color Offset.")]
	public ClampedFloatParameter colorOffset = new ClampedFloatParameter(4f, 0f, 50f);
	[Tooltip("Color Offset Angle.")]
	public ClampedFloatParameter colorOffsetAngle = new ClampedFloatParameter(1, -3f, 3f);
	[Space]
	[Tooltip("Vertical twitch frequency.")]
	public ClampedFloatParameter verticalOffsetFrequency = new ClampedFloatParameter(1f, 0f, 100f);
	[Tooltip("Amount of vertical twitch. ")]
	public ClampedFloatParameter verticalOffset = new ClampedFloatParameter(0.01f, 0f, 1f);
	[Tooltip("Amount of horizontal distortion.")]
	public ClampedFloatParameter offsetDistortion = new ClampedFloatParameter(0.008f, 0f, 0.5f);
	[Space]
	[Tooltip("Noise texture.")]
	public TextureParameter noiseTexture = new TextureParameter(null);
	public BlendModeParameter blendMode = new BlendModeParameter { };
	public Vector2Parameter tile = new Vector2Parameter(new Vector2(1, 1));
	[Space]
	[Tooltip("Intensity of noise texture.")]
	public ClampedFloatParameter _textureIntensity = new ClampedFloatParameter(1f, 0f, 1f);
	[Space]
	public BoolParameter smoothCut = new BoolParameter(false);
	[Tooltip("Amount of horizontal distortion.")]
	public ClampedIntParameter iterations = new ClampedIntParameter(5, 0, 30);
	[Tooltip("Amount of horizontal distortion.")]
	public ClampedFloatParameter smoothSize = new ClampedFloatParameter(0.1f, 0f, 0.5f);
	[Tooltip("Amount of horizontal distortion.")]
	public ClampedFloatParameter deviation = new ClampedFloatParameter(0.1f, 0f, 0.5f);
	[Space]
	[Tooltip("Cut off.")]
	public ClampedFloatParameter _textureCutOff = new ClampedFloatParameter(1f, -1f, 1f);
	[Space]
	[Tooltip("black bars")]
	public ClampedFloatParameter stripes = new ClampedFloatParameter(0.01f, 0.01f, 0.5f);
	[Space]
	public BoolParameter unscaledTime = new BoolParameter(false);
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