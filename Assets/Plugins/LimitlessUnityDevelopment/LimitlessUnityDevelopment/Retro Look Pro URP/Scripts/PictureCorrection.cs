using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;


[VolumeComponentMenu("Retro Look Pro/Picture Correction")]

public class PictureCorrection : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	[Range(-0.25f, 0.25f), Tooltip(" Y permanent adjustment..")]
	public ClampedFloatParameter signalAdjustY = new ClampedFloatParameter(0f, -0.25f, 0.25f);
	[Range(-0.25f, 0.25f), Tooltip("I permanent adjustment..")]
	public ClampedFloatParameter signalAdjustI = new ClampedFloatParameter(0f, -0.25f, 0.25f);
	[Range(-0.25f, 0.25f), Tooltip("Q permanent adjustment..")]
	public ClampedFloatParameter signalAdjustQ = new ClampedFloatParameter(0f, -0.25f, 0.25f);
	[Range(-2f, 2f), Tooltip("tweak/shift Y values..")]
	public ClampedFloatParameter signalShiftY = new ClampedFloatParameter(1f, -2f, 2f);
	[Range(-2f, 2f), Tooltip("tweak/shift I values..")]
	public ClampedFloatParameter signalShiftI = new ClampedFloatParameter(1f, -2f, 2f);
	[Range(-2f, 2f), Tooltip("tweak/shift Q values..")]
	public ClampedFloatParameter signalShiftQ = new ClampedFloatParameter(1f, -2f, 2f);
	[Range(0f, 2f), Tooltip("use this to balance the gamma(brightness) of the signal.")]
	public ClampedFloatParameter gammaCorection = new ClampedFloatParameter(1f, -0f, 2f);
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