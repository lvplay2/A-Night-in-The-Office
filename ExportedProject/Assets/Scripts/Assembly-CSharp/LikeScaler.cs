using System.Collections;
using UnityEngine;

public class LikeScaler : MonoBehaviour
{
	private RectTransform mytrans;

	public Vector2 firstState;

	public Vector2 secondState;

	public Vector2 finalState;

	public float speed;

	public bool loop;

	public float loopDelay;

	private void Start()
	{
		mytrans = GetComponent<RectTransform>();
		StartCoroutine(First());
	}

	private IEnumerator First()
	{
		while (mytrans.sizeDelta != firstState)
		{
			mytrans.sizeDelta = Vector3.MoveTowards(mytrans.sizeDelta, firstState, Time.unscaledDeltaTime * speed * 7f);
			yield return null;
		}
		StartCoroutine(Second());
	}

	private IEnumerator Second()
	{
		while (mytrans.sizeDelta != secondState)
		{
			mytrans.sizeDelta = Vector3.MoveTowards(mytrans.sizeDelta, secondState, Time.unscaledDeltaTime * speed * 4f);
			yield return null;
		}
		StartCoroutine(Final());
	}

	private IEnumerator Final()
	{
		while (mytrans.sizeDelta != finalState)
		{
			mytrans.sizeDelta = Vector3.MoveTowards(mytrans.sizeDelta, finalState, Time.unscaledDeltaTime * speed * 2f);
			yield return null;
		}
		if (loop)
		{
			yield return new WaitForSeconds(loopDelay);
			StartCoroutine(First());
		}
	}
}
