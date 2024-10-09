using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	protected static SortedDictionary<string, MenuController> all = new SortedDictionary<string, MenuController>();

	public string menuName = string.Empty;

	public string currItemName;

	public MenuItem[] items;

	protected bool isBusy;

	protected MenuItem curr;

	public MenuItem GetItem(string name)
	{
		MenuItem[] array = items;
		foreach (MenuItem menuItem in array)
		{
			if (menuItem.name == name)
			{
				return menuItem;
			}
		}
		return null;
	}

	public void ShowMenuItem(string name)
	{
		MenuItem[] array = items;
		foreach (MenuItem menuItem in array)
		{
			if (menuItem.name != name)
			{
				menuItem.Hide();
			}
		}
		MenuItem[] array2 = items;
		foreach (MenuItem menuItem2 in array2)
		{
			if (menuItem2.name == name)
			{
				menuItem2.Show();
				curr = menuItem2;
			}
		}
	}

	public void OnClickMenuItem(string name)
	{
		ShowMenuItem(name);
	}

	public void RegistrateMeInAll()
	{
		if (menuName != string.Empty && !all.ContainsKey(menuName))
		{
			all.Add(menuName, this);
		}
	}

	public void DeleteMeFromAll()
	{
		if (menuName != string.Empty && all.ContainsKey(menuName) && all[menuName] == this)
		{
			all.Remove(menuName);
		}
	}

	public static MenuController GetById(string id)
	{
		if (all.ContainsKey(id))
		{
			return all[id];
		}
		return null;
	}

	protected virtual void Start()
	{
		RegistrateMeInAll();
		if (currItemName != string.Empty)
		{
			OnClickMenuItem(currItemName);
		}
	}

	protected virtual void OnDestroy()
	{
		DeleteMeFromAll();
	}
}
