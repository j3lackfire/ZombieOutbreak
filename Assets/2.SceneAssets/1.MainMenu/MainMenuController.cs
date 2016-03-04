using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour {

	#region PLAYER_PREFS
	public static string VILLA_SCENE_KEY = "VILLA_HIGH_SCORE";
	public static string JUNGLE_SCENE_KEY = "JUNGLE_HIGH_SCORE";
	public static string FARM_SCENE_KEY = "FARM_HIGH_SCORE";
	
	#endregion

	#region UNITY_EDITOR
	[SerializeField]Button StartGameButton;

	[SerializeField]Button BackButton;

	[SerializeField]GameObject MainView;
	[SerializeField]GameObject LevelSelectionView;
	
	[SerializeField]MainMenuPanel VillaPanel;
	[SerializeField]MainMenuPanel JunglePanel;
	[SerializeField]MainMenuPanel FarmPanel;
	
	#endregion

	public static int villaScore = 0;
	public static int jungleScore = 0;
	public static int farmScore = 0;

	private AsyncOperation async;
	float timer = 1f;

	void Awake() {
		timer = 1f;
		villaScore = PlayerPrefs.GetInt(VILLA_SCENE_KEY,0);
		jungleScore = PlayerPrefs.GetInt(JUNGLE_SCENE_KEY,-1);
		farmScore = PlayerPrefs.GetInt(FARM_SCENE_KEY,-1);

		if (villaScore >= 15 && jungleScore == -1) {
			jungleScore = 0;
		}
		if (jungleScore >= 15 && farmScore == -1) {
			farmScore = 0;
		}

		VillaPanel.PanelSetUp(villaScore);
		JunglePanel.PanelSetUp(jungleScore);
		FarmPanel.PanelSetUp(farmScore);
		
	}

	void Update() {
		if (async != null) {
			Debug.Log(async.progress);
			if (async.progress > 0.85f) {
				async.allowSceneActivation = true;
			}
		}
	}

	void OnEnable(){
		StartGameButton.onClick.AddListener (OnStartGameButtonClicked);
		BackButton.onClick.AddListener(OnBackButtonClicked);
	}

	void OnDisable(){
		StartGameButton.onClick.RemoveListener (OnStartGameButtonClicked);
		BackButton.onClick.RemoveListener(OnBackButtonClicked);
	}

	void OnStartGameButtonClicked(){
		MainView.gameObject.SetActive(false);
		LevelSelectionView.gameObject.SetActive(true);
	}

	void OnBackButtonClicked() {
		MainView.gameObject.SetActive(true);
		LevelSelectionView.gameObject.SetActive(false);
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

