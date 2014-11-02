﻿using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {

	public bool isPlayer;
	public Color color;
	public int health;
	public const int maxHealth = 100;
	public float rotationAngle;
	public float targetAngle;
	public float rotateSpeed = 2f;
	public const float angleThreshold = 10f;
	public float travelAngle;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		targetAngle = 90f;
		rotationAngle = 90f;
		travelAngle = 0f;

		//retrieve data about the player
		rotationAngle = transform.rotation.eulerAngles.z;
		color = GetComponents<SpriteRenderer> () [0].color;
		if (color == new Color (1, 0, 0)) {
			isPlayer = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Detect mouse clicks
		if (Input.GetMouseButton (0)) { //left click
			//Only detect player
			if(isPlayer) {
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if(mousePos.x > this.transform.position.x)
					targetAngle = 180.0f / Mathf.PI * Mathf.Atan ((mousePos.y - this.transform.position.y) / (mousePos.x - this.transform.position.x));
				else
					targetAngle = 180.0f + 180.0f / Mathf.PI * Mathf.Atan ((mousePos.y - this.transform.position.y) / (mousePos.x - this.transform.position.x));
				travelAngle = targetAngle - rotationAngle;
			}
		}

		//Rotate objects
		if (Mathf.Abs (rotationAngle - targetAngle) > angleThreshold) {
			if(rotationAngle > targetAngle) {
				transform.Rotate (new Vector3 (0, 0, -rotateSpeed));
				rotationAngle -= rotateSpeed;
			} else {
				transform.Rotate (new Vector3 (0, 0, rotateSpeed));
				rotationAngle += rotateSpeed;
			}
		}
	}
}
