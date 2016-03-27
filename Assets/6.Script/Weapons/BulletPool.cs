using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPool : Singleton<BulletPool> {
	public Bullet bulletPrefab;

	public List<Bullet> bulletList = new List<Bullet>();

	void Awake() {
		Init();
	}

	void Init() {
		
	}

	public Bullet GetBullet() {
		return bulletList[0];
	}

}
