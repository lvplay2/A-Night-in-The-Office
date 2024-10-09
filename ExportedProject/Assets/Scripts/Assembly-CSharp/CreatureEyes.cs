using UnityEngine;

public class CreatureEyes : MonoBehaviour
{
	public Transform testTarget;

	public Transform eyesTransform;

	public float horAngle = 90f;

	public float distance = 10f;

	public LayerMask layers;

	private Transform _thisTransform;

	private Transform thisTransform
	{
		get
		{
			if (_thisTransform == null)
			{
				_thisTransform = base.transform;
			}
			return _thisTransform;
		}
		set
		{
			_thisTransform = value;
		}
	}

	public Transform GetEyesTransfrom()
	{
		return (!eyesTransform) ? thisTransform : eyesTransform;
	}

	public bool IsCreatureVisible(Transform target)
	{
		CreatureVisible component = target.GetComponent<CreatureVisible>();
		if (component == null)
		{
			Debug.LogError("CreatureVisible is not exiss on target");
			return false;
		}
		return component.IsCreatureVisibleFor(this);
	}

	private void Update()
	{
		if ((bool)testTarget)
		{
			Debug.Log(">> " + IsCreatureVisible(testTarget));
		}
	}
}
