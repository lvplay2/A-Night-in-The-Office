using System;
using UnityEngine;

public class Monster : Creature
{
	[Serializable]
	public class SpawnInfo
	{
		public BoxCollider box;

		public Transform[] spawnPoints;
	}

	public float damagePerAnim = 15f;

	public PlaySound monsterFarSounds;

	public LightNoise[] noises;

	public Renderer[] renderers;

	public CreatureEyes eyes;

	public Transform hidePoint;

	public SpawnInfo[] spawnInfos;

	protected MonsterPaw[] paws;

	protected DifficultLevel currDiff;

	protected float shownedStart;

	protected float shownedTime = 10f;

	protected float vanishStart;

	protected float vanishTime = 17f;

	protected float noisesTime = 5f;

	protected SoundFader chasing;

	protected StateMachine laughtMachine;

	protected string runAnimTrigger = "Run";

	private float findTaretStart;

	private float findTaretTime = 6.33f;

	private float defEyesAnge;

	private float lastTargetSeen;

	private float targetLoseTime = 5f;

	private bool wasTargetVisible;

	private float chasingStart;

	private float chasingTime = 10f;

	private bool useChasingTime;

	private bool _toggleNoises;

	protected bool toggleNoises
	{
		get
		{
			return _toggleNoises;
		}
		set
		{
			if (_toggleNoises != value)
			{
				ActiveteNoises(value);
				_toggleNoises = value;
			}
		}
	}

	private void SetMonsterParameters()
	{
		currDiff = Difficult.Instance.GetSelectedLevel();
		damagePerAnim *= currDiff.monsterMakeDamage;
		runSpeed = 2f;
		walkSpeed = 2f;
		lookRange = 45f;
		SetKrastyParams();
	}

	public void SetBasementParams()
	{
		runSpeed = 3.5f;
		vanishTime = 7f;
		noisesTime = 2f;
		useChasingTime = true;
		findTaretTime = 5f;
		runAnimTrigger = "RunFast";
	}

	public void SetKrastyParams()
	{
		runSpeed = 2.4f;
		vanishTime = 15f;
		noisesTime = 5f;
		useChasingTime = false;
		findTaretTime = 6.33f;
		runAnimTrigger = "Run";
	}

	protected override void Start()
	{
		paws = GetComponentsInChildren<MonsterPaw>();
		SetMonsterParameters();
		chasing = Unical.Get("chasing").GetComponent<SoundFader>();
		base.Start();
		stateMachine.isLoggingEnabled = true;
		stateMachine.logName = base.gameObject.name;
		StateMachine.State state = new StateMachine.State("findTarget");
		stateMachine.AddState(state);
		state.OnEnter = delegate
		{
			agent.enabled = false;
			anim.SetTrigger("FindTarget");
			findTaretStart = Time.time;
		};
		state.AddLink("runToTarget", () => eyes.IsCreatureVisible(target));
		state.AddLink("vanish", () => Time.time > findTaretStart + findTaretTime);
		StateMachine.State stateById = stateMachine.GetStateById("runToTarget");
		stateById.OnEnter = delegate
		{
			Vector3 normalized = (target.position - thisTransform.position).normalized;
			normalized.y = 0f;
			base.transform.rotation = Quaternion.LookRotation(normalized);
			anim.SetTrigger(runAnimTrigger);
			agent.enabled = true;
			agent.speed = runSpeed;
			agent.SetDestination(target.position);
			lastTargetSeen = Time.time;
			wasTargetVisible = false;
			if ((bool)chasing)
			{
				chasing.playFore = true;
			}
			chasingStart = Time.time;
		};
		stateById.Update = delegate
		{
			bool flag = eyes.IsCreatureVisible(target);
			if (flag)
			{
				lastTargetSeen = Time.time;
				agent.SetDestination(target.position);
			}
			if (flag && !wasTargetVisible)
			{
				monsterFarSounds.PlayRand("Laught");
			}
			else if (!flag && !wasTargetVisible)
			{
			}
			wasTargetVisible = flag;
		};
		stateById.OnExit = delegate
		{
			if ((bool)chasing)
			{
				chasing.playFore = false;
			}
		};
		stateById.AddLink("attackTarget", RunToTarget_AttackTarget);
		stateById.AddLink("vanish", () => Time.time > chasingStart + chasingTime && useChasingTime);
		stateById.AddLink("findTarget", () => Time.time > lastTargetSeen + targetLoseTime);
		StateMachine.State stateById2 = stateMachine.GetStateById("attackTarget");
		stateById2.AddLink("runToTarget", AttackTarget_RunToTarget);
		StateMachine.State state2 = new StateMachine.State("loseTarget");
		stateMachine.AddState(state2);
		StateMachine.State state3 = new StateMachine.State("vanish");
		stateMachine.AddState(state3);
		state3.OnEnter = Vanish_OnEnter;
		state3.Update = Vanish_Update;
		state3.OnExit = Vanish_OnExit;
		state3.AddLink("findTarget", TimeToShow);
		stateMachine.SwitchStateTo(null);
	}

	public void EnableBob()
	{
		stateMachine.SwitchStateTo(stateMachine.GetStateById("vanish"));
	}

	public void DisableBob()
	{
		base.gameObject.SetActive(false);
	}

	public void StrikeSucces()
	{
	}

	private void SetAnimDelegates()
	{
		SimpleDelegation simpleDelegation = anim.gameObject.AddComponent<SimpleDelegation>();
		simpleDelegation.Add("OnAnimAttackStart", OnAnimAttackStart);
		simpleDelegation.Add("OnAnimAttackEnd", OnAnimAttackEnd);
	}

	private void OnAnimAttackStart()
	{
		Debug.Log("OnAnimAttackStart");
		MonsterPaw[] array = paws;
		foreach (MonsterPaw monsterPaw in array)
		{
			monsterPaw.gameObject.SetActive(true);
		}
	}

	private void OnAnimAttackEnd()
	{
		Debug.Log("OnAnimAttackEnd");
		MonsterPaw[] array = paws;
		foreach (MonsterPaw monsterPaw in array)
		{
			monsterPaw.gameObject.SetActive(false);
		}
	}

	public void OnPlayerRessurected()
	{
		stateMachine.SwitchStateTo(stateMachine.GetStateById("vanish"));
	}

	private Transform GetRandomSpawnPoint()
	{
		Transform transform = null;
		if (spawnInfos != null && spawnInfos.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, spawnInfos.Length);
			Transform[] spawnPoints = spawnInfos[num].spawnPoints;
			if (spawnPoints != null && spawnPoints.Length > 0)
			{
				int num2 = UnityEngine.Random.Range(0, spawnPoints.Length);
				transform = spawnPoints[num2];
			}
		}
		if (!transform)
		{
			transform = thisTransform;
		}
		return transform;
	}

	private void Vanish_OnEnter()
	{
		vanishStart = Time.time;
		ToggleRenderers(false);
		agent.enabled = false;
		OnAnimAttackEnd();
		useCollider.isTrigger = true;
		monsterFarSounds.PlayRand("Vanish");
		if (hidePoint != null)
		{
			thisTransform.position = hidePoint.position;
			thisTransform.rotation = hidePoint.rotation;
		}
		Player.ApplyKeys();
	}

	private void ActiveteNoises(bool value)
	{
		if (noises != null && noises.Length > 0)
		{
			LightNoise[] array = noises;
			foreach (LightNoise lightNoise in array)
			{
				lightNoise.enableNoise = value;
			}
		}
	}

	private void Vanish_Update()
	{
		if (Time.time > vanishStart + vanishTime - noisesTime)
		{
			toggleNoises = true;
		}
	}

	private Transform GetFarSpPoint(Transform[] arr, float minDist = 0f)
	{
		Transform result = arr[0];
		float num = 0f;
		foreach (Transform transform in arr)
		{
			float magnitude = (transform.position - Unical.Get("Player").transform.position).magnitude;
			if (magnitude > num)
			{
				num = magnitude;
				result = transform;
			}
		}
		return result;
	}

	private Transform SelectSpawnPoint()
	{
		Transform transform = null;
		if (spawnInfos != null && spawnInfos.Length > 0)
		{
			SpawnInfo[] array = spawnInfos;
			foreach (SpawnInfo spawnInfo in array)
			{
				if (spawnInfo.box != null && spawnInfo.box.bounds.Contains(target.position))
				{
					Transform[] spawnPoints = spawnInfo.spawnPoints;
					if (spawnPoints != null && spawnPoints.Length > 0)
					{
						transform = GetFarSpPoint(spawnPoints);
					}
					return transform;
				}
			}
		}
		if (transform == null)
		{
			transform = GetRandomSpawnPoint();
		}
		return transform;
	}

	private void ChangePosition()
	{
		Transform transform = SelectSpawnPoint();
		if (transform != null)
		{
			thisTransform.position = transform.position;
			thisTransform.rotation = transform.rotation;
		}
	}

	private void Vanish_OnExit()
	{
		ChangePosition();
		toggleNoises = false;
		shownedStart = Time.time;
		ToggleRenderers(true);
		OnAnimAttackStart();
		useCollider.isTrigger = false;
		monsterFarSounds.PlayRand("Show");
	}

	private bool TimeToVanish()
	{
		if (Time.time > shownedStart + shownedTime)
		{
			return true;
		}
		return false;
	}

	private bool TimeToShow()
	{
		if (Time.time > vanishStart + vanishTime)
		{
			return true;
		}
		return false;
	}

	private void ToggleRenderers(bool value)
	{
		Renderer[] array = renderers;
		foreach (Renderer renderer in array)
		{
			if ((bool)renderer)
			{
				renderer.enabled = value;
			}
		}
	}

	private bool TargetInRadius(float radius)
	{
		if ((target.position - thisTransform.position).sqrMagnitude < radius * radius)
		{
			return true;
		}
		return false;
	}

	private void AttackTarget_OnExit()
	{
	}

	private bool WalkPatrolling_IdlePatrolling()
	{
		if ((thisTransform.position - walkingPos).sqrMagnitude < 9f)
		{
			return true;
		}
		return false;
	}

	private bool IdlePatrolling_WalkPatrolling()
	{
		if (Time.time >= idlePatrollBegin + idlePatrollMax)
		{
			return true;
		}
		return false;
	}

	private bool RunToTarget_AttackTarget()
	{
		if ((thisTransform.position - target.position).sqrMagnitude < minRangeForAttack * minRangeForAttack)
		{
			return true;
		}
		return false;
	}

	private bool AttackTarget_RunToTarget()
	{
		if ((thisTransform.position - target.position).sqrMagnitude > maxRangeForAttack * maxRangeForAttack)
		{
			return true;
		}
		return false;
	}

	private bool Patrolling_RunToTarget()
	{
		if ((thisTransform.position - target.position).sqrMagnitude < lookRange * lookRange)
		{
			return true;
		}
		return false;
	}

	private bool MonsterRetreate()
	{
		return false;
	}

	private bool IsMonsterScared()
	{
		return false;
	}

	public override void Die()
	{
		Player component = target.GetComponent<Player>();
		component.OnMonsterDie();
		base.Die();
	}

	private void FixedUpdate()
	{
		stateMachine.Update();
	}
}
