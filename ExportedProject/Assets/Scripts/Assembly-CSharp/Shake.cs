using System.Collections;
using UnityEngine;

[AddComponentMenu("Camera Shake System/Shake")]
public class Shake : MonoBehaviour
{
	public enum ShakeType
	{
		standard = 0,
		rumble = 1,
		explosion = 2,
		earthquake = 3,
		random = 4,
		custom = 5
	}

	private bool shaking;

	private bool startDecay;

	private bool addDecay;

	private float decay = 0.00025f;

	private float intensity = 0.032f;

	private float intensityLimit = 0.015f;

	private Vector3 initialPosition;

	private Quaternion initialRotation;

	private float initialIntensity;

	private float initialIntensityLimit;

	private float customDecay = 0.00025f;

	private float customIntensity = 0.032f;

	private float customIntensityLimit = 0.015f;

	private bool customAddDecay;

	private static Shake _instance;

	public static Shake Instance
	{
		get
		{
			return _instance;
		}
	}

	public static Shake GetInstance()
	{
		if (!_instance)
		{
			Debug.Log("Shake() - Please assign the script to the object in the scene before trying to access it.");
		}
		return _instance;
	}

	private void Awake()
	{
		_instance = this;
		initialPosition = base.transform.localPosition;
		initialRotation = Quaternion.Euler(Vector3.zero);
	}

	public bool GetShakeState()
	{
		return shaking;
	}

	public void StartShake(float shakeIntensity, float shakeDecay, float shakeLimit, bool shakeAddDecay)
	{
		customIntensity = ((!(shakeIntensity > 0f)) ? intensity : shakeIntensity);
		customIntensityLimit = ((!(shakeLimit > 0f)) ? intensityLimit : shakeLimit);
		customDecay = ((!(shakeDecay > 0f)) ? decay : shakeDecay);
		customAddDecay = shakeAddDecay;
		StartShake(ShakeType.custom);
	}

	public void StartShake()
	{
		StartShake(ShakeType.standard);
	}

	public void StartShake(ShakeType type)
	{
		initialRotation = base.transform.localRotation;
		initialPosition = base.transform.localPosition;
		if (!shaking)
		{
			switch (type)
			{
			case ShakeType.rumble:
				Rumble();
				break;
			case ShakeType.explosion:
				Explosion();
				break;
			case ShakeType.earthquake:
				Earthquake();
				break;
			case ShakeType.random:
				RandomShake();
				break;
			case ShakeType.custom:
				CustomShake();
				break;
			default:
				DefaultShake();
				break;
			}
		}
	}

	public void StopShake()
	{
		shaking = false;
		startDecay = false;
		intensity = initialIntensity;
		intensityLimit = initialIntensityLimit;
	}

	private IEnumerator BeginShake()
	{
		while (shaking)
		{
			base.transform.localPosition = initialPosition + Random.insideUnitSphere * intensity;
			base.transform.localRotation = new Quaternion(initialRotation.x + Random.Range(0f - intensity, intensity) * Random.value, initialRotation.y + Random.Range(0f - intensity, intensity) * Random.value, initialRotation.z + Random.Range(0f - intensity, intensity) * Random.value, initialRotation.w + Random.Range(0f - intensity, intensity) * Random.value);
			if (addDecay)
			{
				if (!startDecay)
				{
					intensity += decay;
					if (intensity >= intensityLimit)
					{
						startDecay = true;
					}
				}
				else
				{
					intensity -= decay;
				}
			}
			else
			{
				intensity -= decay;
			}
			if (intensity <= 0f)
			{
				StopShake();
			}
			yield return null;
		}
	}

	private void Rumble()
	{
		if (!shaking)
		{
			initialIntensity = intensity;
			initialIntensityLimit = intensityLimit;
			shaking = true;
			addDecay = true;
			intensityLimit = 0.015f;
			intensity = 0.000125f;
			decay = 8.25E-06f;
			StartCoroutine("BeginShake");
		}
	}

	private void Explosion()
	{
		if (!shaking)
		{
			initialIntensity = intensity;
			initialIntensityLimit = intensityLimit;
			shaking = true;
			addDecay = true;
			intensityLimit = 0.013f;
			intensity = 0.009f;
			decay = 0.0004f;
			StartCoroutine("BeginShake");
		}
	}

	private void Earthquake()
	{
		if (!shaking)
		{
			initialIntensity = intensity;
			initialIntensityLimit = intensityLimit;
			shaking = true;
			addDecay = true;
			intensityLimit = 0.015f;
			intensity = 0.000125f;
			decay = 8.25E-06f;
			StartCoroutine("BeginShake");
		}
	}

	private void RandomShake()
	{
		if (!shaking)
		{
			initialIntensity = intensity;
			initialIntensityLimit = intensityLimit;
			shaking = true;
			if (Random.Range(0f, 100f) > 50f)
			{
				addDecay = true;
			}
			else
			{
				addDecay = false;
			}
			intensityLimit = Random.Range(0.01f, 0.035f);
			intensity = Random.Range(0.00025f, 0.0055f);
			decay = Random.Range(8.25E-06f, 0.00015f);
			StartCoroutine("BeginShake");
		}
	}

	private void CustomShake()
	{
		if (!shaking)
		{
			initialIntensity = intensity;
			initialIntensityLimit = intensityLimit;
			shaking = true;
			addDecay = customAddDecay;
			intensity = customIntensity;
			intensityLimit = customIntensityLimit;
			decay = customDecay;
			StartCoroutine("BeginShake");
		}
	}

	private void DefaultShake()
	{
		if (!shaking)
		{
			initialIntensity = intensity;
			initialIntensityLimit = intensityLimit;
			shaking = true;
			addDecay = false;
			intensityLimit = 0.015f;
			intensity = 0.032f;
			decay = 0.00025f;
			StartCoroutine("BeginShake");
		}
	}
}
