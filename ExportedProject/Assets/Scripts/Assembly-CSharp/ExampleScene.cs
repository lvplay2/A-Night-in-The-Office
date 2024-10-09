using System.Collections;
using UnityEngine;

public class ExampleScene : MonoBehaviour
{
	private Shake.ShakeType shakeType;

	private Shake cameraShake;

	private Shake objectShake;

	public GUITexture[] guiTextures;

	public GUITexture[] controlTextures;

	public GameObject[] guiValues;

	public GUIText valueText;

	public GUIText decayText;

	public Texture2D buttonUp;

	public Texture2D buttonDown;

	public GameObject cameraObject;

	public GameObject cubeObject;

	private float maxShake = 0.05f;

	private float shakeAmount = 5f;

	private float shakeIncrement = 5f;

	private float addDecayIntensity = 0.0001f;

	private bool targetIsCamera = true;

	private bool addDecay;

	private void Start()
	{
		cameraShake = cameraObject.GetComponent(typeof(Shake)) as Shake;
		objectShake = cubeObject.GetComponent(typeof(Shake)) as Shake;
	}

	private void Update()
	{
		if (!Input.GetMouseButtonUp(0))
		{
			return;
		}
		if (guiTextures[0].HitTest(Input.mousePosition))
		{
			StartShake();
			StartCoroutine(WaitForTexture(guiTextures[0], 0.2f));
		}
		if (guiTextures[1].HitTest(Input.mousePosition))
		{
			cameraShake.StopShake();
			objectShake.StopShake();
			StartCoroutine(WaitForTexture(guiTextures[1], 0.2f));
		}
		if (guiTextures[2].HitTest(Input.mousePosition))
		{
			shakeType = Shake.ShakeType.standard;
			ResetTextures(2);
		}
		if (guiTextures[3].HitTest(Input.mousePosition))
		{
			shakeType = Shake.ShakeType.rumble;
			ResetTextures(3);
		}
		if (guiTextures[4].HitTest(Input.mousePosition))
		{
			shakeType = Shake.ShakeType.explosion;
			ResetTextures(4);
		}
		if (guiTextures[5].HitTest(Input.mousePosition))
		{
			shakeType = Shake.ShakeType.earthquake;
			ResetTextures(5);
		}
		if (guiTextures[6].HitTest(Input.mousePosition))
		{
			shakeType = Shake.ShakeType.custom;
			ResetTextures(6);
			ToggleValueObjects(true);
		}
		if (guiTextures[7].HitTest(Input.mousePosition))
		{
			shakeType = Shake.ShakeType.random;
			ResetTextures(7);
		}
		if (guiTextures[8].HitTest(Input.mousePosition))
		{
			StartCoroutine(WaitForTexture(guiTextures[8], 0.05f));
			shakeAmount += shakeIncrement;
			if (shakeAmount > 100f)
			{
				shakeAmount = shakeIncrement;
			}
			valueText.text = shakeAmount + "%";
		}
		if (guiTextures[9].HitTest(Input.mousePosition))
		{
			StartCoroutine(WaitForTexture(guiTextures[9], 0.15f));
			addDecay = !addDecay;
			decayText.text = ((!addDecay) ? "IMMEDIATE" : "DELAY");
		}
		if (controlTextures[0].HitTest(Input.mousePosition))
		{
			targetIsCamera = true;
			ToggleShakeTarget(0);
		}
		if (controlTextures[1].HitTest(Input.mousePosition))
		{
			targetIsCamera = false;
			ToggleShakeTarget(1);
		}
	}

	private void StartShake()
	{
		Shake shake = ((!targetIsCamera) ? objectShake : cameraShake);
		if (shakeType == Shake.ShakeType.custom)
		{
			if (addDecay)
			{
				shake.StartShake(addDecayIntensity, 0f, maxShake * (shakeAmount / 100f), true);
			}
			else
			{
				shake.StartShake(maxShake * (shakeAmount / 100f), 0f, 0f, false);
			}
		}
		else
		{
			shake.StartShake(shakeType);
		}
	}

	private IEnumerator WaitForTexture(GUITexture guiTexture, float timeDelay)
	{
		guiTexture.texture = buttonDown;
		yield return new WaitForSeconds(timeDelay);
		guiTexture.texture = buttonUp;
	}

	private void ToggleShakeTarget(int shakeTarget)
	{
		controlTextures[shakeTarget].texture = buttonDown;
		for (int i = 0; i < controlTextures.Length; i++)
		{
			if (i != shakeTarget)
			{
				controlTextures[i].texture = buttonUp;
			}
		}
	}

	private void ToggleValueObjects(bool state)
	{
		for (int i = 0; i < guiValues.Length; i++)
		{
			guiValues[i].SetActive(state);
		}
	}

	private void ResetTextures(int skipTexture)
	{
		guiTextures[skipTexture].texture = buttonDown;
		for (int i = 2; i < guiTextures.Length; i++)
		{
			if (i != skipTexture)
			{
				guiTextures[i].texture = buttonUp;
			}
		}
		if (skipTexture != 8 && skipTexture != 9)
		{
			ToggleValueObjects(false);
		}
	}
}
