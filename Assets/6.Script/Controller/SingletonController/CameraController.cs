using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum CameraFocusingTarget{
	Player, //The flayer is on foot and the camera is following the player.
	Car, //The camera is following the car.
	Else //this is wrong !!!
}


[RequireComponent(typeof(Camera))]
public class CameraController : Singleton<CameraController> {

#region CAMERA_SET_UP_VALUES

	Vector3 cameraPositionOffset;



	CameraFocusingTarget cameraFocusingTarget;

	//if camera is on the Big Shake animation (get hit), the shake with gun shot will not be used
	private bool isBigShaking = false;

	private Vector3 cameraShakeOffset;

#endregion

#region UNITY EDITOR
//	private MainPlayerController mainPlayer;

	[SerializeField]public GameObject focusingTarget;

#endregion

	void Awake(){
		this.gameObject.transform.parent = null;
		cameraPositionOffset = new Vector3 (0, 15, -8);
		this.transform.localRotation = Quaternion.Euler (new Vector3(60,0,0));
//		mainPlayer = FindObjectOfType<MainPlayerController> ();
//		SwitchViewToPlayer ();
	}


	void OnEnable(){
		//active the button, but we might not need the button anyway
//		changeCameraViewButton.onClick.AddListener (onChangeCameraviewButtonClicked);
	}

	void OnDisable(){
		//de-active the button but I plan to remove the button completely
//		changeCameraViewButton.onClick.RemoveListener (onChangeCameraviewButtonClicked);
	}

	void Update () {
		//make the camera follow the player
		this.transform.position = MainPlayerController.Instance.transform.position + cameraPositionOffset;
		this.transform.position = new Vector3 (this.transform.position.x, cameraPositionOffset.y, this.transform.position.z);
		this.transform.position += cameraShakeOffset;
	}

	//make the camera shake a little when the player fires the gun
	public IEnumerator SmallCameraShake(float magnitude = 0.05f,float duration = 0.005f){
		int randomSide = UnityEngine.Random.Range (0, 2) == 0 ? -1 : 1;


//		if (!isBigShaking) cameraShakeOffset = Vector3.zero;
//
//		yield return new WaitForSeconds (duration);
		if (!isBigShaking) cameraShakeOffset = new Vector3 (magnitude, 0, magnitude) * randomSide;

		yield return new WaitForSeconds (duration);
		if (!isBigShaking) cameraShakeOffset = new Vector3 (0.7f * magnitude, 0, 0.7f * magnitude) * randomSide;

		yield return new WaitForSeconds (duration);
		if (!isBigShaking) cameraShakeOffset = new Vector3 (0.4f * magnitude, 0, 0.4f * magnitude) * randomSide;

		yield return new WaitForSeconds (duration);
        if (!isBigShaking) cameraShakeOffset = new Vector3 (0.2f * magnitude, 0, 0.2f * magnitude) * randomSide;

		yield return new WaitForSeconds (duration);
		if (!isBigShaking) cameraShakeOffset = Vector3.zero;
		
		yield return null;	
	}

	public IEnumerator BigCameraShake(){
		isBigShaking = true;
		int randomSide = UnityEngine.Random.Range (0, 2) == 0 ? -1 : 1;

//		cameraShakeOffset = Vector3.zero;
//
//		yield return new WaitForSeconds (0.05f);
//		cameraShakeOffset = new Vector3 (0.02f, 0, 0.02f) * randomSide;
//
//		yield return new WaitForSeconds (0.05f);
//		cameraShakeOffset = new Vector3 (0.04f, 0, 0.04f) * randomSide;
//
//		yield return new WaitForSeconds (0.05f);
//		cameraShakeOffset = new Vector3 (0.07f, 0, 0.07f) * randomSide;
//
//		yield return new WaitForSeconds (0.05f);
		cameraShakeOffset = new Vector3 (0.1f, 0, 0.1f) * randomSide;
		
		yield return new WaitForSeconds (0.05f);
		cameraShakeOffset = new Vector3 (0.07f, 0, 0.07f) * randomSide;
		
		yield return new WaitForSeconds (0.05f);
		cameraShakeOffset = new Vector3 (0.04f, 0, 0.04f) * randomSide;

		yield return new WaitForSeconds (0.05f);
		cameraShakeOffset = new Vector3 (0.02f, 0, 0.02f) * randomSide;
		
		yield return new WaitForSeconds (0.05f);
		cameraShakeOffset = Vector3.zero;



		isBigShaking = false;
		yield return null;
	}

}
