using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {

	public bool isPlayer;
	public Vector3 color;
	public int health;
	public const int maxHealth = 100;
	public float rotationAngle;
	public float targetAngle;
	public float rotateSpeed = 7f;
	public float threshold = 1f;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		targetAngle = 90f;
		rotationAngle = 90f;

		//retrieve data about the player
		rotationAngle = transform.rotation.eulerAngles.z;
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (rotationAngle - targetAngle) > threshold) {
			if(rotationAngle > targetAngle) {
				transform.Rotate (new Vector3 (0, 0, -1f));
				rotationAngle -= 1f;
			} else {
				transform.Rotate (new Vector3 (0, 0, 1f));
				rotationAngle += 1f;
			}
		}
	}
}
