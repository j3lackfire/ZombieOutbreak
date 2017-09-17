using UnityEngine;
using System.Collections;

public enum ZombieType{
	BullZombie, //run very fast but can't turn sharp. Has delay between attack
	BunnyZombie, //normal zombie run slow, attack slow, since he is flying, maybe give he a range zombie
	GorillaZombie, //ninja zombie, fast and karate
	PenguinZombie, //I don't know what to think - normal one
	Else
}

public enum ZombieState{
	Idle, //the zombie is standing in one place and does nothing
	Move, //the zombie is on the move
	Attack, //the zombie is attacking something (must be the evil player)
	Hurt, //the poor zombie is hurt
	Die, //yeah 
	GoToDie, //the zombie will be despawned after a while, this make the game more interesting
	Else
}

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent),typeof(ZombieAnimatorController),typeof(Rigidbody))]
public class BaseZombieController : MonoBehaviour {

#region Zombie_value

	[Tooltip("Type of the zombie")]
	public ZombieType thisZombieType;

	public string ZombieDisplayName;

	[Tooltip("The max health of the zombie")]
	[SerializeField]public int ZombieMaxHealth = 100;

	[Tooltip("The current health of the zombie")]
	[SerializeField]public int ZombieHealth = 100;

	[Tooltip("The amount of damage the zombie can cause to the player")]
	[SerializeField]protected int ZombieDamage = 20;

	[Tooltip("If the player is in range, the enemy will throw an attack animation")]
	[SerializeField]protected float ZombieAgressiveRange = 2f;

	[Tooltip("The range of the attack of the zombie")]
	[SerializeField]protected float ZombieAttackRange = 2.5f;

	[Tooltip("Movement speed of the zombie. Default is 1")]
	[SerializeField]float MoveSpeed = 1;

	[Tooltip("The zombie is delay for a little while, after attacking the player")]
	[SerializeField]float delayTime = 1;

	[Tooltip("The min time for the zombie to despawn")]	
	[SerializeField]float TimeUntilDespawnMin = 30;

	[Tooltip("The mac time for the zombie to despawn")]	
	[SerializeField]float TimeUntilDespawnMax = 45;

	[SerializeField]int ZombieGunSpawnRate = -1;
#endregion


#region Animator_Controller


#endregion

	//the default speed of the nav mesh.
//	/*[SerializeField]*/float NavMeshMoveSpeed = 3.5f;

//	[Tooltip("Read only, please don't change it in the inspector.")]	
//	[SerializeField]ZombieState currentZombieState;

#region LOCAL_COMPONENT

	[SerializeField]ZombieAnimatorController ZombieAnimator;


	UnityEngine.AI.NavMeshAgent navMeshAgent;

	protected Transform goal; //the target goal should be the player
	
	float TimeUntilDespawn;//the zombie will despawn after a while

	[SerializeField]bool isRunningFast = false;
#endregion



	protected virtual void Start(){
		goal = this.transform;
		navMeshAgent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		navMeshAgent.destination = this.transform.position;
		
		
		if (ZombieAnimator == null) {
			ZombieAnimator = this.gameObject.GetComponent<ZombieAnimatorController>();
		}
		
		
		if (MoveSpeed != 0) {
			ZombieAnimator.animator.SetFloat ("MoveSpeed",MoveSpeed);
			navMeshAgent.speed *= MoveSpeed;
		}
		//this is the default of nav mesh move speed. It usually is 3.5f
		
		//		StartCoroutine(CheckSpeed());
		
		
		ZombieHealth = ZombieMaxHealth;
		
		TimeUntilDespawn = UnityEngine.Random.Range (TimeUntilDespawnMin,TimeUntilDespawnMax);
		
		ZombieAnimator.EnterIdleAnimation ();
		
		StartCoroutine(DelayAfterStart());
	}

	IEnumerator DelayAfterStart() {
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		goal = MainPlayerController.Instance.transform;
		MoveToTarget ();
	}

	void Update(){
		if (TimeUntilDespawn <= 0 && ZombieAnimator.zombieState != ZombieState.Die) {
			if (ZombieAnimator.zombieState == ZombieState.Move) {
				ZombieAnimator.zombieState = ZombieState.GoToDie;
				//set goal to the nearest spawn point
				goal = ZombieManager.Instance.DeSpawnPoints[UnityEngine.Random.Range(0,ZombieManager.Instance.DeSpawnPoints.Count)].transform;
				navMeshAgent.destination = goal.transform.position;
				Debug.Log("<color=yellow>The zombie is going to die</color>");
			}
		} 
		else {
			TimeUntilDespawn -= Time.deltaTime;
		}

		switch (ZombieAnimator.zombieState) {
		case ZombieState.Idle:
			LookAtPlayer();
			break;
		case ZombieState.Move:
			navMeshAgent.destination = goal.transform.position;
			if ((this.transform.position - goal.transform.position).magnitude < ZombieAgressiveRange && ZombieAnimator.zombieState != ZombieState.Attack) {
				navMeshAgent.destination = this.transform.position;
				ZombieAnimator.EnterAttackAnimation();
			}
			break;
		case ZombieState.Attack:
			LookAtPlayer();
			break;
		case ZombieState.Hurt:
			navMeshAgent.destination = this.transform.position;
			break;
		case ZombieState.Die:
			if (navMeshAgent != null){
				navMeshAgent.destination = this.transform.position;
			}
			return;
//			break;
		case ZombieState.GoToDie:
			if( (this.transform.position - goal.transform.position).magnitude <= 1){
				DespawnZombie();
			}
			break;
		default:
			break;
		}

		if (ZombieHealth <= 0) {
			ZombieAnimator.zombieState = ZombieState.Die;
		}
	}

	IEnumerator CheckSpeed() {
		float zombieSpeed = navMeshAgent.speed;
		isRunningFast = false;
		navMeshAgent.acceleration = 1000;
		while (true) {
			if ((this.transform.position - goal.transform.position).magnitude >= 40) {
				if (!isRunningFast) {
					navMeshAgent.speed = zombieSpeed * 5;
				}
			}
			else {
				if (isRunningFast) {
					navMeshAgent.speed = zombieSpeed;
				}
			}
			yield return null;
		}
	}




	void MoveToTarget(){
		navMeshAgent.destination = goal.transform.position;
		ZombieAnimator.EnterMoveAnimation ();	
	}

	void LookAtPlayer(){
		this.transform.LookAt(goal.position);
		float yRotation = this.gameObject.transform.localRotation.eulerAngles.y;
		this.transform.localRotation = Quaternion.Euler(new Vector3(0,yRotation,0));
	}

	//This function is called by the Unity Animator when the attack deals damage.
	public virtual void DealDamageToPlayer(){
		Vector3 distantToPlayer = this.transform.position - goal.transform.position;
		if (distantToPlayer.magnitude < ZombieAttackRange) {
			MainPlayerController.Instance.HurtPlayer(ZombieDamage);
		}
	}

	public void HurtZombie(int damage = 20){
		if (ZombieAnimator.zombieState == ZombieState.Die) {
			return;
		}
		ZombieHealth -= damage;
		if (UIController.Instance.targetedZombie != this.gameObject.GetComponent<BaseZombieController>()) {
			UIController.Instance.targetedZombie = this.gameObject.GetComponent<BaseZombieController>();
		}
		UIController.Instance.SetZombieUI();

		if (ZombieHealth <= 0) {
			this.gameObject.layer = 0;
			ZombieAnimator.EnterDieAnimation ();
			navMeshAgent.Stop();
			StartCoroutine (DespawnZombieCoroutine(4));

			GetComponent<BoxCollider>().enabled = false;
			PlayerStats.Instance.numberOfZombieKilled ++;
		} 
		else {
			ZombieAnimator.EnterHurtAnimation();
		}
	}

	//This function is called by the Unity Animator when the attack animation ends.
	public virtual void EndAttackAnimation(){
		StartCoroutine (DelayAfterAttack());
	}

	//This function is called by the Unity Animator when the hurt animation ends.
	public void EndHurtAnimation(){
		StartCoroutine (DelayAfterAttack());
	}

	//This function is called by the Unity Animator when the die animation ends.
	public void EndDieAnimation(){
		navMeshAgent.destination = this.transform.position;
		StartCoroutine (DespawnZombieCoroutine());
		PickUpManager.Instance.SpawnGunOnZombieDie(this.transform.position,ZombieGunSpawnRate);
	}

	IEnumerator DelayAfterAttack(){
//		Debug.Log ("<color=yellow>End of attack animation</color>");
		if (ZombieAnimator.zombieState == ZombieState.Die) {
			goto end;
		}
		if ((this.transform.position - goal.transform.position).magnitude < 2) {
			navMeshAgent.destination = this.transform.position;
			ZombieAnimator.EnterAttackAnimation ();
		} 
		else {
			if (delayTime != 0) {
				yield return new WaitForSeconds (delayTime);
			}
			ZombieAnimator.EnterMoveAnimation();
		}
	end:
		yield return null;
	}

	public void DespawnZombie(){
		ZombieManager.Instance.ZombieList.Remove (this);
		ZombieManager.Instance.numberOfZombie --;
		Destroy (this.gameObject);
	}

	IEnumerator DespawnZombieCoroutine(float waitTime = 0){
		if (waitTime != 0) {
			yield return new WaitForSeconds(waitTime);
		} 
		yield return new WaitForEndOfFrame ();
		//destroy the navmesh component
		navMeshAgent = null;
//		Destroy (this.gameObject.GetComponent<NavMeshAgent>());
		float timerDelay = 2;

		while (true) {
			if (timerDelay <= 0){
				break;
			}
			timerDelay -= Time.deltaTime;
			this.transform.localPosition = new Vector3(this.transform.localPosition.x,this.transform.localPosition.y - Time.deltaTime,this.transform.localPosition.z);
			yield return null;
		}
		DespawnZombie ();
	}
}
