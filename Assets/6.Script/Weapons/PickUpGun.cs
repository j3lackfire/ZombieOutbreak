using UnityEngine;
using System.Collections;

public class PickUpGun : MonoBehaviour {
//	public int index;
	public float despawnTime = 15f;

	public GunType thisGunType;
	public int AmmoCount = 30;

	void Update() {
		despawnTime -= Time.deltaTime;
		if (despawnTime <= 0) {
			Destroy(this.gameObject);
		}
	}


	void OnTriggerEnter(Collider c) {
		if (c.gameObject == MainPlayerController.Instance.gameObject) {
			OnPickedByPLayer();
		}
	}

	void OnDestroy() {
//		PickUpManager.Instance.hasSpawn[index] = false;
	}

	void OnPickedByPLayer() {
		GunManager.Instance.gunAmmoList[(int)thisGunType] += AmmoCount;
		if (thisGunType == MainPlayerController.Instance.currentGunType) {
			UIController.Instance.UISetGunAmmoText(GunManager.Instance.gunAmmoList[(int)thisGunType].ToString());
		}
		if (MainPlayerController.Instance.currentGunType == GunType.Glock) {
			MainPlayerController.Instance.EquipGun(GetCurrentGun());
		}
		//Play a sound, maybe ???
		Destroy(this.gameObject);
	}

	GunController GetCurrentGun() {
		switch(thisGunType) {
		case GunType.Glock:
			return GunManager.Instance.Glock;
		case GunType.DesertEagle:
			return GunManager.Instance.DesertEagle;
		case GunType.M4A1:
			return GunManager.Instance.M4A1;
		case GunType.AK47:
			return GunManager.Instance.AK47;
		case GunType.MachineGun:
			return GunManager.Instance.MachineGun;
		case GunType.SMG:
			return GunManager.Instance.SMG;
		case GunType.Shotgun:
			return GunManager.Instance.Shotgun;
		case GunType.Sniper:
			return GunManager.Instance.Sniper;
		default:
			Debug.Log("<color=red>Problem with setting up the gun</color>");
			return null;
		}
	}
}
