using UnityEngine;
using System.Collections;

public class BunnyBullet : MonoBehaviour {
	public int bulletDamage = 20;
	public float BulletSpeed = 8;
	public GameObject BulletImpactPrefab;
	GameObject bulletImpact;
	Vector3 bulletDirection;

	float bulletLifeTime = 0;
	public bool isAvailable = false;

	public float bulletExistTime = 5;
	void Awake() {
		bulletLifeTime = 0;
		bulletImpact = GameObject.Instantiate(BulletImpactPrefab,this.transform.position,Quaternion.identity) as GameObject;
		bulletImpact.gameObject.SetActive(false);
		bulletImpact.transform.parent = this.transform;
	}

	void OnEnable() {
		bulletLifeTime = 0;
	}

	void Update() {
		bulletLifeTime += Time.deltaTime;
		if (bulletLifeTime >= bulletExistTime) {
			BulletExplode();
		}
		this.transform.position = Vector3.MoveTowards(this.transform.position,this.transform.position + bulletDirection,Time.deltaTime * BulletSpeed);
	}

	public void SetTarget(Vector3 target) {
		bulletDirection = (target - this.transform.position).normalized;
	}

	public void BulletExplode () {
		bulletImpact.transform.parent = null;
		bulletImpact.transform.position = this.transform.position;
		bulletImpact.gameObject.SetActive(true);
		this.transform.position = new Vector3(0,-1000,0);
		StartCoroutine(BulletDestroyDelay());
	}

	IEnumerator BulletDestroyDelay() {
		yield return new WaitForSeconds(2f);
		bulletImpact.transform.parent = this.transform;
		bulletImpact.gameObject.SetActive(false);
		isAvailable = true;
		this.gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider c) {
		if (bulletLifeTime >= 0.15f) {
			if (c.gameObject == MainPlayerController.Instance.gameObject) {
				BulletExplode();
				MainPlayerController.Instance.HurtPlayer(bulletDamage);
			}
		}
	}
}
