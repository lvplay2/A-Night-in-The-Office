using System.Collections;
using UnityEngine;

public class Droper : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(4f);
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().AddForce(Vector3.down * 3f);
		yield return new WaitForSeconds(5f);
		Object.Destroy(base.gameObject);
	}
}
