using UnityEngine;
using System.Collections;

public class MouseController : Singleton<MouseController> {
//	public DataController MainDataController;

#region UNITY EDITOR
	private Camera mainCamera;
#endregion
	Ray ray;
	RaycastHit hitInfo;
	bool useMouseInput = true;
//	Vector3 RaycastHitPosition;

	void Awake(){
		mainCamera = CameraController.Instance.gameObject.GetComponent<Camera>();
		if (Application.platform == RuntimePlatform.OSXEditor) {
			useMouseInput = true;
		}
		else {
			useMouseInput = false;
		}
	}

	void Update() {
		#region SINGLE MOUSE INPUT
		if (useMouseInput || Input.touchCount == 1) {
			if (Input.GetMouseButton (0)) {
				ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				//layer 8 : road and terrain ,
				int layerMask = (1 << 8 /*| 1 << 9 | 1 << 10*/);
				
				//if the raycase hit something
				if (Physics.Raycast (ray,out hitInfo,100f,layerMask)){
					//return if player is touching on a game UI.
					try{
//						Debug.Log("Why it's not running ??");
						if (Application.platform == RuntimePlatform.OSXEditor) {
							if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
								//if player is click on the UI button
								return;
							}
						}
						else {
							if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
								//if player is click on the UI button
								return;
							}
						}

					}
					catch{
						//why there is no event system ????
						Debug.Log("<color=red>No event system</color>");
					}
					
					//The command type, to be pass to the Data controller
					CommandType PlayerCommandType = CommandType.MoveToPosition;
					
					//check what the raycast hit
					switch(hitInfo.transform.tag){
					case "Untagged": //should usually be a terrain
						if (hitInfo.transform.name == "Terrain"){
							PlayerCommandType = CommandType.MoveToPosition;
						}
						else{
							Debug.Log("<color=green>The raycast hit an Untagged GameObject name : </color>" + hitInfo.transform.name);
						}
						break;
					case "TerrainAndRoad": //move to the position
						PlayerCommandType = CommandType.MoveToPosition;
						break;
					case "Cars": //move to car and drive it, maybe
						PlayerCommandType = CommandType.InteractWithObject;
						DataController.Instance.interactableGameObject = hitInfo.transform.gameObject;
						break;
					case "Farms":
						PlayerCommandType = CommandType.InteractWithObject;
						DataController.Instance.interactableGameObject = hitInfo.transform.gameObject;
						break;
					default:
						Debug.Log("<color=red>Something wrong happen at the Mouse controller, please check here</color>");
						PlayerCommandType = CommandType.Error;
						break;
					}
					DataController.Instance.SetMouseClickPosition(hitInfo.point,PlayerCommandType);
				}
			}

		}
		#endregion

		#region MULTI_INPUT
		if (Input.touchCount == 2) {
			for(int i = 0; i < Input.touchCount; i ++) {
				ray = mainCamera.ScreenPointToRay(Input.GetTouch(i).position);
				//layer 8 : road and terrain ,
				int layerMask = (1 << 8 /*| 1 << 9 | 1 << 10*/);
//				Vector2 touchPos = Input.GetTouch(i).position;
//				if (touchPos.x < 0.16f && touchPos.y < 0.28f) {
//					return;
//				}
				//if the raycase hit something
				if (Physics.Raycast (ray,out hitInfo,100f,layerMask)){
					//return if player is touching on a game UI.
					try{
//						Debug.Log("Why it's not running ??");
						if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId)){
							//if player is click on the UI button
//							return;
						}
						else {
							//The command type, to be pass to the Data controller
							CommandType PlayerCommandType = CommandType.MoveToPosition;
							
							//check what the raycast hit
							switch(hitInfo.transform.tag){
							case "Untagged": //should usually be a terrain
								if (hitInfo.transform.name == "Terrain"){
									PlayerCommandType = CommandType.MoveToPosition;
								}
								else{
									Debug.Log("<color=green>The raycast hit an Untagged GameObject name : </color>" + hitInfo.transform.name);
								}
								break;
							case "TerrainAndRoad": //move to the position
								PlayerCommandType = CommandType.MoveToPosition;
								break;
							case "Cars": //move to car and drive it, maybe
								PlayerCommandType = CommandType.InteractWithObject;
								DataController.Instance.interactableGameObject = hitInfo.transform.gameObject;
								break;
							case "Farms":
								PlayerCommandType = CommandType.InteractWithObject;
								DataController.Instance.interactableGameObject = hitInfo.transform.gameObject;
								break;
							default:
								Debug.Log("<color=red>Something wrong happen at the Mouse controller, please check here</color>");
								PlayerCommandType = CommandType.Error;
								break;
							}
							DataController.Instance.SetMouseClickPosition(hitInfo.point,PlayerCommandType);
						}
					}
					catch{
						//why there is no event system ????
						Debug.Log("<color=red>No event system</color>");
					}
					
				}
			}

		}
		else {
			return;
		}


		#endregion
	}

}
