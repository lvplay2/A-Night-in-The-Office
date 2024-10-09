using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Creature
{
	[Serializable]
	public class ResetInputs
	{
		public FP_Joystick movJoy;

		public FP_Lookpad lookPad;

		public FP_Button shoot;

		public FP_Button reload;

		public void Reset()
		{
			if ((bool)movJoy)
			{
				movJoy.Reset();
			}
			if ((bool)lookPad)
			{
				lookPad.Reset();
			}
			if ((bool)shoot)
			{
				shoot.Reset();
			}
			if ((bool)reload)
			{
				reload.Reset();
			}
		}
	}

	public Transform spawnPoint;

	public GameObject headObj;

	public GameObject headBox;

	public MenuController uiController;

	public GameObject darkPlane;

	public GameObject mainCam;

	public GameObject uiCam;

	public GameObject disableObj;

	public PlaySound voiceSounds;

	public bool invincible;

	public float hpForRessurect = 1f;

	public DamageArrow damageArrow;

	public static HashSet<string> keys = new HashSet<string>();

	public static HashSet<string> newKeys = new HashSet<string>();

	public ResetInputs resetInputs;

	protected float firstPainBreazeAfter;

	protected float repeatPainBreazeAfter;

	public const int maxRessurections = 2;

	public static int numberOfRessurections = 0;

	protected override void Start()
	{
		base.Start();
		keys = new HashSet<string>();
		newKeys = new HashSet<string>();
		Screen.sleepTimeout = -1;
		firstPainBreazeAfter = heals.hpMax / 2f;
		repeatPainBreazeAfter = heals.hpMax / 4f;
		numberOfRessurections = 0;
	}

	private void Update()
	{
	}

	public override void OnNotLethalStrike(float dmg)
	{
		base.OnNotLethalStrike(dmg);
		Debug.Log("OnNotLethalStrike");
		Invoke("LateTakeHit", 0.25f);
		if (heals.hp < firstPainBreazeAfter)
		{
			Invoke("LateInjuredBreaze", 2f);
		}
	}

	private void LateTakeHit()
	{
		if ((bool)voiceSounds)
		{
			voiceSounds.PlayRand("takeHit");
		}
	}

	private void LateInjuredBreaze()
	{
		CancelInvoke("LateInjuredBreaze");
		if (heals.hp < firstPainBreazeAfter)
		{
			if ((bool)voiceSounds)
			{
				voiceSounds.PlayRand("injuredBreaze");
			}
			if (heals.hp < repeatPainBreazeAfter)
			{
				Invoke("LateInjuredBreaze", 10f);
			}
		}
	}

	public override void TakeDamage(float damage, Transform damager)
	{
		if (!invincible)
		{
			if ((bool)damageArrow)
			{
				damageArrow.ShowDamageArrow(damager);
			}
			base.TakeDamage(damage, damager);
		}
	}

	public void OnMonsterDie()
	{
		Invoke("PlayerWin", 5f);
	}

	private void PlayerWin()
	{
		CancelInvoke();
		if ((bool)voiceSounds)
		{
			voiceSounds.Play(string.Empty);
		}
		resetInputs.Reset();
		uiController.OnClickMenuItem("VictoryUi");
		Difficult instance = Difficult.Instance;
		DifficultLevel selectedLevel = instance.GetSelectedLevel();
		if (instance.playerMaxLevelNum < instance.selectedLevelNum)
		{
			instance.playerMaxLevelNum = instance.selectedLevelNum;
		}
		Transform transform = uiController.GetItem("VictoryUi").gameObj.transform;
		transform.Find("txtWinningText").GetComponent<Text>().text = selectedLevel.winnigText;
	}

	public static void GiveKey(string keyValue)
	{
		keys.Add(keyValue);
	}

	public static void RemoveLastKeys()
	{
	}

	public static void ApplyKeys()
	{
	}

	public override void Die()
	{
		GameObject gameObject = Unical.Get("Bob");
		gameObject.GetComponent<PlaySound>().PlayRand("LongLaught");
		ChangePitch component = gameObject.GetComponent<ChangePitch>();
		component.lowVoice = true;
		RemoveLastKeys();
		CancelInvoke();
		if ((bool)voiceSounds)
		{
			voiceSounds.PlayRand("die");
		}
		resetInputs.Reset();
		uiController.ShowMenuItem("no item");
		Invoke("Darking", 0.7f);
		mainCam.GetComponent<DieCamAnim>().PlayAnim();
		GetComponent<FP_CameraLook>().enabled = false;
		SetHeadToBox();
		Invoke("DisablePlayer", 0.2f);
		Invoke("ShowGameOverMenu", 2f);
	}

	public void Darking()
	{
		darkPlane.SetActive(true);
	}

	private void SetHeadToBox()
	{
		headBox.SetActive(true);
		Transform transform = headBox.transform;
		Transform transform2 = mainCam.transform;
		Transform transform3 = uiCam.transform;
		transform.rotation = transform2.rotation;
		transform.position = transform2.position;
		transform2.parent = transform;
		transform2.localPosition = new Vector3(0f, 0f, 0f);
		transform2.localRotation = Quaternion.Euler(0f, 0f, 0f);
		transform3.parent = transform;
		transform3.localPosition = transform2.localPosition;
		transform3.localRotation = transform2.localRotation;
	}

	private void SetHeadToPlayer()
	{
		Transform parent = headObj.transform;
		Transform transform = mainCam.transform;
		Transform transform2 = uiCam.transform;
		transform.parent = parent;
		transform.localPosition = new Vector3(0f, 0f, 0f);
		transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		transform2.parent = parent;
		transform2.localPosition = transform.localPosition;
		transform2.localRotation = transform.localRotation;
		headBox.SetActive(false);
	}

	private void DisablePlayer()
	{
		disableObj.SetActive(false);
	}

	private void ShowGameOverMenu()
	{
		uiController.OnClickMenuItem("GameOverUi");
		Invoke("ShowHomeButton", 3f);
	}

	private void ShowHomeButton()
	{
	}

	public void GotoMenu_Settings()
	{
		PlayerPrefs.SetString("GotoMenuItem", "Settings");
		GotoMenu();
	}

	public void GotoMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void Ressurection()
	{
		GameObject gameObject = Unical.Get("Bob");
		ChangePitch component = gameObject.GetComponent<ChangePitch>();
		component.lowVoice = false;
		numberOfRessurections++;
		target.GetComponent<Monster>().OnPlayerRessurected();
		base.gameObject.SetActive(true);
		base.gameObject.transform.position = spawnPoint.position;
		base.gameObject.transform.rotation = spawnPoint.rotation;
		SetHeadToPlayer();
		Heal(hpForRessurect);
		GetComponent<FP_CameraLook>().enabled = true;
		mainCam.GetComponent<Shake>().StopShake();
		uiController.OnClickMenuItem("inGameUi");
		darkPlane.SetActive(false);
	}

	public override void Freeze(float time)
	{
	}
}
