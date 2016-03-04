/// <summary>
/// This class is unused
/// </summary>
//TODO : delete this class
using UnityEngine;
using System.Collections;

public class PickUpSpawner : MonoBehaviour {
	public PickUpGun GlockPrefab;
	public PickUpGun DesertEaglePrefab;
	public PickUpGun AK47Prefab;
	public PickUpGun M4A1Prefab;
	public PickUpGun SniperPrefab;
	
	//the chance to spawn each gun
	public int GlockWeight  = 0;
	public int DesertEagleWeight = 50;
	public int AK47Weight = 30;
	public int M4A1Weight = 65;
	public int SniperWeight = 25;

	int totalGunWeight;
	
	void Awake() {
		totalGunWeight = GlockWeight + DesertEagleWeight + AK47Weight + M4A1Weight + SniperWeight;
	}

		
	public void TrySpawnGun() {
		int randomNumber = UnityEngine.Random.Range(0,totalGunWeight);
		int gunWeight = DesertEagleWeight;
		if (randomNumber < gunWeight) {
			InstantiatePickUp(GunType.DesertEagle,this.transform.position);
			return;
		}
		gunWeight += AK47Weight;
		if (randomNumber < gunWeight) {
			InstantiatePickUp(GunType.AK47,this.transform.position);
			return;
		}
		gunWeight += M4A1Weight;
		if (randomNumber < gunWeight) {
			InstantiatePickUp(GunType.M4A1,this.transform.position);
			return;
		}
		gunWeight += SniperWeight;
		if (randomNumber < gunWeight) {
			InstantiatePickUp(GunType.Sniper,this.transform.position);
			return;
		}

	}

	void InstantiatePickUp(GunType type,Vector3 _position) {
		PickUpGun newGun = GlockPrefab;
		switch(type) {
		case GunType.DesertEagle:
			newGun = (PickUpGun)GameObject.Instantiate(DesertEaglePrefab);
			break;
		case GunType.AK47:
			newGun = (PickUpGun)GameObject.Instantiate(AK47Prefab);
			break;
		case GunType.M4A1:
			newGun = (PickUpGun)GameObject.Instantiate(M4A1Prefab);
			break;
		case GunType.Sniper:
			newGun = (PickUpGun)GameObject.Instantiate(SniperPrefab);
			break;
		}
		
		newGun.transform.parent = PickUpManager.Instance.PickUpContainer.transform;
		newGun.transform.localScale = Vector3.one;
		newGun.transform.position = _position + new Vector3(0,1,0);
	}

}
