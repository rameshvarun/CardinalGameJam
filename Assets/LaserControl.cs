﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserControl : MonoBehaviour {

//	public Vector3 testShipLocation = new Vector3(0,0,0); 
//	public Laser testLaser;
	public Transform laserPrefab;
	public Queue<Laser> activeLasers;
	public Laser playerLaser; // of this client
	public const int LASER_SPRITE_LENGTH = 100;
	
	// Uses this for initialization
	void Start () {
		GameObject redLaser = GameObject.FindGameObjectWithTag ("RedLaser");
		playerLaser = new Laser (redLaser.transform.position, Input.mousePosition, new Color(255,0,0), redLaser);
		playerLaser.gameObject.transform.localScale = new Vector3 (10, 1, 0);
		playerLaser.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		updatePlayerLaser ();
		// checkLaserCollisions(); TODO: Implement this
	}

	void updatePlayerLaser() {
		if (Input.GetMouseButtonDown (0)) {
			playerLaser.active = true;
			playerLaser.gameObject.SetActive(true);
		}
		// If mouse is down, rotate with its parent
		else if (Input.GetMouseButton (0)) {
			if (playerLaser.active) {
				Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 dir = mousePos - playerLaser.gameObject.transform.position;
//				Debug.Log (dir.x);
				//float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
				float angle = playerLaser.gameObject.transform.parent.gameObject.transform.rotation.eulerAngles.z;
				playerLaser.angle = angle;
				playerLaser.gameObject.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			}
		}
		else if (Input.GetMouseButtonUp (0)) {
			playerLaser.active = false;
			playerLaser.gameObject.SetActive(false);
		}

// 		Code for instantiating a new laser, if we do that:
//			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//			Vector3 dir = mousePos - testShipLocation;
//			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
//			Transform newLaser = Instantiate (laserPrefab, testShipLocation, Quaternion.identity) as Transform;
//			newLaser.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
//			newLaser.localScale = new Vector3(8,1,0);
	}

	void checkLaserCollisions() {

	}
}

//public static class vectorExtensions {
//	public static Vector2 XY (this Vector3 v) {
//		return new Vector2(v.x, v.y);
//	}
//}

// TODO: Convert all Vector2's to Vector3's and use Euler angles
public class Laser {
	public Vector3 startPosition;
	public Vector3 endPosition;
	public Vector3 trajectory;
	public float angle; 
	public GameObject gameObject;
	public bool active;
	public Color color;

	// Goes from x1,y1 to and through x2, y2
//	public Laser (int x1, int y1, int x2, int y2) {
//		startPosition = new Vector3 (x1, y1, 0);
//		Vector3 targetPosition = new Vector3 (x2, y2, 0);
//		angle = Vector3.Angle(startPosition, targetPosition);
//		trajectory = endPosition - startPosition;
//		endPosition = startPosition + trajectory * 10; // Testing for now
//		gameObject = null;
//		active = false;
//	}

	public Laser (Vector3 start, Vector3 target, Color aColor) {
		startPosition = start;
		angle = Vector3.Angle(start, target);
		trajectory = target - startPosition;
		endPosition = startPosition + trajectory * 10; // Testing for now
		gameObject = null;
		active = false;
		color = aColor;
	}

	public Laser (Vector3 start, Vector3 target, Color aColor, GameObject obj) {
		startPosition = start;
		angle = Vector3.Angle(start, target);
		trajectory = target - startPosition;
		endPosition = startPosition + trajectory * 10; // Testing for now
		gameObject = obj;
		active = false;
		color = aColor;
		obj.GetComponent<SpriteRenderer>().color = aColor;
	}

	public Vector3 getPossibleIntersectionWith(Laser laser2) {
		float x1 = startPosition.x;
		float y1 = startPosition.y;
		float m1 = 9999;
		if (x1 == 0) {
			m1 = y1 / x1;
		}
		float x2 = laser2.startPosition.x;
		float y2 = laser2.startPosition.y;
		float m2 = 9999;
		if (x2 == 0) {
			m2 = y2 / x2;
		}
		
		float x = 9999;
		if (m1 != m2) {
			x = (m1 * x1 - m2 * x2 + y2 - y1) / (m1 - m2);
		}
		float y = y1 + m1*(x - x1);
		
		return new Vector3(x, y, 0);
	}
	public bool doesIntersectWith(Laser laser2) {
		Vector3 possibleIntersection = getPossibleIntersectionWith (laser2);
		float x = possibleIntersection.x;
		float y = possibleIntersection.y;

		float startX1 = startPosition.x;
		float startY1 = startPosition.y;
		float endX1 = endPosition.x;
		float endY1 = endPosition.y;
		float startX2 = laser2.startPosition.x;
		float startY2 = laser2.startPosition.y;
		float endX2 = laser2.endPosition.x;
		float endY2 = laser2.endPosition.y;

		return ((startX1 - x) * (endX1 - x) < 0 && (startY1 - y) * (endY1 - y) < 0 && (startX2 - y) * (endX2 - y) < 0 && (startY2 - y) * (endY2 - y) < 0);
	}
}



