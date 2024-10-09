using UnityEngine;

public class ItemController : MonoBehaviour
{
	public virtual bool PickUpItem(PickUpItem item)
	{
		Debug.Log("PickUpItem " + item.id);
		if (item.id == "idDoor")
		{
			Door component = item.target.GetComponent<Door>();
			component.opened = !component.opened;
		}
		else if (item.id == "idKey")
		{
			Player.GiveKey(item.itemName);
			NotesController component2 = MenuController.GetById("uiController").GetItem("NotesUi").gameObj.GetComponent<NotesController>();
			component2.ObjectiveComplete(item.itemName);
			Object.Destroy(item.gameObject);
		}
		else if (item.id == "idHatchCover")
		{
			NotesController component3 = MenuController.GetById("uiController").GetItem("NotesUi").gameObj.GetComponent<NotesController>();
			NotesController.Objective objectiveById = component3.GetObjectiveById("key");
			if (objectiveById.active && !objectiveById.succes)
			{
				GameObject gameObject = GameObject.Find("KrabsSP_basement");
				GameObject gameObject2 = Unical.Get("Player");
				GameObject gameObject3 = Unical.Get("Bob");
				PlaySound component4 = Unical.Get("ambient").GetComponent<PlaySound>();
				if ((bool)gameObject && (bool)gameObject2)
				{
					gameObject2.GetComponent<Player>().spawnPoint = gameObject.transform;
					gameObject2.transform.position = gameObject.transform.position;
					gameObject3.GetComponent<Monster>().SetBasementParams();
					component4.Play("Basement");
				}
				else
				{
					Debug.Log("Something wrong " + (bool)gameObject + " " + (bool)gameObject2);
				}
			}
			else
			{
				Message.Show("Not now.");
			}
		}
		else if (item.id == "idLadder")
		{
			NotesController component5 = MenuController.GetById("uiController").GetItem("NotesUi").gameObj.GetComponent<NotesController>();
			NotesController.Objective objectiveById2 = component5.GetObjectiveById("key");
			if (objectiveById2.active && objectiveById2.succes)
			{
				GameObject gameObject4 = GameObject.Find("KrabsSP_krasty");
				GameObject gameObject5 = Unical.Get("Player");
				GameObject gameObject6 = Unical.Get("Bob");
				PlaySound component6 = Unical.Get("ambient").GetComponent<PlaySound>();
				if ((bool)gameObject4 && (bool)gameObject5)
				{
					gameObject5.GetComponent<Player>().spawnPoint = gameObject4.transform;
					gameObject5.transform.position = gameObject4.transform.position;
					gameObject6.GetComponent<Monster>().SetKrastyParams();
					component6.Play("Krasty");
				}
				else
				{
					Debug.Log("Something wrong " + (bool)gameObject4 + " " + (bool)gameObject5);
				}
			}
			else
			{
				Message.Show("Not now.");
			}
		}
		else if (item.id == "idLock")
		{
			MenuController.GetById("uiController").ShowMenuItem("VictoryUi");
		}
		return false;
	}

	private void Start()
	{
	}
}
