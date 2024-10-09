using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TransScript : MonoBehaviour
{
	private Text myText;

	private void Awake()
	{
		myText = GetComponent<Text>();
	}

	private void OnEnable()
	{
		myText.text = myText.text.Translate();
	}
}
