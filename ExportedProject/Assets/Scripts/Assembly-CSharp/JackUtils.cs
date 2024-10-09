using UnityEngine;

public class JackUtils
{
	public delegate void SimpleFunc();

	public static void ControllActive(GameObject obj, bool value, SimpleFunc onValueDifferent = null)
	{
		if (!(obj == null) && obj.activeSelf != value)
		{
			obj.SetActive(value);
			if (onValueDifferent != null)
			{
				onValueDifferent();
			}
		}
	}

	public static void ControllEnabled(MonoBehaviour comp, bool value, SimpleFunc onValueDifferent = null)
	{
		if (!(comp == null) && comp.enabled != value)
		{
			comp.enabled = value;
			if (onValueDifferent != null)
			{
				onValueDifferent();
			}
		}
	}

	public static bool Contains(Collider collider, Vector3 point)
	{
		return false;
	}
}
