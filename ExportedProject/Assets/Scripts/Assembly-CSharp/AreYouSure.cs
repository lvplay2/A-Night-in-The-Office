using UnityEngine;

public class AreYouSure : MonoBehaviour
{
	public GameObject inGameMenu;

	private void Show()
	{
		inGameMenu.SetActive(false);
		base.gameObject.SetActive(true);
	}

	private void Hide()
	{
		inGameMenu.SetActive(true);
		base.gameObject.SetActive(false);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
