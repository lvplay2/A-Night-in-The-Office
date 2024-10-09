using System;
using UnityEngine.Events;

[Serializable]
public class MenuItemEvents
{
	public UnityEvent onShow = new UnityEvent();

	public UnityEvent onHide = new UnityEvent();
}
