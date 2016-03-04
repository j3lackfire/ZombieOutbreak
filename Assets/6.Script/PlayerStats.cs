using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : Singleton<PlayerStats> {

#region PUBLIC FIELD
	[Tooltip("The maximum health the player can have")]
	public int MaxHealth = 100;
#endregion

	//The current health of the player
	[SerializeField]private int _playerHealth;

	public int PlayerHealth{
		get{
			return _playerHealth;
		}
		set{
			_playerHealth = value;
			int healthPercentage = (_playerHealth * 100) / MaxHealth;
			if (healthPercentage < 0) {
				healthPercentage = 0;
			}
			UIController.Instance.HealthText.text = "HEALTH : " + healthPercentage + "%";
			UIController.Instance.HealthSlider.value = _playerHealth;
			if (_playerHealth <= 0){
				Debug.Log ("<color=cyan>PLAYER IS DEATH !!!</color>");
			}
		}
	}

	public GunType currentEquipGun; // 0 = primary gun, 1 = secondary gun

	[SerializeField]int _numberOfZombieKilled = 0;
	public int numberOfZombieKilled {
		get {
			return _numberOfZombieKilled;
		}
		set {
			_numberOfZombieKilled = value;
			UIController.Instance.ZombieKillText.text = _numberOfZombieKilled.ToString();
			if (_numberOfZombieKilled <= 3) {
				if (_numberOfZombieKilled < 1) {
					ZombieManager.Instance.MinNumberOfZombie = 0; //less than 3
					ZombieManager.Instance.MaximumNumberOfZombie = 0;
					PickUpManager.Instance.gunSpawnRate = 100;
				}
				else {
					ZombieManager.Instance.MinNumberOfZombie = 1; //less than 3
					ZombieManager.Instance.MaximumNumberOfZombie = 4;
					PickUpManager.Instance.gunSpawnRate = 100;
				}
			}
			else {
				if (_numberOfZombieKilled <= 7) { //more than 3, less than 7
					PickUpManager.Instance.gunSpawnRate = 50;
					ZombieManager.Instance.MinNumberOfZombie = 2;
					ZombieManager.Instance.MaximumNumberOfZombie = 6;
				}
				else {
					if (_numberOfZombieKilled <= 12) {
						PickUpManager.Instance.gunSpawnRate = 40;
						ZombieManager.Instance.MinNumberOfZombie = 4;
						ZombieManager.Instance.MaximumNumberOfZombie = 8;
					}
					else {
						if (_numberOfZombieKilled <= 18) {
							PickUpManager.Instance.gunSpawnRate = 33;
							ZombieManager.Instance.MinNumberOfZombie = 5;
							ZombieManager.Instance.MaximumNumberOfZombie = 11;
						}
					}
				}
			}
		}
	}

	void Awake(){
		//this is hardcode so this is bad.
		if (MaxHealth == 0 || MaxHealth == null) {
			MaxHealth = 100;
		}
		numberOfZombieKilled = 0;
		_playerHealth = MaxHealth;

		StartCoroutine(HealthRegen());
	}

	IEnumerator HealthRegen() {
		while (true) {
			if (PlayerHealth < MaxHealth && PlayerHealth > 0) {
				PlayerHealth ++;
			}
			yield return new WaitForSeconds(0.75f);
		}
	}

}
