using UnityEngine;

public class DieCamAnim : MonoBehaviour
{
	private bool die_rotation;

	private Quaternion die_rot;

	private Transform fl;

	public void PlayAnim()
	{
		fl = base.transform.Find("FlashLight");
		Transform transform = Unical.Get("Bob").transform;
		Vector3 normalized = (transform.position - base.transform.position).normalized;
		normalized.y = 0f;
		die_rot = Quaternion.LookRotation(normalized);
		die_rotation = true;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (die_rotation)
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, die_rot, 360f * Time.deltaTime);
			if (base.transform.rotation == die_rot)
			{
				die_rotation = false;
				Shake component = GetComponent<Shake>();
				component.StartShake(Shake.ShakeType.explosion);
			}
		}
	}
}
