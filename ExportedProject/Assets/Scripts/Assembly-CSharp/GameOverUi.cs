using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUi : MonoBehaviour
{
	public GameObject btnRessurect;

	public GameObject btnHome;

	public Text count;

	public bool ShowRessurect = true;

	public bool ShowHome = true;

	public float ShowHomeDelay = 3f;

	private void Start()
	{
	}

	public void Show()
	{
		if ((bool)count)
		{
			count.text = string.Empty + (2 - Player.numberOfRessurections);
		}
		if ((bool)btnRessurect)
		{
			if (ShowRessurect && 2 > Player.numberOfRessurections)
			{
				btnRessurect.SetActive(true);
			}
			else
			{
				btnRessurect.SetActive(false);
			}
		}
		if ((bool)btnHome)
		{
			btnHome.SetActive(false);
			if (ShowHome)
			{
				Invoke("ShowHomeLater", ShowHomeDelay);
			}
		}
	}

	public void OnClickExit()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void ShowHomeLater()
	{
		btnHome.SetActive(true);
	}
}
