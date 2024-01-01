using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public enum FisheyeTypeEnum { Default = 0, Hyperspace = 1 }
[Serializable]
public sealed class FisheyeTypeParameter : VolumeParameter<FisheyeTypeEnum> { };

[VolumeComponentMenu("Retro Look Pro/Fisheye")]

public class Fisheye : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	public FisheyeTypeParameter fisheyeType = new FisheyeTypeParameter { };
	[Range(0f, 50f), Tooltip("Bend Amount.")]
	public ClampedFloatParameter bend = new ClampedFloatParameter(1f, 0f, 50f);
	[Range(0f, 50f), Tooltip("Cutoff on X axes.")]
	public ClampedFloatParameter cutOffX = new ClampedFloatParameter(0.5f, 0f, 50f);
	[Range(0f, 50f), Tooltip("Cutoff on Y axes.")]
	public ClampedFloatParameter cutOffY = new ClampedFloatParameter(0.5f, 0f, 50f);
	[Range(0f, 50f), Tooltip("Fade on X axes.")]
	public ClampedFloatParameter fadeX = new ClampedFloatParameter(1f, 0f, 50f);
	[Range(0f, 50f), Tooltip("Fade on Y axes.")]
	public ClampedFloatParameter fadeY = new ClampedFloatParameter(1f, 0f, 50f);
	[Range(0.001f, 50f), Tooltip("Fisheye size.")]
	public ClampedFloatParameter size = new ClampedFloatParameter(1f, 0.001f, 50f);
    [Space]
    [Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
    public BoolParameter GlobalPostProcessingSettings = new BoolParameter(false);

    public bool IsActive() => (bool)enable;

    public bool IsTileCompatible() => false;
}