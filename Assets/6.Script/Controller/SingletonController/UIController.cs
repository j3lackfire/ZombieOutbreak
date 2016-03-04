using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIController : Singleton<UIController> {

	[Tooltip("This UI display the health of the player")]
	public Slider HealthSlider;
	public Text HealthText;

	[Tooltip("This UI display the health of the target zombie")]
	public Slider ZombieHealthSlider;
	public Text ZombieNameText;

	private BaseZombieController _targetedZombie;
	public BaseZombieController targetedZombie {
		get {
			return _targetedZombie;
		}
		set {
			_targetedZombie = value;
			ZombieHealthSlider.maxValue = _targetedZombie.ZombieMaxHealth;
			ZombieNameText.text = _targetedZombie.ZombieDisplayName;
		}
	}
	
	public void SetZombieUI() {
		ZombieHealthSlider.value = targetedZombie.ZombieHealth;
	}

	public Text ZombieKillText;

	public Button nextGunButton;
	public Text GunName;
	public Text GunAmmo;


	void Start(){
		HealthSlider.maxValue = PlayerStats.Instance.MaxHealth;
		HealthSlider.value = PlayerStats.Instance.PlayerHealth;
	}

	void OnEnable() {
		nextGunButton.onClick.AddListener(OnNextGunButtonClicked);
	}

	void OnDisable() {
		nextGunButton.onClick.RemoveListener(OnNextGunButtonClicked);
	}

	void OnNextGunButtonClicked() {
		MainPlayerController.Instance.EquipNextGun();
	}

	public void UISetGunNameText(string name) {
		GunName.text = name;
	}

	public void UISetGunAmmoText(string ammo) {
		GunAmmo.text = ammo;
	}
}
