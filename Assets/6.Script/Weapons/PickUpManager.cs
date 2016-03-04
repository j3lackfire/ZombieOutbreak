using UnityEngine;
using System.Collections;
//TODO : clean this class
public class PickUpManager : Singleton<PickUpManager> {

	//don't use anymore
	public float spawnTimer = 10f;
//	private float timer = 10f;

	public int gunSpawnRate = 100;

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
//	private int gunIndex = 0;

	//don't use
	public GameObject[] PickUpSpawnPoints;
	public bool[] hasSpawn;

	public Transform PickUpContainer;

	void Start() {
//		timer = spawnTimer;
		if (PickUpContainer == null) {
			PickUpContainer = this.transform.FindChild("PickUpContainer");
		}

		totalGunWeight = GlockWeight + DesertEagleWeight + AK47Weight + M4A1Weight + SniperWeight;

//		hasSpawn = new bool[PickUpSpawnPoints.Length];
//		for (int i = 0; i < hasSpawn.Length; i ++) {
//			hasSpawn[i] = false;
//		}
	}

//	void Update() {
//		timer -= Time.deltaTime;
//		if (timer <= 0) {
//			timer = spawnTimer;
//			int randomNum = UnityEngine.Random.Range(0,PickUpSpawnPoints.Length);
//			gunIndex = randomNum;
//			if (!hasSpawn[randomNum]) {
//				SpawnRandomGun(PickUpSpawnPoints[randomNum].gameObject.transform.position);
//				hasSpawn[randomNum] = true;
//			}
//		}
//	}

	public void SpawnGunOnZombieDie(Vector3 _position, int _spawnRate = -1) {
		int random = UnityEngine.Random.Range(0,100);
		int rate = _spawnRate == -1 ? gunSpawnRate : _spawnRate;
		if (rate > random) {
			SpawnRandomGun(_position);
		}
	}

	void SpawnRandomGun(Vector3 position) {
		int randomNumber = UnityEngine.Random.Range(0,totalGunWeight);
		int gunWeight = DesertEagleWeight;
		if (randomNumber < gunWeight) {
			InstantiatePickUp(GunType.DesertEagle,position);
			return;
		}
		gunWeight += AK47Weight;
		if (randomNumber < gunWeight) {
			InstantiatePickUp(GunType.AK47,position);
			return;
		}
		gunWeight += M4A1Weight;
		if (randomNumber < gunWeight) {
			InstantiatePickUp(GunType.M4A1,position);
			return;
		}
		gunWeight += SniperWeight;
		if (randomNumber < gunWeight) {
			InstantiatePickUp(GunType.Sniper,position);
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

		newGun.transform.parent = PickUpContainer;
		newGun.transform.localScale = Vector3.one;
		newGun.transform.position = _position + Vector3.one;
	}

}
