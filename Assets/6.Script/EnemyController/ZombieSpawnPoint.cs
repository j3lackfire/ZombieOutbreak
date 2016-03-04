using UnityEngine;
using System.Collections;

public class ZombieSpawnPoint : MonoBehaviour {


	public void SpawnZombie(BaseZombieController zombie){
		BaseZombieController newZombie = (BaseZombieController)Instantiate (zombie);
		newZombie.transform.position = this.transform.position;
		newZombie.transform.parent = ZombieManager.Instance.ZombieParentObject.transform;
		newZombie.transform.name = newZombie.thisZombieType.ToString ();

		ZombieManager.Instance.numberOfZombie ++;
		ZombieManager.Instance.ZombieList.Add (newZombie);
	}
}
