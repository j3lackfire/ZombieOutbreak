/// <summary>
/// The Main player of the game.
/// This is the player. This is the most important part of the game.
/// Don't fuck this up ~.~
/// </summary>

using UnityEngine;
using System.Collections;

public class MainPlayerController : Singleton<MainPlayerController> {

#region UNITY EDITOR

//	public CameraController MainCamera;
	public bool isAiming {
		get {
			return characterAnimatorController.isAiming;
		}
	}
#endregion

	//I don't know, this does not feel as smooth as using character controller
	Rigidbody rigidbodyController;

	//the animator of the game object
	[HideInInspector]public CharacterAnimatorController characterAnimatorController;

	[Tooltip("The right hand of the player. Attach the gun as the child object of this")]
	public GameObject rightHand;

	[HideInInspector]public GunController currentGun;

	public GunType currentGunType;
#region LOCAL VARIABLES
	public bool isDead {
		get {
			return (PlayerStats.Instance.PlayerHealth <= 0);
		}
	}

	Vector3 targetPosition;
	[HideInInspector]public Vector3 shootingPosition;

	//move speed of the player
	
#endregion

	/*[SerializeField]*/float moveSpeed;	

	void Awake(){

//		characterController = this.gameObject.GetComponent<CharacterController> ();


		rigidbodyController = this.gameObject.GetComponent<Rigidbody> ();

		characterAnimatorController = this.gameObject.GetComponent<CharacterAnimatorController> ();

		targetPosition = this.transform.position;

		if (rightHand == null) {
			rightHand = GameObject.Find("Main Player/LK/LK Pelvis/LK Spine/LK R Clavicle/LK R UpperArm/LK R Forearm/LK R Hand");
		
		}
		if (rightHand.gameObject.transform.childCount == 1) {
			currentGun = rightHand.gameObject.transform.GetChild (0).GetComponent<GunController> ();
		}

		StartCoroutine(FixYPosition());
	}

	void Start() {
		EquipGun(GunType.Glock);
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.B)) {
			HurtPlayer(100);
		}

		if (isDead) {
			return;
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			for (int i = 0 ; i < GunManager.Instance.gunAmmoList.Count; i++) {
				GunManager.Instance.gunAmmoList[i] += 100;
			}
		}
		//machine gun and sniper needs some continuous shoting, so let it be later.

		if (Input.GetKeyDown (KeyCode.X)) {
			EquipNextGun();
		}

		if (characterAnimatorController.characterAnimation == PlayerAnimationType.Shoot) {
			if ((targetPosition - this.transform.position).magnitude > 0.2f){
				shootingPosition = targetPosition;
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			EnterAimMode();
		}

		if (Input.GetKeyUp (KeyCode.Space)){
			ExitAimMode();
		}
	}

	void FixedUpdate(){
		if (isDead) {
			if (characterAnimatorController.characterAnimation != PlayerAnimationType.Die) {
				characterAnimatorController.EnterDieAnimation();
			}
			return;
		}

		if (characterAnimatorController.characterAnimation == PlayerAnimationType.Hurt 
					|| characterAnimatorController.characterAnimation == PlayerAnimationType.Die) {
			rigidbodyController.velocity = Vector3.zero;
		} 
		else {
			if (characterAnimatorController.characterAnimation == PlayerAnimationType.Aim
			    || characterAnimatorController.characterAnimation == PlayerAnimationType.Shoot){
				//stop moving
				rigidbodyController.velocity = Vector3.zero;
			}
			else{
				MoveToTargetedPosition ();
			}
		}
//		playerSpeed = rigidbodyController.velocity.magnitude;
	}

	#region MOVEMENT
	IEnumerator FixYPosition() {
		bool shoudEscape = false;
//		if (Application.loadedLevelName != "2.1.JungleHouse") {
//			shoudEscape = true;
//		}
		while (true) {
			if (shoudEscape) {
				break;
			}
			if (this.transform.position.y > 0.2f) {
				this.transform.position = new Vector3(transform.position.x,0,transform.position.z);
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield return null;
	}

	//this function is called every frame. 
	void MoveToTargetedPosition(){
		if (characterAnimatorController.characterAnimation != PlayerAnimationType.Walk
			&& characterAnimatorController.characterAnimation != PlayerAnimationType.Run) {

			return;
		}
		Vector3 distance = new Vector3
			(targetPosition.x - this.transform.position.x,0,targetPosition.z - this.transform.position.z);


		if (distance.magnitude < 0.2f) {
			if (characterAnimatorController.characterAnimation == PlayerAnimationType.Walk
			    		|| characterAnimatorController.characterAnimation == PlayerAnimationType.Run){
				
//				Debug.Log("<color=green>Enter idle animation</color>");
				characterAnimatorController.EnterIdleAnimation();
			}
			return;
		} 
		else {
			//this cause weird things to happen
//			characterController.Move(distance.normalized* Time.deltaTime * moveSpeed);
			//weird things and unstable as hell
//			rigidbodyController.MovePosition(this.transform.position + distance.normalized * Time.deltaTime * moveSpeed);
			//the movement speed is not constant and not smooth
//			rigidbodyController.AddForce(distance.normalized * Time.deltaTime * moveSpeed * 2245f ,ForceMode.Acceleration);
//			rigidbodyController.AddForce(distance.normalized * moveSpeed * 45f ,ForceMode.Acceleration);
			//not recommended by unity .... and somehow not working
			rigidbodyController.velocity = distance.normalized * Time.deltaTime * moveSpeed * 45f;
			
		}

	}	

	//The player will move to the target positoin. 
	//The target position is determined by the player input, from MouseController.cs
	public void SetTargetPosition(Vector3 _targetPosition){

		if (isDead) {
			return;
		}

		//turn the player to the clicked position
		this.transform.LookAt(_targetPosition);
		float yRotation = this.gameObject.transform.localRotation.eulerAngles.y;
		this.transform.localRotation = Quaternion.Euler(new Vector3(0,yRotation,0));
		targetPosition = _targetPosition;

		if (characterAnimatorController.characterAnimation == PlayerAnimationType.Shoot) {
			return;
		}

		if (characterAnimatorController.characterAnimation == PlayerAnimationType.Aim) {
			//shot at the target position,
			//draw a ray from the current position to the target position, and shot at it

			//other stuffs
//			targetPosition = this.transform.position;
			characterAnimatorController.EnterShootAnimation();
			shootingPosition = _targetPosition;
			return;
		}

		Vector3 distance = new Vector3
			(targetPosition.x - this.transform.position.x,0,targetPosition.z - this.transform.position.z);

		if (distance.magnitude < 0.21f) {
			return;
		}
//		if (distance.magnitude < 4f) {
		if (distance.magnitude < 0.1f) { //the idea of walk / run is nice, but it's just tooo annoying
			moveSpeed = 2.5f;			
			if (characterAnimatorController.characterAnimation != PlayerAnimationType.Walk 
			    		&& characterAnimatorController.characterAnimation != PlayerAnimationType.Hurt
			    		&& characterAnimatorController.characterAnimation != PlayerAnimationType.Die) {
				characterAnimatorController.EnterWalkAnimation ();
			}
		} else {
			moveSpeed = 8;
			if (characterAnimatorController.characterAnimation != PlayerAnimationType.Hurt
					    && characterAnimatorController.characterAnimation != PlayerAnimationType.Die) {
				if (!characterAnimatorController.characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run_Basic")){
					characterAnimatorController.EnterRunAnimation ();
				}
			}
		}
//		Vector3 moveDirection = destinatedPosition - this.transform.position;
	}

	public void EnterAimMode() {
		targetPosition = this.transform.position;
		characterAnimatorController.isAiming = true;
		characterAnimatorController.EnterAimAnimation();
	}

	public void ExitAimMode() {
		characterAnimatorController.isAiming = false;
		if (!Input.GetMouseButton(0)){
			targetPosition = this.transform.position;
		}
		if (characterAnimatorController.characterAnimation == PlayerAnimationType.Shoot){
		}
		else{
//			Debug.Log("<color=orange>Enter idle animation</color>");
			characterAnimatorController.EnterIdleAnimation();
		}
	}

	#endregion

	#region Guns And stuffs

	public void EquipGun (GunType type) {
		EquipGun(GunManager.Instance.GetGunInstance(type));
	}

	public void EquipGun (GunController gun){
		GunController newGun = (GunController)Instantiate (gun);
		if (currentGun != null) {
			Destroy (currentGun.gameObject);
		}
		currentGun = newGun;
		currentGunType = gun.gunType;
		PlayerStats.Instance.currentEquipGun = currentGunType;

		newGun.gameObject.transform.parent = rightHand.transform;
		newGun.GunSetup ();
		switch (newGun.gunType) {
		case GunType.Glock:
			characterAnimatorController.characterAnimator.runtimeAnimatorController = GunManager.Instance.PistolAnimator;
			break;
		case GunType.DesertEagle:
			characterAnimatorController.characterAnimator.runtimeAnimatorController = GunManager.Instance.PistolAnimator;
			break;
		case GunType.M4A1:
			characterAnimatorController.characterAnimator.runtimeAnimatorController = GunManager.Instance.RifleAnimator;
			break;
		case GunType.AK47:
			characterAnimatorController.characterAnimator.runtimeAnimatorController = GunManager.Instance.RifleAnimator;
			break;
		case GunType.MachineGun:
			characterAnimatorController.characterAnimator.runtimeAnimatorController = GunManager.Instance.MachineGunAnimator;
			break;
		case GunType.SMG:
			characterAnimatorController.characterAnimator.runtimeAnimatorController = GunManager.Instance.SMGAnimator;
			break;
		case GunType.Shotgun:
			characterAnimatorController.characterAnimator.runtimeAnimatorController = GunManager.Instance.ShotgunAnimator;
			break;
		case GunType.Sniper:
			characterAnimatorController.characterAnimator.runtimeAnimatorController = GunManager.Instance.ShotgunAnimator;
			break;
		default:
			Debug.Log("<color=red>Problem with setting up the gun</color>");
			break;
		}
		characterAnimatorController.EnterAnimation(characterAnimatorController.characterAnimation);
	}

	//this method is called by the Unity Animation Event, in the middle of the gun shot animation
	public void GunShoot(){
		currentGun.ShootBullet (shootingPosition);
//		rigidbodyController.AddForce(new Vector3(0,0,0));
//		Debug.Log ("<color=orange>Pew!!!</color>");
	}

	public void EquipNextGun() {
		int currentGunIndex = 0;
		for(int i = 0; i < GunManager.Instance.gunCycle.Count; i ++) {
			if (currentGunType == GunManager.Instance.gunCycle[i].gunType) {
				currentGunIndex = i;
				break;
			}
		}
		if (currentGunIndex == GunManager.Instance.gunCycle.Count - 1 ) {
			currentGunIndex = 0;
			EquipGun(GunManager.Instance.gunCycle[currentGunIndex]);
			return;
		}
		else {
			while (currentGunIndex < GunManager.Instance.gunCycle.Count - 1) {
				currentGunIndex ++;
				if (GunManager.Instance.gunAmmoList[currentGunIndex] > 0) {
					EquipGun(GunManager.Instance.gunCycle[currentGunIndex]);
					currentGun.ammoCount = GunManager.Instance.gunAmmoList[currentGunIndex];
					return;
				}
			}
			if (currentGunType != GunType.Glock) {
				EquipGun(GunManager.Instance.gunCycle[0]);
			}
		}
	}

	#endregion

	//player is hurt in here
	public void HurtPlayer(int damage = 20){

		StartCoroutine(CameraController.Instance.BigCameraShake ());

		//if the player is already, there is no point of this function
		if (PlayerStats.Instance.PlayerHealth <= 0) {
			return;
		}

		PlayerStats.Instance.PlayerHealth -= damage;
		if (PlayerStats.Instance.PlayerHealth <= 0) {
			KillPlayer();
		}
		else {
			characterAnimatorController.EnterHurtAnimation();
		}
		
	}

	public void KillPlayer(){
		characterAnimatorController.EnterDieAnimation ();
		StartCoroutine(ShowGameOverMenu());
	}

	IEnumerator ShowGameOverMenu() {
		yield return new WaitForSeconds(3.5f);
		GameMenuController.Instance.OnPlayerDie();
	}
}
