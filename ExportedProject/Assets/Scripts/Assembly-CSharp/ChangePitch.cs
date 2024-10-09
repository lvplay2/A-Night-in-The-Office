using UnityEngine;

public class ChangePitch : MonoBehaviour
{
	public AudioSource source;

	public float pitchNorm = 1f;

	public float pitchMin = 0.55f;

	public float changeTime = 1.5f;

	public bool lowVoice;

	private void Update()
	{
		if (lowVoice)
		{
			source.pitch = Mathf.MoveTowards(source.pitch, pitchMin, (pitchNorm - pitchMin) / changeTime * Time.deltaTime);
		}
		else
		{
			source.pitch = Mathf.MoveTowards(source.pitch, pitchNorm, pitchNorm - pitchMin);
		}
	}
}
