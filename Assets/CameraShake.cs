using UnityEngine;
using System.Collections;

// Let's do this when lasers cross?

public class CameraShake : MonoBehaviour {
	public Vector3 originalPosition;
	public Quaternion originalOrientation;

	public float intensity = 0.0f;

	public static float SMALL_SHAKE = 0.1f;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		originalOrientation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if(intensity > 0.0001) {
			transform.position = originalPosition + new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
			transform.rotation = originalOrientation * Quaternion.AngleAxis(Random.Range(-intensity, intensity), Vector3.forward);

			intensity = Mathf.Lerp(intensity, 0, 4*Time.deltaTime);
		} else {
			transform.position = originalPosition;
			transform.rotation = originalOrientation;
		}
	}

	void Shake(float newIntensity) {
		intensity = newIntensity;
	}
}
