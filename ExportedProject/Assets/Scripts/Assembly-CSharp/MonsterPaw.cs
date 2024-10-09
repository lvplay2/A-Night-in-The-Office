using UnityEngine;

public class MonsterPaw : MonoBehaviour
{
	public Vector3 punchVec;

	public float power;

	public PlaySound sound;

	private Monster monster;

	private void Start()
	{
		monster = base.gameObject.GetComponentInParent<Monster>();
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider coll)
	{
		if (!(power > 0f))
		{
			return;
		}
		CreatureHitBox component = coll.gameObject.GetComponent<CreatureHitBox>();
		if (!component)
		{
			return;
		}
		Creature creature = component.GetCreature();
		if (creature != monster)
		{
			monster.StrikeSucces();
			component.TakeDamage(power, monster.transform);
			if ((bool)sound)
			{
				sound.PlayRand("punch");
			}
		}
	}

	public void SetPower(float pow)
	{
		power = pow;
	}
}
