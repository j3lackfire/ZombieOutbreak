using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialController : Singleton<TutorialController> {
	public int CurrentTutorialSteps;
	public Image[] TutorialSteps;

	public bool isTutorialShowing = false;

	void Awake() {
//		DontDestroyOnLoad(this.gameObject);
		if (MainMenuController.villaScore == 0) {
			//first time player
			ShowTutorial();
		}
		else {
			HideTutorial();
		}
	}

	void Update() {
		if (isTutorialShowing) {
			if (Input.GetMouseButtonDown(0)) {
				NextTutorialStep();
			}
		}
	}

	public void ShowTutorial() {
		isTutorialShowing = true;
		this.GetComponent<Canvas>().enabled = true;
		Time.timeScale = 0.0000001f;
		CurrentTutorialSteps = 0;
		foreach(Image _im in TutorialSteps) {
			_im.gameObject.SetActive(false);
		}
		TutorialSteps[CurrentTutorialSteps].gameObject.SetActive(true);
	}

	public void NextTutorialStep () {
		CurrentTutorialSteps ++;
		if (CurrentTutorialSteps == TutorialSteps.Length) {
			HideTutorial();
			return;
		}
		foreach(Image _im in TutorialSteps) {
			_im.gameObject.SetActive(false);
		}
		TutorialSteps[CurrentTutorialSteps].gameObject.SetActive(true);

	}

	public void HideTutorial() {
		this.GetComponent<Canvas>().enabled = false;
		if (!GameMenuController.Instance.isGamePaused) {
			Time.timeScale = 1f;
		}
		isTutorialShowing = false;
	}

}
