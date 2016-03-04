using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GunType{
	Glock = 0,
	DesertEagle = 1,
	M4A1 = 2,
	AK47 = 3,
	MachineGun = 4,
	SMG = 5,
	Shotgun = 6,
	Sniper = 7
}

public class GunManager : Singleton<GunManager> {

	public ParticleSystem hitParticle;

	#region PREFABS
	public GunController Glock;
	public GunController DesertEagle;

	public GunController M4A1;
	public GunController AK47;

	public GunController Shotgun;

	public GunController MachineGun;

	public GunController SMG;

	public GunController Sniper;

	
	#endregion

	#region ANIMATOR
	public AnimatorOverrideController PistolAnimator;
	public AnimatorOverrideController RifleAnimator;
	public AnimatorOverrideController ShotgunAnimator;
	public AnimatorOverrideController MachineGunAnimator;
	public AnimatorOverrideController SMGAnimator;

	#endregion

	public List<GunController> gunCycle = new List<GunController> ();
	public List<int> gunAmmoList = new List<int> {
		0, 0, 0, 0, 0, 0, 0, 0
	};

	public AudioSource gunFX;

	public AudioClip glock_FX;
	public AudioClip de_FX;
	public AudioClip AK_FX;
	public AudioClip M4_FX;
	public AudioClip SMG_FX;
	public AudioClip Machine_FX;
	public AudioClip Shotgun_FX;
	public AudioClip Sniper_FX;

	void Awake(){
		gunFX = this.gameObject.GetComponent<AudioSource> ();
		gunCycle.Add(Glock);
		gunCycle.Add(DesertEagle);
		gunCycle.Add(M4A1);
		gunCycle.Add(AK47);
		gunCycle.Add(Shotgun);
		gunCycle.Add(MachineGun);
		gunCycle.Add(SMG);
		gunCycle.Add(Sniper);
	}

	public void PlayGunShot(GunType _gunType){
		switch(_gunType){
		case GunType.Glock:
			gunFX.clip = glock_FX;
			break;
		case GunType.DesertEagle:
			gunFX.clip = de_FX;
			break;
		case GunType.AK47:
			gunFX.clip = AK_FX;
			break;
		case GunType.M4A1:
			gunFX.clip = M4_FX;
			break;
		case GunType.SMG:
			gunFX.clip = SMG_FX;
			break;
		case GunType.MachineGun:
			gunFX.clip = Machine_FX;
			break;
		case GunType.Shotgun:
			gunFX.clip = Shotgun_FX;
			break;
		case GunType.Sniper:
			gunFX.clip = Sniper_FX;
			break;
		}
		gunFX.Play ();
	}

	public void PlayHitParticle(Vector3 hitPosition){
		hitParticle.transform.position = hitPosition;
		hitParticle.Play ();
	}

	public GunController GetGunInstance(GunType type) {
		switch(type) {
		case GunType.Glock:
			return Glock;;
		case GunType.DesertEagle:
			return DesertEagle;;
		case GunType.M4A1:
			return M4A1;;
		case GunType.AK47:
			return AK47;;
		case GunType.MachineGun:
			return MachineGun;;
		case GunType.SMG:
			return SMG;;
		case GunType.Shotgun:
			return Shotgun;;
		case GunType.Sniper:
			return Sniper;;
		default:
			Debug.Log("<color=red>Problem with setting up the gun</color>");
			return null;;
		}
	}

}
