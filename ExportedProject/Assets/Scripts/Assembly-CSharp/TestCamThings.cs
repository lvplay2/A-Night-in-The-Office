using UnityEngine;

public class TestCamThings : MonoBehaviour
{
	public float fov = 45f;

	public float minRange = 1f;

	public float maxRange = 2f;

	public float aspect = 2f;

	private void OnDrawGizmosSelected()
	{
		Camera component = GetComponent<Camera>();
		Gizmos.color = Color.yellow;
		Gizmos.DrawFrustum(base.transform.position, fov, maxRange, minRange, aspect);
	}
}
