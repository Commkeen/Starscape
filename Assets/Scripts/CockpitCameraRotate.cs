using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitCameraRotate : MonoBehaviour {

	Camera camera;
	Transform cameraTransform;
	Quaternion cameraDefaultRotation;

	float minFov = 20f;
	float maxFov;
	float targetFov;

	bool lockCamera = false;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera> ();
		cameraTransform = GetComponent<Transform> ();
		cameraDefaultRotation = cameraTransform.localRotation;
		maxFov = camera.fieldOfView;
		targetFov = maxFov;
	}
	
	// Update is called once per frame
	void Update () {
		var targetRotation = cameraTransform.localRotation;

		var newTargetFov = targetFov - 50*Input.GetAxis("Mouse ScrollWheel");
		if (newTargetFov <= maxFov && newTargetFov >= minFov) {
			targetFov = newTargetFov;
		}

		if (Input.GetButtonDown ("Lock Camera")) {
			lockCamera = !lockCamera;
		}
		

		if (!lockCamera) {
			var mousePos = Input.mousePosition;
			mousePos.x -= Screen.width / 2;
			mousePos.y -= Screen.height / 2;
			mousePos.x = Mathf.Clamp (mousePos.x, -Screen.width/2, Screen.width/2);
			mousePos.y = Mathf.Clamp (mousePos.y, -Screen.height/2, Screen.height/2) - 200;
			mousePos.Scale (new Vector3 (1.0f / (float)Screen.width, 1.0f / (float)Screen.height));

			targetRotation = Quaternion.Euler (mousePos.y * -30.0f + cameraDefaultRotation.eulerAngles.x,
				mousePos.x * 100.0f + cameraDefaultRotation.eulerAngles.y, 0);


		}
		

		cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, targetRotation, Time.deltaTime*10);
		camera.fieldOfView = Mathf.Lerp (camera.fieldOfView, targetFov, Time.deltaTime * 10);
	}
}
