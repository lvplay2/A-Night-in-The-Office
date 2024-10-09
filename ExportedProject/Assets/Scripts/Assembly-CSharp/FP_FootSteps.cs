using System.Collections.Generic;
using UnityEngine;

public class FP_FootSteps : MonoBehaviour
{
	public AudioClip jumpSound;

	public AudioClip landSound;

	public List<Footsteps> footsteps = new List<Footsteps>();

	private FP_Controller playerController;

	private int randomStep;

	private void Start()
	{
		playerController = GetComponent<FP_Controller>();
	}

	public void PlayFootstepSounds(AudioSource audioSource)
	{
		for (int i = 0; i < footsteps.Count; i++)
		{
			if (footsteps[i].SurfaceTag == playerController.SurfaceTag())
			{
				randomStep = Random.Range(1, footsteps[i].stepSounds.Length);
				audioSource.clip = footsteps[i].stepSounds[randomStep];
				audioSource.Play();
				footsteps[i].stepSounds[randomStep] = footsteps[i].stepSounds[0];
				footsteps[i].stepSounds[0] = audioSource.clip;
			}
		}
	}

	public void ResetFootstepSounds(AudioSource audioSource)
	{
		for (int i = 0; i < footsteps.Count; i++)
		{
			if (footsteps[i].SurfaceTag == playerController.SurfaceTag())
			{
				audioSource.clip = footsteps[i].stepSounds[0];
				audioSource.Play();
			}
		}
	}
}
