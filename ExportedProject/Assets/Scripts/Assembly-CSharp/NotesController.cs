using System;
using UnityEngine;

public class NotesController : ItemController
{
	[Serializable]
	public class Objective
	{
		public string name;

		public GameObject gmObj;

		public GameObject markSucces;

		public GameObject nowApeared;

		public GameObject nowCompleted;

		public bool defActive;

		public bool defSucces;

		public bool activateAtStart;

		private bool _active = true;

		private bool _succes = true;

		public bool active
		{
			get
			{
				return _active;
			}
			set
			{
				if (_active != value)
				{
					gmObj.SetActive(value);
					_active = value;
				}
			}
		}

		public bool succes
		{
			get
			{
				return _succes;
			}
			set
			{
				if (_succes != value)
				{
					markSucces.SetActive(value);
					_succes = value;
				}
			}
		}

		public void SetDefaults()
		{
			active = defActive;
			succes = defSucces;
		}
	}

	public GameObject showButton;

	public GameObject newObjIco;

	public GameObject completeObjeIco;

	public Objective[] objectives;

	protected MenuController uiController;

	public bool isNotesInited;

	public Objective GetObjectiveById(string name)
	{
		Objective[] array = objectives;
		foreach (Objective objective in array)
		{
			if (objective.name == name)
			{
				return objective;
			}
		}
		return null;
	}

	public void SetObjectveActive(string name, bool value, bool mark = false)
	{
		Objective objectiveById = GetObjectiveById(name);
		if (objectiveById != null)
		{
			objectiveById.active = value;
			objectiveById.nowApeared.SetActive(mark);
		}
		else
		{
			Debug.LogError("Have no objectives with name " + name);
		}
	}

	public void SetObjectiveComplete(string name, bool value, bool mark = false)
	{
		Objective objectiveById = GetObjectiveById(name);
		if (objectiveById != null)
		{
			objectiveById.succes = value;
			objectiveById.nowCompleted.SetActive(mark);
		}
		else
		{
			Debug.LogError("Have no objectives with name " + name);
		}
	}

	public void ObjectiveComplete(string name)
	{
		if (!isNotesInited)
		{
			InitNotes();
		}
		SetObjectiveComplete(name, true, true);
		JackUtils.ControllActive(completeObjeIco, true);
		Triggering();
	}

	public void ObjectiveActivate(string name)
	{
		if (!isNotesInited)
		{
			InitNotes();
		}
		SetObjectveActive(name, true, true);
		JackUtils.ControllActive(newObjIco, true);
		if (name == "toilet key")
		{
			GameObject gameObject = Unical.Get("keyToilet");
			gameObject.SetActive(true);
			gameObject.GetComponent<RandomPlacement>().ResetPos();
		}
		if (name == "key")
		{
			GameObject gameObject2 = Unical.Get("keyKrasty");
			gameObject2.SetActive(true);
			gameObject2.GetComponent<RandomPlacement>().ResetPos();
		}
	}

	public void ResetNowMarks()
	{
		Objective[] array = objectives;
		foreach (Objective objective in array)
		{
			JackUtils.ControllActive(objective.nowApeared, false);
			JackUtils.ControllActive(objective.nowCompleted, false);
		}
	}

	public bool Obj(string name)
	{
		Objective objectiveById = GetObjectiveById(name);
		if (objectiveById != null)
		{
			return GetObjectiveById(name).succes;
		}
		return false;
	}

	public void Triggering()
	{
		if (Obj("office") && Obj("toilet") && Obj("cash_machine") && Obj("burgers"))
		{
			GetObjectiveById("office").active = false;
			GetObjectiveById("toilet").active = false;
			GetObjectiveById("cash_machine").active = false;
			GetObjectiveById("burgers").active = false;
			GetObjectiveById("toilet key").active = false;
			ObjectiveActivate("lock_all");
			ObjectiveActivate("key");
		}
	}

	public override bool PickUpItem(PickUpItem item)
	{
		UnityEngine.Object.Destroy(item.gameObject);
		uiController = MenuController.GetById("uiController");
		uiController.OnClickMenuItem("NotesUi");
		ActivateButton();
		return false;
	}

	public void OnShow()
	{
		JackUtils.ControllActive(newObjIco, false);
		JackUtils.ControllActive(completeObjeIco, false);
	}

	public void OnHide()
	{
		ResetNowMarks();
	}

	public void ActivateButton()
	{
		showButton.SetActive(true);
	}

	private void ResetAllObjectives()
	{
		Objective[] array = objectives;
		foreach (Objective objective in array)
		{
			objective.SetDefaults();
			if (objective.activateAtStart)
			{
				ObjectiveActivate(objective.name);
			}
		}
	}

	private void InitNotes()
	{
		isNotesInited = true;
		ResetAllObjectives();
		ActivateButton();
	}

	private void Start()
	{
		if (!isNotesInited)
		{
			InitNotes();
		}
	}

	private void Update()
	{
	}
}
