using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Retro Look Pro/Noise")]

public class Noise : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	[Tooltip("stretch Resolution")]
	public FloatParameter stretchResolution = new FloatParameter(480f);
	[Tooltip("Vertical Resolution")]
	public FloatParameter VerticalResolution = new FloatParameter(480f);
	[Space]
	[Space]
	[Tooltip("Granularity")]
	public BoolParameter Granularity = new BoolParameter(false);
	[Tooltip("Granularity Amount")]
	public ClampedFloatParameter GranularityAmount = new ClampedFloatParameter(0.5f, 0f, 0.5f);
	[Space]
	[Tooltip("Tape Noise")]
	public BoolParameter TapeNoise = new BoolParameter(false);
	[Tooltip("Tape Noise Signal Processing")]
	public ClampedFloatParameter TapeNoiseSignalProcessing = new ClampedFloatParameter(1f, 0f, 15f);
	[Tooltip("Tape Noise Fade")]
	public ClampedFloatParameter TapeNoiseFade = new ClampedFloatParameter(1f, 0f, 1.5f);
	[Tooltip("Tape Noise Amount(lower value = more noise)")]
	public ClampedFloatParameter TapeNoiseAmount = new ClampedFloatParameter(1f, 0f, 1.5f);
	[Tooltip("tape Lines Amount")]
	public ClampedFloatParameter tapeLinesAmount = new ClampedFloatParameter(0.8f, 0f, 1f);
	[Tooltip("Tape Noise Speed")]
	public ClampedFloatParameter TapeNoiseSpeed = new ClampedFloatParameter(0.5f, -1.5f, 1.5f);
	[Space]
	[Tooltip("Line Noise")]
	public BoolParameter LineNoise = new BoolParameter(false);
	[Tooltip("Line Noise Amount")]
	public ClampedFloatParameter LineNoiseAmount = new ClampedFloatParameter(1f, 0f, 15f);
	[Tooltip("Line Noise Speed")]
	public ClampedFloatParameter LineNoiseSpeed = new ClampedFloatParameter(1f, 0f, 10f);
	[Space]
	[Tooltip("Signal Noise")]
	public BoolParameter SignalNoise = new BoolParameter(false);
	[Tooltip("Signal Noise Power")]
	public ClampedFloatParameter SignalNoisePower = new ClampedFloatParameter(0.9f, 0.5f, 0.97f);
	[Tooltip("Signal Noise Amount")]
	public ClampedFloatParameter SignalNoiseAmount = new ClampedFloatParameter(1f, 0f, 2f);
	[Space]
	[Tooltip("Mask texture")]
	public TextureParameter mask = new TextureParameter(null);
	public maskChannelModeParameter maskChannel = new maskChannelModeParameter();

	[Tooltip("Time.unscaledTime.")]
	public BoolParameter unscaledTime = new BoolParameter(false);
    [Space]
    [Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
    public BoolParameter GlobalPostProcessingSettings = new BoolParameter(false);


    public bool IsActive() => (bool)enable;

    public bool IsTileCompatible() => false;
}