using UnityEngine;
using System.Collections;

public class ZombieAnimatorController : MonoBehaviour {

	public Animator animator;
	
	[Tooltip("Read only, please don't change it in the inspector.")]	
	public ZombieState zombieState;

	void Awake(){
		if (animator == null) {
			animator = this.gameObject.GetComponent<Animator> ();
		}
	}

	public void EnterIdleAnimation(){
		zombieState = ZombieState.Idle;
		animator.SetTrigger("EnterIdleAnimation");
	}
	
	public void EnterMoveAnimation(){
		if (zombieState == ZombieState.Die) {
			return;
		}
		zombieState = ZombieState.Move;
		animator.SetTrigger("EnterMoveAnimation");
	}
	
	public void EnterAttackAnimation(){
		zombieState = ZombieState.Attack;
		animator.SetTrigger ("EnterAttackAnimation");
		//the animation bug has been fixed now,this part of call could be delete
//		yield return new WaitForSeconds (TimeUntilDealDamage);
//		DealDamageToPlayer ();
//		yield return new WaitForSeconds (AttackAnimationTime - TimeUntilDealDamage);
//		EndAttackAnimation ();
//		yield return null;
	}

	public void EnterHurtAnimation(){
		zombieState = ZombieState.Hurt;
		animator.SetTrigger ("EnterHurtAnimation");
	}

	public void EnterDieAnimation(){
		zombieState = ZombieState.Die;
		animator.SetTrigger ("EnterDieAnimation");
	}
}
