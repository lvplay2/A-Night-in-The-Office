using UnityEngine;

public class UnicalDestroy : MonoBehaviour
{
	public string[] destroyList = new string[0];

	private void Awake()
	{
		string[] array = destroyList;
		foreach (string text in array)
		{
			if (text != string.Empty)
			{
				if (Unical.all.ContainsKey(text))
				{
					Debug.Log("Unical.all.ContainsKey('" + text + "')");
					Object.Destroy(Unical.all[text]);
					Unical.all.Remove(text);
				}
				else
				{
					Debug.Log("Cant find unical game object '" + text + "'");
				}
			}
		}
	}
}
