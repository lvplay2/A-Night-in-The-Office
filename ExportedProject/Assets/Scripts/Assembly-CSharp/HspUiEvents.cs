using UnityEngine;
using UnityEngine.SceneManagement;

public class HspUiEvents : MonoBehaviour
{
	public BlockingPlayer blockingPlayer;

	public void GotoMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void UnBlockPlayer()
	{
		if (blockingPlayer.disableObjects == null)
		{
			return;
		}
		GameObject[] disableObjects = blockingPlayer.disableObjects;
		foreach (GameObject gameObject in disableObjects)
		{
			if (gameObject != null)
			{
				JackUtils.ControllActive(gameObject, true);
			}
		}
	}

	public void Victory()
	{
		Unical.Get("Bob").GetComponent<Monster>().DisableBob();
		Unical.Get("chasing").GetComponent<SoundFader>().playFore = false;
		Unical.Get("Player").GetComponent<Player>().darkPlane.SetActive(true);
		MenuController.GetById("uiController").ShowMenuItem("VictoryUi");
	}
}
