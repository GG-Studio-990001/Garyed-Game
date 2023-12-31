using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
[VolumeComponentMenu("Retro Look Pro/VHS Tape Rewind")]

public class VHSTapeRewind : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);

	public ClampedFloatParameter intencity = new ClampedFloatParameter(0.57f, 0, 5);
	public ClampedFloatParameter fade = new ClampedFloatParameter(1f, 0, 1);
    [Space]
    [Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
    public BoolParameter GlobalPostProcessingSettings = new BoolParameter(false);


    public bool IsActive() => (bool)enable;

    public bool IsTileCompatible() => false;

}
