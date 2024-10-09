using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
	public static Message mess;

	public Text text;

	public Animator anim;

	protected const float defDelay = 1f;

	protected const float longDelay = 3f;

	private void Awake()
	{
		if (mess == null)
		{
			mess = this;
		}
	}

	public void InvokedFadeText()
	{
		anim.SetTrigger("Hide");
	}

	public void ShowMessage(string tex)
	{
		tex = tex.Translate();
		CancelInvoke();
		text.text = tex;
		anim.SetTrigger("Show");
		Invoke("InvokedFadeText", 1f);
	}

	public void ShowMessageLong(string tex)
	{
		tex = tex.Translate();
		CancelInvoke();
		text.text = tex;
		anim.SetTrigger("Show");
		Invoke("InvokedFadeText", 3f);
	}

	public static void Show(string tex)
	{
		if (mess != null)
		{
			mess.ShowMessage(tex);
		}
		else
		{
			Debug.LogError("static var is not ready");
		}
	}
}
