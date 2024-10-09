using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MenuController
{
	public static MainMenuController inst;

	private bool Persist_OnStart()
	{
		if (inst == null)
		{
			inst = this;
			Object.DontDestroyOnLoad(base.gameObject);
			return true;
		}
		Object.Destroy(base.gameObject);
		return false;
	}

	private void Persist_OnDestroy()
	{
		if (inst == this)
		{
			inst = null;
		}
	}

	protected override void Start()
	{
		base.Start();
		Debug.Log("MainMenu.Start()");
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void OnClickPlay()
	{
		if (!isBusy)
		{
			OnClickMenuItem("Loading");
			Invoke("LoadGamePlayScene", 0.25f);
		}
	}

	public void OnClickExit()
	{
		Application.Quit();
	}

	private void LoadGamePlayScene()
	{
		SceneManager.LoadScene("KrastyKrabs");
	}

	public void OnClickSettings()
	{
	}
}
