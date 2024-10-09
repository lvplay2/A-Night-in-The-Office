using System;
using UnityEngine;

[Serializable]
public class MenuItem
{
	public string name;

	public GameObject gameObj;

	public MenuItemEvents events;

	private Animator anim;

	private void Init()
	{
		if (!anim && (bool)gameObj)
		{
			anim = gameObj.GetComponent<Animator>();
		}
	}

	public void Show()
	{
		Init();
		if ((bool)gameObj)
		{
			JackUtils.ControllActive(gameObj, true, delegate
			{
				events.onShow.Invoke();
			});
		}
	}

	public void Hide()
	{
		Init();
		if ((bool)gameObj)
		{
			JackUtils.ControllActive(gameObj, false, delegate
			{
				events.onHide.Invoke();
			});
		}
	}
}
