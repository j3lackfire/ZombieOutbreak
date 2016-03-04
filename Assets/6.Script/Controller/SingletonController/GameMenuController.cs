using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenuController : Singleton<GameMenuController> {
	public Button MenuButton;

	public GameObject MenuPanel;
	public Button ResumeGameButon;
	public Button BackToMainMenuButton;

	public Button HelpButton;

	public GameObject GameOverPanel;
	public Button RetryGameButton;
	public Button ExitToMainMenuButotn;
	public Text PlayerScoreText;
	public Text HighScoreText;
	public bool isGamePaused;

	void Awake() {
		ResumeGame();
		isGamePaused = false;
		HideMenu();
		HideGameOver();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			PauseGame();
			if (!MainPlayerController.Instance.isDead) {
				ShowMenu();
			}
		}
		if (Input.GetKeyDown(KeyCode.H)) {
			TutorialController.Instance.ShowTutorial();
		}
		
	}

//	public void SetUpHighScore () {
//		string levelName;
//		int highScore;
//		switch(Application.loadedLevelName) {
//		case "2.1.JungleHouse":
//			levelName = MainMenuController.JUNGLE_SCENE_KEY;
//			break;
//		case "2.2.Mansion":
//			levelName = MainMenuController.VILLA_SCENE_KEY;
//			break;
//		case "2.3.Farm" :
//			levelName = MainMenuController.FARM_SCENE_KEY;
//			break;
//		default:
//			Debug.Log("<color=red> Error right here !!!!</color>");
//			break;
//		}
//		highScore = PlayerPrefs.GetInt(levelName,-1);
//		if (PlayerStats.Instance.numberOfZombieKilled > highScore) {
//			PlayerPrefs.SetInt(levelName,PlayerStats.Instance.numberOfZombieKilled);
//			PlayerPrefs.Save();
//		}
//	}

	public void PauseGame() {
		MenuButton.gameObject.SetActive(false);
		isGamePaused = true;
		Time.timeScale = 0.0000001f;
	}

	public void ResumeGame() {
		MenuButton.gameObject.SetActive(true);
		isGamePaused = false;
		Time.timeScale = 1;
	}

	public void OnPlayerDie() {
		PauseGame();
		HideMenu();
		ShowGameOver();
		PlayerScoreText.text = PlayerStats.Instance.numberOfZombieKilled.ToString() + " !!";

		string levelName = "";
		int highScore;
		switch(Application.loadedLevelName) {
		case "2.1.JungleHouse":
			levelName = MainMenuController.JUNGLE_SCENE_KEY;
			break;
		case "2.2.Mansion":
			levelName = MainMenuController.VILLA_SCENE_KEY;
			break;
		case "2.3.Farm" :
			levelName = MainMenuController.FARM_SCENE_KEY;
			break;
		default:
			Debug.Log("<color=red> Error right here !!!!</color>");
			break;
		}
		highScore = PlayerPrefs.GetInt(levelName,0);
		HighScoreText.text = "HighScore : " + highScore.ToString() + "!";
		if (PlayerStats.Instance.numberOfZombieKilled > highScore) {
			PlayerPrefs.SetInt(levelName,PlayerStats.Instance.numberOfZombieKilled);
			PlayerPrefs.Save();
		}
	}

	void OnEnable() {
		MenuButton.onClick.AddListener(OnMenuButtonClicked);

		ResumeGameButon.onClick.AddListener(OnResumeGameButtonClicked);
		BackToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClicked);

		RetryGameButton.onClick.AddListener(OnRetryGameButtonClicked);
		ExitToMainMenuButotn.onClick.AddListener(OnExitToMainMenuButtonClicked);

		HelpButton.onClick.AddListener(OnHelpButtonClicked);
	}

	void OnDisable() {
		MenuButton.onClick.RemoveListener(OnMenuButtonClicked);
		ResumeGameButon.onClick.RemoveListener(OnResumeGameButtonClicked);
		BackToMainMenuButton.onClick.RemoveListener(OnBackToMainMenuButtonClicked);
		RetryGameButton.onClick.RemoveListener(OnRetryGameButtonClicked);
		ExitToMainMenuButotn.onClick.RemoveListener(OnExitToMainMenuButtonClicked);
		HelpButton.onClick.RemoveListener(OnHelpButtonClicked);
	}

	void OnMenuButtonClicked() {
		PauseGame();
		ShowMenu();
	}

	void OnResumeGameButtonClicked() {
		ResumeGame();
		HideMenu();
	}

	void OnBackToMainMenuButtonClicked() {
		ResumeGame();
		Application.LoadLevel("1.MainMenuScene");
	}

	void OnRetryGameButtonClicked() {
		ResumeGame();
		Application.LoadLevel(Application.loadedLevelName);
	}

	void OnExitToMainMenuButtonClicked() {
		ResumeGame();
		Application.LoadLevel("1.MainMenuScene");
	}

	void OnHelpButtonClicked () {
		TutorialController.Instance.ShowTutorial();
	}

	void ShowMenu() {
		MenuPanel.gameObject.SetActive(true);
	}

	void HideMenu() {
		MenuPanel.gameObject.SetActive(false);
	}

	void ShowGameOver() {
		GameOverPanel.gameObject.SetActive(true);
	}
	void HideGameOver() {
		GameOverPanel.gameObject.SetActive(false);
	}
}
