using UnityEngine;
using System.Collections;

public enum PlayerAnimationType{
	Idle,

	Look_around, //idle look around
	Wave, //idle wave
	Yawn, //idle yawn

	Walk,

	Crawl, //walk _ Crawl - not use

	Run,

	Jump,

	Roll,
	Dash,

	Swim_Idle,
	Swim_Foward,

	Hurt,

	Aim,
	Shoot,

	Die
};


public class CharacterAnimatorController : MonoBehaviour {
	public PlayerAnimationType characterAnimation;

	public PlayerAnimationType previousAnimation;

	//should make this one private as well. There is no reason for it to be public, I think ????
	[HideInInspector]public Animator characterAnimator;
	[HideInInspector]public bool isAiming;


#region Animator_Events


#endregion

	void Awake(){
		characterAnimator = this.gameObject.GetComponent<Animator> ();
		characterAnimation = PlayerAnimationType.Idle;
		EnterIdleAnimation ();

	}

	public void EnterIdleAnimation(){
//		Debug.Log("<color=blue>Enter idle animation</color>");
		if (characterAnimation != PlayerAnimationType.Hurt) {
			previousAnimation = characterAnimation;		
		}
		characterAnimation = PlayerAnimationType.Idle;
		characterAnimator.SetTrigger ("EnterIdleAnimation");
		characterAnimator.SetInteger ("IdleAnimation",UnityEngine.Random.Range (1, 4));
	}

	//no parameter, -> normal walk
	//else -> funny Walk
	public void EnterWalkAnimation(bool funnyWalk = false){
		if (characterAnimation != PlayerAnimationType.Hurt) {
			previousAnimation = characterAnimation;		
		}		characterAnimation = PlayerAnimationType.Walk;
		characterAnimator.SetTrigger("EnterWalkAnimation");
		characterAnimator.SetInteger ("WalkAnimation", funnyWalk ? 3 : UnityEngine.Random.Range(1,3));
	}

	//no parameter = normal run.
	//2 = funny run - 3 = panic run
	public void EnterRunAnimation(int runAnimation = 1){
		if (characterAnimation != PlayerAnimationType.Hurt) {
			previousAnimation = characterAnimation;		
		}		characterAnimation = PlayerAnimationType.Run;
		characterAnimator.SetTrigger("EnterRunAnimation");
		characterAnimator.SetInteger ("RunAnimation",runAnimation);
	}

	public void EnterWaveAnimation(){
		if (characterAnimation != PlayerAnimationType.Hurt) {
			previousAnimation = characterAnimation;		
		}		characterAnimation = PlayerAnimationType.Wave;
		characterAnimator.SetTrigger ("EnterWaveAnimation");
//		Invoke ("EnterIdleAnimation", 0.2f);
	}

	public void EnterHurtAnimation(){
		if (characterAnimation != PlayerAnimationType.Hurt) {
			previousAnimation = characterAnimation;		
		}		

		characterAnimation = PlayerAnimationType.Hurt;
		characterAnimator.SetTrigger ("EnterHurtAnimation");
//		StartCoroutine (EndOfHurtAnimation());
	}

	public void EnterDieAnimation(){
		if (characterAnimation != PlayerAnimationType.Hurt) {
			previousAnimation = characterAnimation;		
		}		

		characterAnimation = PlayerAnimationType.Die;
		characterAnimator.SetTrigger ("EnterDieAnimation");
	}

	public void EnterAimAnimation(){
		if (characterAnimation != PlayerAnimationType.Hurt) {
			previousAnimation = characterAnimation;		
		}		
		characterAnimation = PlayerAnimationType.Aim;
		characterAnimator.SetTrigger ("EnterAimAnimation");
	}

	public void EnterShootAnimation(){
		if (characterAnimation != PlayerAnimationType.Hurt) {
			previousAnimation = characterAnimation;		
		}		
		characterAnimation = PlayerAnimationType.Shoot;
		characterAnimator.SetTrigger ("EnterShootAnimation");
	}

	//this function is called when the Hurt Animation is done.
	//It is called by the Unity animation system so you will not see its reference in the solution
	IEnumerator EndOfHurtAnimation(){
		if (PlayerStats.Instance.PlayerHealth <= 0) {
			yield break;
		}
//		yield return new WaitForSeconds (HurtAnimationTime);
//		Debug.Log ("<color=yellow>End of player hurt animation</color>");
//		PlayerAnimationType cachedAnimation = previousAnimation;
		if (previousAnimation != PlayerAnimationType.Shoot) {
			EnterAnimation (previousAnimation);
		}
		else {
			EnterIdleAnimation();
		}
		//by this time, the previous animation will be changed to hurt ..., we don't want that, so
		previousAnimation = characterAnimation;
		yield return null;
	}

	public void EndOfShootAnimation(){
		if (characterAnimation == PlayerAnimationType.Die) {
			return;
		}
		if (isAiming){
			EnterAimAnimation();
		}
		else{
			if (Input.GetMouseButton(0)){
//				Debug.Log("<color=yellow>Enter run animation</color>");
				EnterRunAnimation();
			}
			else{
				if (characterAnimation != PlayerAnimationType.Run){
					EnterIdleAnimation();
				}
			}
		}


	}

	public void EnterAnimation(PlayerAnimationType _animationType){
		switch (_animationType) {
		case PlayerAnimationType.Idle :
//			Debug.Log("<color=red>Enter idle animation</color>");
			EnterIdleAnimation();
			break;
		case PlayerAnimationType.Walk :
			EnterWalkAnimation();
			break;
		case PlayerAnimationType.Run :
			EnterRunAnimation();
			break;
		case PlayerAnimationType.Hurt :
			EnterHurtAnimation();
			break;
		case PlayerAnimationType.Die :
			EnterDieAnimation();
			break;
		case PlayerAnimationType.Aim:
			EnterAimAnimation();
			break;
		case PlayerAnimationType.Shoot:
			EnterShootAnimation();
			break;
		default:
			Debug.Log("<color=red>Something is wrong setting up the animation</color>");
			break;
		}
	}

}
