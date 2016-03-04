using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour {

	[Tooltip("Topping point of the gun")]
	public LineRenderer ShootingLine;

	public string GunDisplayName = "Gun";

	[Tooltip("The flashing time of the gun")]
	public float FlashingTime = 0.2f;

	[Tooltip("The damage of the gun")]
	public int gunDamage = 10;

	[Tooltip("The offset position of the gun")]
	public Vector3 positionOffset;

	[Tooltip("The offset rotation of the gun")]
	public Vector3 rotationOffset;

	[Tooltip("The offset scale of the gun")]
	public Vector3 scaleOffset;

	[Tooltip("The type of the gun")]
	public GunType gunType;

	[Tooltip("Camera Shake Magnitude")]
	public float CamShakeMagnitude = 0.05f;

	[Tooltip("Camera Shake Duration")]
	public float CamShakeDuration = 0.005f;


	[SerializeField]AudioClip thisGunFX;

	/*[SerializeField]*/ParticleSystem muzzleFlash;

	public int ammoCount = 0;

	void Awake(){
		muzzleFlash = ShootingLine.gameObject.GetComponent<ParticleSystem> ();
	}

	void OnEnable(){
		ammoCount = GunManager.Instance.gunAmmoList[(int)gunType];
		if (gunType == GunType.Glock) {
			UIController.Instance.UISetGunAmmoText("INFINITE");
		}
		else {
			UIController.Instance.UISetGunAmmoText(ammoCount.ToString());
		}
		UIController.Instance.UISetGunNameText(GunDisplayName);
		GunSetup ();
	}

	public void GunSetup(){
		this.transform.localPosition = positionOffset;
		this.transform.localRotation = Quaternion.Euler (rotationOffset);
		this.transform.localScale = scaleOffset;
	}

	public void ShootBullet(Vector3 endPos){
		if (this.gunType == GunType.Glock) {
			//do nothing
		}
		else {
			ammoCount = GunManager.Instance.gunAmmoList[(int)gunType];
			if (ammoCount <= 0) {
				return;
			}
			ammoCount --;
			if (ammoCount < 0) {
				ammoCount = 0;
			}
			GunManager.Instance.gunAmmoList[(int)gunType] = ammoCount;
			UIController.Instance.UISetGunAmmoText(ammoCount.ToString());
		}

		GunManager.Instance.PlayGunShot (this.gunType);
		StartCoroutine(CameraController.Instance.SmallCameraShake (CamShakeMagnitude,CamShakeDuration));
		ShootingLine.SetWidth (0.1f,0.1f);
		if (this.gunType != GunType.Shotgun && this.gunType != GunType.Sniper) {
			ShootingLine.SetPosition(0,ShootingLine.transform.position);
			Vector3 endOfLine = new Vector3(endPos.x,ShootingLine.transform.position.y,endPos.z);
			Vector3 direction = endOfLine - ShootingLine.transform.position;
			Ray shootingRay = new Ray(ShootingLine.transform.position,direction);
			RaycastHit hitInfo;
			// 9 = enemy - 10 = obstacles
			int layerMask = (1 << 9 | 1 << 10);
			muzzleFlash.Play();
			if (Physics.Raycast (shootingRay,out hitInfo,direction.magnitude * 20,layerMask)){
				if (hitInfo.transform.gameObject.tag == "Enemy"){
					hitInfo.transform.gameObject.GetComponent<BaseZombieController>().HurtZombie(gunDamage);
				}
				ShootingLine.SetPosition(1,hitInfo.point);
				GunManager.Instance.PlayHitParticle(hitInfo.point);
			}
			else{
				ShootingLine.SetPosition(1,ShootingLine.transform.position + direction * 20);
			}
		} 
		else {
			if (this.gunType == GunType.Shotgun){
			}
			else{//sniper gun
				List<GameObject> enemyList = new List<GameObject>();
				ShootingLine.SetPosition(0,ShootingLine.transform.position);
				Vector3 endOfLine = new Vector3(endPos.x,ShootingLine.transform.position.y,endPos.z);
				Vector3 direction = endOfLine - ShootingLine.transform.position;
				Ray shootingRay = new Ray(ShootingLine.transform.position,direction);
				RaycastHit hitInfo;
				int layerMask = (1 << 9 | 1 << 10);
				muzzleFlash.Play();
				for (int i = 0; i < 5; i ++){
					if (Physics.Raycast (shootingRay,out hitInfo,direction.magnitude * 20,layerMask)){
						if (hitInfo.transform.gameObject.tag == "Enemy" && !enemyList.Contains(hitInfo.transform.gameObject)){
							enemyList.Add(hitInfo.transform.gameObject);
							hitInfo.transform.gameObject.GetComponent<BaseZombieController>().HurtZombie(gunDamage);
						}
					}
				}
				ShootingLine.SetPosition(1,ShootingLine.transform.position + direction * 20);
			}

		}
		StartCoroutine (delayShutdown());

	}

	//the shooting light only flash up for a short moment
	IEnumerator delayShutdown(){
		yield return new WaitForSeconds(FlashingTime);
		muzzleFlash.Stop ();
		ShootingLine.SetWidth (0, 0);
	}

	public void AddAmmo(int amount) {
		ammoCount += amount;
	}

}
