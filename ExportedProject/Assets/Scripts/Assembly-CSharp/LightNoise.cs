using UnityEngine;

public class LightNoise : MonoBehaviour
{
	public Light useLight;

	public bool enableNoise;

	public bool random = true;

	public bool takeIntesityAtStart = true;

	public float normalIntensity = 5f;

	public float noiseMin = 0.5f;

	public float noiseMax = 1f;

	public float noiseSpd = 30f;

	protected float inTarget;

	private void Start()
	{
		if (takeIntesityAtStart)
		{
			normalIntensity = useLight.intensity;
		}
	}

	private void Update()
	{
		if (enableNoise)
		{
			if (useLight.intensity == inTarget)
			{
				if (random)
				{
					inTarget = Random.Range(noiseMin * normalIntensity, noiseMax * normalIntensity);
				}
				else
				{
					inTarget = ((inTarget != noiseMin) ? noiseMin : noiseMax);
				}
			}
			else
			{
				useLight.intensity = Mathf.MoveTowards(useLight.intensity, inTarget, noiseSpd * Time.deltaTime);
			}
		}
		else if (useLight.intensity != normalIntensity)
		{
			useLight.intensity = normalIntensity;
		}
	}
}
