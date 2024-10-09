using System;
using UnityEngine.Events;

[Serializable]
public class DoorEvents
{
	public UnityEvent OnOpen = new UnityEvent();

	public UnityEvent OnClose = new UnityEvent();
}
