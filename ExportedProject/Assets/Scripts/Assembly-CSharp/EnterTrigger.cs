using UnityEngine;
using UnityEngine.Events;

public class EnterTrigger : MonoBehaviour
{
	public Collider targetCollider;

	public UnityEvent onEnter = new UnityEvent();

	public void OnTriggerEnter(Collider other)
	{
		if (other == targetCollider && base.enabled)
		{
			onEnter.Invoke();
		}
	}

	private void Start()
	{
	}
}
