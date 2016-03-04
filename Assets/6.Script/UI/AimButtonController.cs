using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AimButtonController : Button {

	protected override void Awake() {
		base.Awake();
		StartCoroutine(checkAiming());
	}

	IEnumerator checkAiming() {
		while(true) {
			if (MainPlayerController.Instance.isAiming) {
#if UNITY_EDITOR
				if (!IsPressed() && !Input.GetKey(KeyCode.Space)) {
					MainPlayerController.Instance.ExitAimMode();
				}
#else
				if (!IsPressed()) {
					MainPlayerController.Instance.ExitAimMode();
				}
#endif
			}
			yield return new WaitForSeconds(0.5f);
		}
//		yield return null;
	}

	public override void OnPointerDown (UnityEngine.EventSystems.PointerEventData eventData) {
		base.OnPointerDown (eventData);
		MainPlayerController.Instance.EnterAimMode();
	}

	public override void OnPointerUp (UnityEngine.EventSystems.PointerEventData eventData) {
		base.OnPointerUp (eventData);
		MainPlayerController.Instance.ExitAimMode();
	}

}
