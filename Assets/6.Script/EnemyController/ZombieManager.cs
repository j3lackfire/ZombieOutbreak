using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class ZombieManager : Singleton<ZombieManager> {

#region Zombie_Prefab
	public BaseZombieController BullZombiePrefab;
	public BaseZombieController BunnyZombiePrefab;
	public BaseZombieController GorillaZombiePrefab;
	public BaseZombieController PenguinZombiePrefab;

	//weight for spawn ratio
	public int bullZombieWeight = 20;
	public int bunnyZombieWeight = 40;
	public int gorillaZombieWeight = 30;
	public int penguinZombieWeight = 55;

	int totalWeight;
#endregion

#region UNITY_EDITOR

	[HideInInspector]public GameObject ZombieParentObject;

	[Tooltip("The spawn timer of the zombie spawner")]	
	[SerializeField]float SpawnTimer = 10;

	[Tooltip("The list of all of the spawn points for the zombie")]	
	public List<ZombieSpawnPoint> SpawnPoints = new List<ZombieSpawnPoint>();

	[Tooltip("The list of all of the de-spawn points for the zombie")]	
	public List<GameObject> DeSpawnPoints = new List<GameObject>();

	[Tooltip("List of all the zombie in the game")]
	public List<BaseZombieController> ZombieList = new List<BaseZombieController>();

#endregion

	[Tooltip("Current number of zombie on the field")]
	[SerializeField]int _numberOfZombie;

	public int numberOfZombie{
		get {
			return _numberOfZombie;
		}
		set{
			_numberOfZombie = value;
		}
	}

	[Tooltip("Minimum number of zombie")]
	public int MinNumberOfZombie = 5;

	[Tooltip("Maximum number of zombie")]
	public int MaximumNumberOfZombie= 10;

	//delete this later
	float timer;

	void Awake(){
		totalWeight = bullZombieWeight + bunnyZombieWeight + gorillaZombieWeight + penguinZombieWeight;
		ZombieList.Clear ();
		//set up zombie parents gameobject
		GameObject g = GameObject.Find ("Enemy");
		if (g == null) {
			g = GameObject.Instantiate (new GameObject ());
			g.transform.parent = null;
			g.name = "Enemy";
			ZombieParentObject = g;
		}
		else {
			ZombieParentObject = g;
		}

		BaseZombieController[] ZombieArray = GameObject.FindObjectsOfType<BaseZombieController> ();
		foreach (BaseZombieController b in ZombieArray) {
			ZombieList.Add(b);
		}
		numberOfZombie = ZombieList.Count;
		//delete this later
		timer = 0;
	}

	void Update(){
		timer += Time.deltaTime;
		if (timer >= SpawnTimer) {
			timer = 0;
			if (numberOfZombie <= MaximumNumberOfZombie) {
				spawnNewZombie();
			}
		}
		if (numberOfZombie <= MinNumberOfZombie) {
			spawnNewZombie();
		}
	}

	void spawnNewZombie(){
		int PointToSpawn = UnityEngine.Random.Range (0,SpawnPoints.Count);

		BaseZombieController CachedZombie = BullZombiePrefab;

		int randomNumber = UnityEngine.Random.Range(0,totalWeight);
		int ZombieToSpawn = 0;
		if (randomNumber < bullZombieWeight) {
			ZombieToSpawn = 0;
		}
		else {
			if (randomNumber < bunnyZombieWeight + bullZombieWeight) {
				ZombieToSpawn = 1;
			}
			else {
				if (randomNumber < bunnyZombieWeight + bullZombieWeight + gorillaZombieWeight) {
					ZombieToSpawn = 2;
				}
				else {
					if (randomNumber < bunnyZombieWeight + bullZombieWeight + gorillaZombieWeight + penguinZombieWeight) {
						ZombieToSpawn = 3;
					}
				}
			}
		}


		switch (ZombieToSpawn) {
		case 0: //spawn bull;
			CachedZombie = BullZombiePrefab;
			break;
		case 1:
			CachedZombie = BunnyZombiePrefab;
			break;
		case 2:
			CachedZombie = GorillaZombiePrefab;
			break;
		case 3 :
			CachedZombie = PenguinZombiePrefab;
			break;
		default:
			break;
		}
		SpawnPoints [PointToSpawn].SpawnZombie (CachedZombie);
	}

	//if a new scene is loaded
	void OnLevelWasLoaded(){
//		Debug.Log ("On level was loaded is called");
		ZombieList.Clear ();
		//set up zombie parents gameobject
		GameObject g = GameObject.Find ("Enemy");
		if (g == null) {
			g = GameObject.Instantiate (new GameObject ());
			g.transform.parent = null;
			g.name = "Enemy";
			ZombieParentObject = g;
		}
		else {
			ZombieParentObject = g;
		}
		
		BaseZombieController[] ZombieArray = GameObject.FindObjectsOfType<BaseZombieController> ();
		foreach (BaseZombieController b in ZombieArray) {
			ZombieList.Add(b);
		}
		numberOfZombie = ZombieList.Count;
		
		//delete this later
		timer = 0;
	}

}
