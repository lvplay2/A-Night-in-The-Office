using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour
{
	public static ArrayList allCreatures = new ArrayList();

	protected static Transform navHelp = null;

	protected Transform walkPoints;

	protected Transform boxes;

	public Animator anim;

	public Transform target;

	public NavMeshAgent agent;

	public Collider useCollider;

	protected StateMachine stateMachine = new StateMachine();

	protected float rayCastHeight = 100f;

	protected Vector3 walkingPos;

	protected float walkSpeed = 3.5f;

	protected float idlePatrollBegin;

	protected float idlePatrollMax = 3f;

	protected float runSpeed = 16f;

	protected float runAwayRange = 50f;

	protected Vector3 runAwayPoint;

	protected float runAwayPointRadius = 10f;

	protected Vector3 huntPoint;

	protected float huntPointRadius = 10f;

	protected float watchTimeMin = 20f;

	protected float watchTimeMax = 60f;

	protected float watchTime;

	protected float watchBegin;

	protected float freezeBegin;

	protected float freezeMax = 5f;

	protected float lookRange = 10f;

	protected float minRangeForAttack = 1.5f;

	protected float maxRangeForAttack = 2f;

	protected Vector3 lastValidDestination;

	protected bool isAlive = true;

	protected Transform thisTransform;

	protected Heals heals;

	private Ray rayToTerrain = default(Ray);

	protected bool relocationEnd;

	protected Transform currDecoy;

	private void OnDestroy()
	{
		allCreatures.Remove(this);
	}

	protected virtual void Start()
	{
		allCreatures.Add(this);
		heals = GetComponent<Heals>();
		thisTransform = base.transform;
		agent = GetComponent<NavMeshAgent>();
		StateMachine.State state = new StateMachine.State("walkPatrolling");
		stateMachine.AddState(state);
		state.OnEnter = WalkPatrolling_OnEnter;
		StateMachine.State state2 = new StateMachine.State("idlePatrolling");
		stateMachine.AddState(state2);
		state2.OnEnter = IdlePatrolling_OnEnter;
		state2.OnExit = IdlePatrolling_OnExit;
		StateMachine.State state3 = new StateMachine.State("runToTarget");
		stateMachine.AddState(state3);
		state3.OnEnter = RunToTarget_OnEnter;
		state3.Update = RunToTarget_Update;
		StateMachine.State state4 = new StateMachine.State("attackTarget");
		stateMachine.AddState(state4);
		state4.OnEnter = AttackTarget_OnEnter;
		state4.Update = AttackTarget_OnUpdate;
		StateMachine.State state5 = new StateMachine.State("runAwayFromTarget");
		stateMachine.AddState(state5);
		state5.OnEnter = RunAwayFromTarget_OnEnter;
		state5.Update = RunAwayFromTarget_Update;
		StateMachine.State state6 = new StateMachine.State("huntTarget");
		stateMachine.AddState(state6);
		state6.OnEnter = HuntTarget_OnEnter;
		StateMachine.State state7 = new StateMachine.State("watchTarget");
		stateMachine.AddState(state7);
		state7.OnEnter = WatchTarget_OnEnter;
		state7.Update = WatchTarget_Update;
		StateMachine.State state8 = new StateMachine.State("frozen");
		stateMachine.AddState(state8);
		state8.OnEnter = Frozen_OnEnter;
		state8.AddLink("runAwayFromTarget", Frozen_RunAwasFromTarget);
		StateMachine.State state9 = new StateMachine.State("gotoDecoy");
		stateMachine.AddState(state9);
	}

	public bool IsAlive()
	{
		return isAlive;
	}

	private void WalkPatrolling_OnEnter()
	{
		anim.SetTrigger("Walk");
		int childCount = walkPoints.childCount;
		int index = Random.Range(0, childCount);
		walkingPos = walkPoints.GetChild(index).position;
		agent.enabled = true;
		agent.speed = walkSpeed;
		agent.SetDestination(walkingPos);
	}

	private void IdlePatrolling_OnEnter()
	{
		agent.enabled = false;
		anim.SetTrigger("Idle");
		idlePatrollBegin = Time.time;
	}

	private void IdlePatrolling_OnExit()
	{
	}

	private void RunToTarget_OnEnter()
	{
		anim.SetTrigger("Run");
		agent.enabled = true;
		agent.speed = runSpeed;
		agent.SetDestination(target.position);
	}

	private void RunToTarget_Update()
	{
		agent.SetDestination(target.position);
	}

	private void AttackTarget_OnEnter()
	{
		anim.SetTrigger("Idle");
		agent.enabled = false;
	}

	private void AttackTarget_OnUpdate()
	{
	}

	private void RunAwayFromTarget_OnEnter()
	{
		anim.SetTrigger("Run");
		agent.enabled = true;
		agent.speed = runSpeed;
		Vector3 a = thisTransform.position - target.position;
		a.Normalize();
		a = Vector3.Scale(a, new Vector3(runAwayRange + 20f, runAwayRange + 20f, runAwayRange + 20f));
		a += thisTransform.position;
		if (!DestinationVerifyByBoxes(a) || !DestinationVerifyByTerrain(a, out a))
		{
			a = GetMostFarWalkPoint();
		}
		runAwayPoint = a;
		agent.SetDestination(a);
	}

	private bool DestinationVerifyByBoxes(Vector3 point)
	{
		BoxCollider[] componentsInChildren = boxes.GetComponentsInChildren<BoxCollider>();
		BoxCollider[] array = componentsInChildren;
		foreach (BoxCollider boxCollider in array)
		{
			if (boxCollider.bounds.Contains(point))
			{
				return true;
			}
		}
		return false;
	}

	private bool DestinationVerifyByTerrain(Vector3 point, out Vector3 hit)
	{
		rayToTerrain.origin = new Vector3(point.x, point.y + rayCastHeight, point.z);
		rayToTerrain.direction = Vector3.down;
		int mask = LayerMask.GetMask("Terrain");
		RaycastHit hitInfo;
		bool result = Physics.Raycast(rayToTerrain, out hitInfo, rayCastHeight * 2f, mask);
		hit = hitInfo.point;
		return result;
	}

	private Vector3 GetMostFarWalkPoint()
	{
		int childCount = walkPoints.childCount;
		Vector3 vector = walkPoints.GetChild(0).position;
		float num = (vector - thisTransform.position).sqrMagnitude;
		for (int i = 0; i < childCount; i++)
		{
			Vector3 position = walkPoints.GetChild(i).position;
			float sqrMagnitude = (thisTransform.position - position).sqrMagnitude;
			if (sqrMagnitude > num)
			{
				num = sqrMagnitude;
				vector = position;
			}
		}
		return vector;
	}

	private void RunAwayFromTarget_Update()
	{
	}

	private void HuntTarget_OnEnter()
	{
		anim.SetTrigger("Walk");
		agent.enabled = true;
		agent.speed = walkSpeed;
		huntPoint = target.position;
		agent.SetDestination(huntPoint);
	}

	private void HidenRelocation_OnEnter()
	{
		relocationEnd = false;
	}

	private void HidenRelocation_Check()
	{
	}

	private void WatchTarget_OnEnter()
	{
		anim.SetTrigger("Idle");
		agent.enabled = false;
		watchTime = Random.Range(watchTimeMin, watchTimeMax);
		watchBegin = Time.time;
	}

	private void WatchTarget_Update()
	{
	}

	private void Frozen_OnEnter()
	{
		anim.SetTrigger("Idle");
		agent.enabled = false;
		freezeBegin = Time.time;
	}

	private bool Frozen_RunAwasFromTarget()
	{
		if (freezeBegin + freezeMax < Time.time)
		{
			return true;
		}
		return false;
	}

	private void GotoDecoy_OnEnter()
	{
		agent.enabled = true;
		agent.SetDestination(currDecoy.position);
		agent.speed = walkSpeed;
		anim.SetTrigger("walk");
	}

	public virtual void TakeDamage(float damage, Transform damager)
	{
		if (!(heals.hp <= 0f))
		{
			Debug.Log(base.gameObject.name + " take damage " + damage);
			heals.TakeDamage(damage);
			if (heals.hp > 0f)
			{
				OnNotLethalStrike(damage);
			}
			else
			{
				OnLethalStrike();
			}
		}
	}

	public virtual void ScareByRocket()
	{
	}

	public virtual void ScareBySound()
	{
	}

	public virtual void OnNotLethalStrike(float damage)
	{
	}

	public virtual void OnLethalStrike()
	{
		isAlive = false;
		Die();
	}

	public virtual void Die()
	{
		stateMachine.SwitchStateTo(null);
		anim.SetTrigger("Death");
		agent.enabled = false;
		GetComponent<Collider>().isTrigger = true;
	}

	public virtual void Heal(float heal)
	{
		heals.Heal(heal);
	}

	public virtual void Freeze(float time)
	{
		if (!(heals.hp <= 0f))
		{
			stateMachine.SwitchStateTo(stateMachine.GetStateById("frozen"));
		}
	}
}
