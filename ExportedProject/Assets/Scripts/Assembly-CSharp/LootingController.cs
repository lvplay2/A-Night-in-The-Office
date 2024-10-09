using UnityEngine;
using UnityEngine.UI;

public class LootingController : ItemController
{
	public MenuController uiController;

	public Bar progress;

	public Text lootStr;

	public PlaySound sounds;

	public LootType[] types;

	protected LootType currLootType;

	protected float lootStart;

	protected float lootTime = 5f;

	protected PickUpItem pickUpComp;

	private bool looting_now;

	public LootType GetLootType(PickUpItem item)
	{
		LootType result = null;
		LootType[] array = types;
		foreach (LootType lootType in array)
		{
			if (lootType.name == item.id)
			{
				result = lootType;
				break;
			}
		}
		return result;
	}

	public override bool PickUpItem(PickUpItem item)
	{
		LootType lootType = GetLootType(item);
		if (lootType != null)
		{
			currLootType = lootType;
			pickUpComp = item;
			BeginLooting();
		}
		return false;
	}

	private void BeginLooting()
	{
		uiController.OnClickMenuItem("LootingUi");
		looting_now = true;
		lootStart = Time.time;
		lootStr.text = currLootType.lootStr;
		Invoke("PlayLootSound", 0f);
	}

	public void LootingCancel()
	{
		sounds.Play(string.Empty);
		uiController.OnClickMenuItem("inGameUi");
	}

	public void LootingFinish()
	{
		sounds.Play(string.Empty);
		uiController.OnClickMenuItem("inGameUi");
		pickUpComp.callEvent.Invoke();
		Object.Destroy(pickUpComp.gameObject);
	}

	public void PlayLootSound()
	{
		if (looting_now)
		{
			sounds.PlayRand(currLootType.lootSoundName);
			Invoke("PlayLootSound", currLootType.soundsDelay);
		}
	}

	public void OnLootingHide()
	{
		looting_now = false;
		sounds.PlayRand(string.Empty);
		CancelInvoke();
	}

	private void Start()
	{
	}

	private void Update()
	{
		float num = lootStart + lootTime;
		if (looting_now)
		{
			if (Time.time < num)
			{
				float num2 = num - Time.time;
				progress.SetValue(num2 / lootTime);
			}
			else
			{
				LootingFinish();
			}
		}
	}
}
