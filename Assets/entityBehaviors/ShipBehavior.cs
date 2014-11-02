﻿using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {

	public bool isPlayer;
	public Color color;
	public int health;
	public const int maxHealth = 100;
	public float rotationAngle;
	public float targetAngle;
	public float rotateSpeed;
	public const float angleThreshold = 2f;
	public float travelAngle;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		targetAngle = 90f;
		rotationAngle = 90f;
		travelAngle = 0f;

		//retrieve data about the player
//		transform.Rotate(new Vector3 (0, 0, 90f));
		rotationAngle = transform.rotation.eulerAngles.z;
	}

	// Update is called once per frame
	void Update () {
		//Detect mouse clicks
		if (Input.GetMouseButton (0)) { //left click

			//Only detect player
			if(networkView.isMine) {
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				if(mousePos.x > this.transform.position.x)
					targetAngle = 180.0f / Mathf.PI * Mathf.Atan ((mousePos.y - this.transform.position.y) / (mousePos.x - this.transform.position.x));
				else
					targetAngle = 180.0f + 180.0f / Mathf.PI * Mathf.Atan ((mousePos.y - this.transform.position.y) / (mousePos.x - this.transform.position.x));
				travelAngle = targetAngle - rotationAngle;
//				float roll = Mathf.LerpAngle(rotationAngle, targetAngle, Time.time);
//				transform.eulerAngles = new Vector3 (0, 0, roll);
//				transform.Rotate (new Vector3 (0, 0, roll));
			}

			if (Mathf.Abs (rotationAngle - targetAngle) > angleThreshold) {
				if(rotationAngle > targetAngle) {
					//float roll = Mathf.LerpAngle(rotationAngle, targetAngle, (targetAngle-rotationAngle)/rotateSpeed);
					//transform.Rotate (new Vector3 (0, 0, -rotateSpeed));
					rotationAngle -= rotateSpeed;
					//transform.eulerAngles = new Vector3(0, 0, rotationAngle);
					gameObject.transform.Find("player_ship_turret").transform.eulerAngles = new Vector3(0, 0, rotationAngle);

				} else {
//					transform.Rotate (new Vector3 (0, 0, rotateSpeed));
					rotationAngle += rotateSpeed;
					gameObject.transform.Find("player_ship_turret").transform.eulerAngles = new Vector3(0, 0, rotationAngle);
					//transform.eulerAngles = new Vector3(0, 0, rotationAngle);

				}
			}

		}

		//Rotate objects




	}
}
