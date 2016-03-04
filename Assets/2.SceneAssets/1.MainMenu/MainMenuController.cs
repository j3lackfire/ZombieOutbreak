using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour {

	#region UNITY_EDITOR
	[SerializeField]Button NewGameButton;

	[SerializeField]Button QuitGameButton;

	#endregion

	private AsyncOperation async;
	float timer = 1f;
	void Awake() {
		timer = 1f;
	}

	void Update() {
		if (async != null) {
			Debug.Log(async.progress);
			if (async.progress > 0.85f) {
				async.allowSceneActivation = true;
			}
		}

//		Debug.Log("Async is done !!!");
//		timer -= Time.deltaTime;
//		if (timer <= 0) {
//			Debug.Log("Done with one second countdown");
//			async.allowSceneActivation = true;
//		}

	}

	void OnEnable(){
		NewGameButton.onClick.AddListener (OnNewGameButtonClicked);
		QuitGameButton.onClick.AddListener (OnQuitGameButtonClicked);
	}

	void OnDisable(){
		NewGameButton.onClick.RemoveListener (OnNewGameButtonClicked);
		QuitGameButton.onClick.RemoveListener (OnQuitGameButtonClicked);
	}

	void OnNewGameButtonClicked(){
//		Application.LoadLevel ("2.1.FarmScene");
		StartCoroutine(LoadScene());
	}

	IEnumerator LoadScene() {
		if (async == null) {
			async = Application.LoadLevelAsync("2.1.FarmScene");
			async.allowSceneActivation = false;
			Debug.Log("Start Loading !!!");
			yield return async;
		}
		yield return null;
	}

	void OnQuitGameButtonClicked(){
		Application.Quit ();
	}

}

