using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MainMenuPanel : MonoBehaviour {

	public Text ScoreText;
	public Text LockedText;
	public Text StatusText;
	public Button PlayButton;
	public string SceneName;

	void OnEnable() {
		PlayButton.onClick.AddListener(LoadScene);
	}

	void OnDisable() {
		PlayButton.onClick.RemoveListener(LoadScene);
	}

	void LoadScene() {
		Application.LoadLevel(SceneName);
	}

	public void PanelSetUp(int score) {
		if (score == -1) {
			PlayButton.gameObject.SetActive(false);
			ScoreText.gameObject.SetActive(false);

			LockedText.gameObject.SetActive(true);
			StatusText.gameObject.SetActive(true);
		}
		else {
			PlayButton.gameObject.SetActive(true);
			ScoreText.gameObject.SetActive(true);
			ScoreText.text = "Score : " + score.ToString();
			
			LockedText.gameObject.SetActive(false);
			StatusText.gameObject.SetActive(false);
		}
	}
}
