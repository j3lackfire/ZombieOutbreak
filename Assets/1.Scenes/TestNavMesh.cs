using UnityEngine;
using System.Collections;

public class TestNavMesh : MonoBehaviour {

	public Transform goal;
	UnityEngine.AI.NavMeshAgent pathFinder;

	void Awake(){
		goal = GameObject.Find ("Goal").GetComponent<Transform>();
		pathFinder = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			pathFinder.destination = goal.transform.position;
		}
	}
}
