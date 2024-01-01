using UnityEngine;
using UnityEngine.Rendering;
//
public class EffectsManipulationExample : MonoBehaviour
{
	// Your Post-processing volume with Glitch1 effect;
	public Volume volume;

	// Temp Glitch effect.
	private LimitlessGlitch1 m_Glitch;

	private void Start()
	{
		//Null check
		if (volume == null)
			return;

		// Get refference to glitch effect
		volume.profile.TryGet(out m_Glitch);

		//Null check
		if (m_Glitch is null)
		{
			Debug.Log("Add Glitch1 effect to your Volume component to make Manipulation Example work");
			return;
		}

		//Activate effect
		m_Glitch.active = true;
	}
	private void FixedUpdate()
	{
		//Null check
		if (volume == null)
			return;
		if (m_Glitch is null)
			return;

		// Randomly change glitch value
		m_Glitch.bMultiplier.value = UnityEngine.Random.Range(-2f, 2f);
	}
}
