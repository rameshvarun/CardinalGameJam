using UnityEngine;
using System.Collections;

public class ForceRatio : MonoBehaviour {

	int lastX = 0;
	int lastY = 0;

	// Use this for initialization
	void Start() {
		Resize();

		Camera bg = new GameObject("BackgroundCam", typeof(Camera)).camera;
		bg.depth = int.MinValue;
		bg.clearFlags = CameraClearFlags.SolidColor;
		bg.backgroundColor = Color.black;
		bg.cullingMask = 0;
	}
	void Resize () {
		float realAspect = (float)Screen.width / (float)Screen.height;
		float desiredAspect = 9.0f / 16.0f;
		float scaleHeight = realAspect / desiredAspect;

		Camera camera = Camera.main;
		if(scaleHeight < 1.0f) {
			Rect rect = camera.rect;
			rect.width = 1.0f;
			rect.height = scaleHeight;
			rect.x = 0;
			rect.y = (1.0f - scaleHeight) / 2.0f;

			camera.rect = rect;
		}
		else {
			float scaleWidth = 1.0f/scaleHeight;
			Rect rect = camera.rect;
			rect.width = scaleWidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scaleWidth) / 2.0f;
			rect.y = 0;

			camera.rect = rect;
		}
		lastX = Screen.width;
		lastY = Screen.height;

		Debug.Log("Enforcing aspect ratio.");
	}
	
	// Update is called once per frame
	void Update () {
		if(lastX != Screen.width || lastY != Screen.height) {
			Resize();
		}
	
	}
}
