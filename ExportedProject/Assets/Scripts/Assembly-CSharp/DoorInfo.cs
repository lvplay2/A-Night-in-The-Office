using System;
using UnityEngine;

[Serializable]
public class DoorInfo
{
	public Transform door;

	public Transform rotOpened;

	public Transform rotClosed;

	public Collider doorCollider;
}
