using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BunnyZombieController : BaseZombieController {
	public BunnyBullet BunnyBulletPrefab;
	public BunnyBullet[] bunnyBulletPool;
	public int poolSize = 3;
	protected override void Start () {
		base.Start ();
		bunnyBulletPool = new BunnyBullet[poolSize];
		for (int i = 0; i < poolSize; i ++) {
			bunnyBulletPool[i] = Instantiate(BunnyBulletPrefab) as BunnyBullet;
			bunnyBulletPool[i].gameObject.SetActive(false);
			bunnyBulletPool[i].isAvailable = true;
			bunnyBulletPool[i].transform.parent = this.transform;
		}
	}

	public override void DealDamageToPlayer () {
		BunnyBullet bullet = bunnyBulletPool[0];
		for (int i = 0; i < poolSize; i ++) {
			if (bunnyBulletPool[i].isAvailable) {
				bullet = bunnyBulletPool[i];
				break;
			}
		}
		bullet.isAvailable = false;
		bullet.transform.position = this.transform.position + new Vector3(0, 1, 0);
		bullet.transform.gameObject.SetActive(true);
		bullet.transform.localScale = Vector3.one;
		bullet.SetTarget(goal.position + new Vector3(0, 1, 0));
	}

}
