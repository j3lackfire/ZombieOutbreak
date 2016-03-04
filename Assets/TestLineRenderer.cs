using UnityEngine;
using System.Collections;

public class TestLineRenderer : MonoBehaviour {

	public ParticleSystem particle;

	void Awake(){
		particle = this.gameObject.GetComponent<ParticleSystem> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			particle.Play();
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			particle.Stop();
		}
	}
}
