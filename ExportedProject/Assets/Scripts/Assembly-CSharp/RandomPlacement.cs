using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
	public bool resetAtStart;

	public Transform[] spawns;

	public void ResetPos()
	{
		if (spawns != null && spawns.Length > 1)
		{
			int num = Random.Range(0, spawns.Length);
			Transform transform = spawns[num];
			base.transform.position = transform.position;
			base.transform.rotation = transform.rotation;
		}
	}

	private void Start()
	{
		if (resetAtStart)
		{
			ResetPos();
		}
	}
}
