using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenuController : Singleton<GameMenuController> {
	public Button MenuButton;

	public GameObject MenuPanel;
	public Button ResumeGameButon;
	public Button BackToMainMenuButton;


	public GameObject GameOverPanel;
	public Button RetryGameButton;
	public Button ExitToMainMenuButotn;
	public Text PlayerScoreText;

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
	}

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
		ShowGameOver();
		PlayerScoreText.text = PlayerStats.Instance.numberOfZombieKilled.ToString() + " !!";
	}

	void OnEnable() {
		MenuButton.onClick.AddListener(OnMenuButtonClicked);

		ResumeGameButon.onClick.AddListener(OnResumeGameButtonClicked);
		BackToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClicked);

		RetryGameButton.onClick.AddListener(OnRetryGameButtonClicked);
		ExitToMainMenuButotn.onClick.AddListener(OnExitToMainMenuButtonClicked);
	}

	void OnDisable() {
		MenuButton.onClick.RemoveListener(OnMenuButtonClicked);
		ResumeGameButon.onClick.RemoveListener(OnResumeGameButtonClicked);
		BackToMainMenuButton.onClick.RemoveListener(OnBackToMainMenuButtonClicked);
		RetryGameButton.onClick.RemoveListener(OnRetryGameButtonClicked);
		ExitToMainMenuButotn.onClick.RemoveListener(OnExitToMainMenuButtonClicked);
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
		Application.LoadLevel("2.1.FarmScene");
	}

	void OnExitToMainMenuButtonClicked() {
		ResumeGame();
		Application.LoadLevel("1.MainMenuScene");
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
