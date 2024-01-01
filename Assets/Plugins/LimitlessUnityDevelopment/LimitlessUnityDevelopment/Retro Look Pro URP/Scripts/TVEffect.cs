using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using RetroLookPro.Enums;

[Serializable]
public sealed class WarpModeParameter : VolumeParameter<WarpMode> { };
[VolumeComponentMenu("Retro Look Pro/TV Effect")]

public class TVEffect : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
	[Range(0f, 1f), Tooltip("Effect fade.")]
	public ClampedFloatParameter fade = new ClampedFloatParameter(1, 0, 1);
	[Range(0f, 2f), Tooltip("Dark areas adjustment.")]
	public ClampedFloatParameter maskDark = new ClampedFloatParameter(0.5f, 0, 2f);
	[Range(0f, 2f), Tooltip("Light areas adjustment.")]
	public ClampedFloatParameter maskLight = new ClampedFloatParameter(1.5f, 0, 2f);
	[Range(-8f, -16f), Tooltip("Dark areas fine tune.")]
	public ClampedFloatParameter hardScan = new ClampedFloatParameter(-8f, -8f, 16f);
    [Space]
	[Range(1f, 16f), Tooltip("Effect resolution.")]
	public ClampedFloatParameter resScale = new ClampedFloatParameter(4f, 1f, 16f);
    [Tooltip("Correct effect resolution, depending on screen resolution")]
    public BoolParameter ScaleWithActualScreenSize = new BoolParameter(false);
    [Space]
	[Range(-3f, 1f), Tooltip("pixels sharpness.")]
	public ClampedFloatParameter hardPix = new ClampedFloatParameter(-3f, -3f, 1f);
	[Tooltip("Warp mode.")]
	public WarpModeParameter warpMode = new WarpModeParameter { };
	[Tooltip("Warp picture.")]
	public Vector2Parameter warp = new Vector2Parameter(new Vector2(0f, 0f));
    public FloatParameter scale = new FloatParameter(0.5f);
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