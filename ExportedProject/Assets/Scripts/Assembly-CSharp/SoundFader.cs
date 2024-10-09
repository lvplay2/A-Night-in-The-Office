using UnityEngine;

public class SoundFader : MonoBehaviour
{
	public bool playFore;

	public float fadeTime = 2f;

	public AudioSource back;

	public AudioSource fore;

	public float backMinVol;

	public float backMaxVol = 1f;

	public float foreMinVol;

	public float foreMaxVol = 1f;

	private void Update()
	{
		if (playFore)
		{
			back.volume = Mathf.MoveTowards(back.volume, backMinVol, 1f / fadeTime * Time.deltaTime);
			fore.volume = Mathf.MoveTowards(fore.volume, foreMaxVol, 1f / fadeTime * Time.deltaTime);
		}
		else
		{
			back.volume = Mathf.MoveTowards(back.volume, backMaxVol, 1f / fadeTime * Time.deltaTime);
			fore.volume = Mathf.MoveTowards(fore.volume, foreMinVol, 1f / fadeTime * Time.deltaTime);
		}
	}
}
