/// <summary>
/// Base class for singleton system
/// http://wiki.unity3d.com/index.php/Singleton
/// </summary>
using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	
	private static object _lock = new object();
	
	public static T Instance
	{
		get
		{
			if (applicationIsQuitting) {
				return null;
			}
			
			lock(_lock)
			{
				if (_instance == null)
				{
					_instance = (T) FindObjectOfType(typeof(T));
					
					if ( FindObjectsOfType(typeof(T)).Length > 1 )
					{
						Debug.LogError("<color=yellow>[Singleton]</color> Something went really wrong " + " - there should never be more than 1 singleton! " + _instance.transform.name);
						return _instance;
					}
					
					if (_instance == null)
					{
						GameObject singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = typeof(T).ToString();
						
//						DontDestroyOnLoad(singleton);

						GameObject singletonManager = GameObject.Find("_SINGLETON MANAGER");

						if (singletonManager == null){
							singletonManager = (GameObject)Instantiate(new GameObject());
							singletonManager.transform.name = "_SINGLETON MANAGER";
							singletonManager.transform.position = Vector3.zero;
						}

						singleton.transform.parent = singletonManager.transform;
						singleton.transform.localPosition = Vector3.zero;

					} 
					else {
//						Debug.Log("<color=yellow>[Singleton]</color> Using instance already created: " + _instance.gameObject.name);
					}
				}
				if (_instance == null) {
					Debug.Log("<color=yellow>CAN'T FIND THE DAMN INSTANCE</color> " + typeof(T).ToString());
				}
				
				return _instance;
			}
		}
	}

	void Awake(){

		if (FindObjectsOfType (typeof(T)).Length > 1) {
			if (_instance != this) {
				Destroy (this.gameObject);
			}
		} 
		else {
			if (FindObjectsOfType (typeof(T)).Length == 1) {
				_instance = this.gameObject.GetComponent<T>();
			}
			else{
				GameObject singleton = new GameObject();
				_instance = singleton.AddComponent<T>();
				singleton.name = typeof(T).ToString();
				
//				DontDestroyOnLoad(singleton);
				
				GameObject singletonManager = GameObject.Find("_SINGLETON MANAGER");
				
				if (singletonManager == null){
					singletonManager = (GameObject)Instantiate(new GameObject());
					singletonManager.transform.name = "_SINGLETON MANAGER";
					singletonManager.transform.position = Vector3.zero;
				}
				
				singleton.transform.parent = singletonManager.transform;
				singleton.transform.localPosition = Vector3.zero;
			}
		}

	}

//	void Update() {
//		insta = Instance.gameObject;
//	}

	private static bool applicationIsQuitting = false;
	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	public void OnDestroy () {
//		Debug.Log (this.gameObject.name + " <color=yellow>is destroyed</color>");
//		applicationIsQuitting = true;
		_instance = null;
	}

	void OnApplicationQuit() {
//		Debug.Log("<color=yellow>On Application quit. Destroying singleton + </color>" + typeof(T).ToString());
		_instance = null;
	}	

//	public void OnLevelWasLoaded(int level){
//		if (level == 0) {
//			Debug.Log("Main menu");
//		}
//		else {
//			Debug.Log("<color=yellow> On level was loaded !!!! </color>" );
//			_instance = null;
//		}
//	}
}
