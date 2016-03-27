/// <summary>
/// The collider (and renderer) part of the bullet
/// </summary>
using UnityEngine;
using System.Collections;

public class BulletCollider : MonoBehaviour {
	public Bullet parentBullet;

	void OnCollisionEnter (Collision col) {
		parentBullet.OnImpact(col.gameObject);
	}
}
