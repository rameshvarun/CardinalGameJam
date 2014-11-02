using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {
//	private Vector2 startPosition;
//	private Vector2 endPosition;
//	private Vector2 trajectory;
//	private float angle; 

//	// Goes from x1,y1 to and through x2, y2
//	void setupLaser (int x1, int y1, int x2, int y2) {
//		startPosition = new Vector2 (x1, y1);
//		Vector2 targetPosition = new Vector2 (x2, y2);
//		angle = Vector2.Angle(startPosition, targetPosition);
//		trajectory = endPosition - startPosition;
//		endPosition = startPosition + trajectory * 10; // Testing for now
//	}
//	
//	void setupLaser (Vector2 start, Vector2 target) {
//		startPosition = start;
//		angle = Vector2.Angle(start, target);
//		trajectory = target - startPosition;
//		endPosition = startPosition + trajectory * 10; // Testing for now
//	}

	// Use this for initialization
	void Start () {
//		startX = transform.position.x;
//		startY = transform.position.y;
//		if (startX != 0) {
//			slope = startY / startX;
//		}
//		else {
//			slope = 9999; // TODO: choose non-arbitrary large #? :D
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 dir = mousePos - transform.position;
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

		}
		// Each frame, start position and angle based on how its variables were changed by the global function
	}

}
