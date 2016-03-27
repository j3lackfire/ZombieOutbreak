/// <summary>
/// The bullet parents
/// </summary>
using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public BulletCollider childCollider;

	public bool isBulletInUse;

	public bool isSniperBullet;
	public int bulletDamage;

	private Vector3 bulletDirection;
	[SerializeField]private float bulletSpeed = 20;

	private float totalDistanceTravel;
	[SerializeField]float distanceTravelMax = 1000;

	void OnEnable() {
	}

	void OnDisable() {
	}

	void Update() {
		this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + bulletDirection,bulletSpeed * Time.deltaTime);
		totalDistanceTravel += bulletSpeed * Time.deltaTime;
		if (totalDistanceTravel >= distanceTravelMax) {
			DestroyBullet();
		}
	}

	public void InitBullet() {

	}

	public void DestroyBullet() {
		
	}

	public void FireBullet(Vector3 startPos, Vector3 direction, float bulletSpeed, int damage, bool isSniper = false) {
		
	}

	public void OnImpact(GameObject other) {
	}

}
