using UnityEngine;

public class Door : MonoBehaviour
{
	public string keyName = string.Empty;

	public string objName = string.Empty;

	public PlaySound sound;

	public DoorInfo[] doors;

	public DoorEvents events = new DoorEvents();

	protected bool _opened;

	public bool opened
	{
		get
		{
			return _opened;
		}
		set
		{
			bool flag = keyName != string.Empty;
			bool flag2 = Player.keys.Contains(keyName);
			bool flag3 = flag && !flag2;
			if (value)
			{
				if (!flag3)
				{
					events.OnOpen.Invoke();
					MakeDoorsOpen(true);
					Sound("op");
					_opened = true;
					return;
				}
				Sound("bl");
				if (objName != string.Empty)
				{
					NotesController component = MenuController.GetById("uiController").GetItem("NotesUi").gameObj.GetComponent<NotesController>();
					component.ObjectiveActivate(objName);
					Message.Show("Door Locked. Find a key.");
				}
				else
				{
					Message.Show("Door Locked.");
				}
			}
			else
			{
				if (value)
				{
					return;
				}
				if (!flag3)
				{
					events.OnClose.Invoke();
					MakeDoorsOpen(false);
					Sound("cl");
					_opened = false;
					return;
				}
				Sound("bl");
				if (objName != string.Empty)
				{
					NotesController component2 = MenuController.GetById("uiController").GetItem("NotesUi").gameObj.GetComponent<NotesController>();
					component2.ObjectiveActivate(objName);
					Message.Show("Door Locked. Find a key.");
				}
				else
				{
					Message.Show("Door Locked.");
				}
			}
		}
	}

	public void Sound(string sndName)
	{
		if ((bool)sound)
		{
			sound.PlayRand(sndName);
		}
	}

	public void MakeDoorsOpen(bool open)
	{
		DoorInfo[] array = doors;
		foreach (DoorInfo doorInfo in array)
		{
			if (open)
			{
				doorInfo.door.rotation = doorInfo.rotOpened.rotation;
				doorInfo.door.position = doorInfo.rotOpened.position;
				if ((bool)doorInfo.doorCollider)
				{
					doorInfo.doorCollider.isTrigger = true;
				}
				_opened = true;
			}
			else
			{
				doorInfo.door.rotation = doorInfo.rotClosed.rotation;
				doorInfo.door.position = doorInfo.rotClosed.position;
				if ((bool)doorInfo.doorCollider)
				{
					doorInfo.doorCollider.isTrigger = false;
				}
				_opened = false;
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
