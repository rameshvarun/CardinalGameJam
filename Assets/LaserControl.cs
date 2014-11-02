using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserControl : MonoBehaviour {

//	public Vector3 testShipLocation = new Vector3(0,0,0); 
//	public Laser testLaser;
	public Transform laserPrefab;
//	public Queue<Laser> activeLasers;

	public Laser redLaser;
	public Laser greenLaser;
	public Laser blueLaser;
	public Laser yellowLaser;
	public Laser magentaLaser;	
	public Laser cyanLaser;
	public Laser whiteLaser;

	public Laser playerLaser; // of this client
	public static int LASER_SPRITE_LENGTH = 100;

	public static float MAX_STRENGTH_DISTANCE = 6;
	
	// Uses this for initialization
	void Start () {
		redLaser = new Laser (GameObject.FindGameObjectWithTag ("RedLaser"));
		greenLaser = new Laser (GameObject.FindGameObjectWithTag ("GreenLaser"));
		blueLaser = new Laser (GameObject.FindGameObjectWithTag ("BlueLaser"));
		yellowLaser = new Laser (GameObject.FindGameObjectWithTag ("YellowLaser"));
		magentaLaser = new Laser (GameObject.FindGameObjectWithTag ("MagentaLaser"));
		cyanLaser = new Laser (GameObject.FindGameObjectWithTag ("CyanLaser"));
		whiteLaser = new Laser (GameObject.FindGameObjectWithTag ("WhiteLaser"));

		if(MainMenu.myColor == ShipColor.Red)
			playerLaser = redLaser;
		else if(MainMenu.myColor == ShipColor.Blue)
			playerLaser = blueLaser;
		else if(MainMenu.myColor == ShipColor.Green)
			playerLaser = greenLaser;

		//greenLaser.active = true;
		//blueLaser.active = true;
		StopLaser(redLaser);
		StopLaser(blueLaser);
		StopLaser(greenLaser);

//		activeLasers.Enqueue (greenLaser);
//		activeLasers.Enqueue (blueLaser);

	}
	
	// Update is called once per frame
	void Update () {
		updatePlayerLaser ();
		checkLaserCollisions ();
	}

	void ActivateLaser(Laser laser) {
		laser.active = true;
		laser.gameObject.SetActive(true);
		Camera.main.SendMessage("Shake", CameraShake.SMALL_SHAKE);
	}

	void StopLaser(Laser laser) {
		laser.active = false;
		laser.gameObject.SetActive(false);
	}

	void updatePlayerLaser() {
		if (Input.GetMouseButtonDown (0)) {
			ActivateLaser(playerLaser);

		}
		// If mouse is down, rotate with its parent
		else if (Input.GetMouseButton (0)) {
			if (playerLaser.active) {
//				Vector3 dir = mousePos - playerLaser.gameObject.transform.position;
//				Debug.Log (dir.x);
				//float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
				Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				mousePos.z = 0;
				float angle = playerLaser.gameObject.transform.parent.gameObject.transform.rotation.eulerAngles.z;
				playerLaser.setAngle(angle);
				playerLaser.setTrajectoryAndEndPositionFromAngle ();
				playerLaser.setAlphaByDistance(Vector3.Distance(mousePos, playerLaser.startPosition));
			}
		}
		else {
			StopLaser(playerLaser);
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

		redLaser.setTrajectoryAndEndPositionFromAngle ();
		greenLaser.setTrajectoryAndEndPositionFromAngle ();
		blueLaser.setTrajectoryAndEndPositionFromAngle ();
		Debug.Log ("Before: " + cyanLaser.endPosition);

		cyanLaser.setTrajectoryAndEndPositionFromAngle ();
		Debug.Log ("After: " + cyanLaser.endPosition);

		magentaLaser.setTrajectoryAndEndPositionFromAngle ();
		yellowLaser.setTrajectoryAndEndPositionFromAngle ();
		
		Vector3 redGreenCollision = redLaser.getIntersectionWith(greenLaser);
		Vector3 redBlueCollision = redLaser.getIntersectionWith(blueLaser);
		Vector3 greenBlueCollision = greenLaser.getIntersectionWith(blueLaser);

		// Matt debugging:
//		Vector3 redGreenPC = redLaser.getPossibleIntersectionWith(greenLaser);
//		Vector3 redBluePC = redLaser.getPossibleIntersectionWith(blueLaser);
//		Vector3 greenBluePC = greenLaser.getPossibleIntersectionWith(blueLaser);
//
//		GameObject rgc = GameObject.FindGameObjectWithTag ("RedGreenCollision");
//		GameObject rbc = GameObject.FindGameObjectWithTag ("RedBlueCollision");
//		GameObject bgc = GameObject.FindGameObjectWithTag ("BlueGreenCollision");
//
//		rgc.transform.position = redGreenPC;
//		rbc.transform.position = redBluePC;
//		bgc.transform.position = greenBluePC;

//		Debug.Log (redLaser.getPossibleIntersectionWith(greenLaser).x);

		float redToGreenDist = Vector3.Distance (redLaser.startPosition, redGreenCollision);
		float redToBlueDist = Vector3.Distance (redLaser.startPosition, redBlueCollision);
		float blueToGreenDist = Vector3.Distance (blueLaser.startPosition, greenBlueCollision);

		// Red/blue collision farthest -> Yellow or Cyan
		if (redToGreenDist <= redToBlueDist) {
//			Debug.Log ("R to G Distance: " + redToGreenDist + ", R to B Distance: " + redToBlueDist + ", B to G Dist: " + blueToGreenDist);

			if (blueToGreenDist < redToGreenDist) {
				// Cyan!
//				Debug.Log ("Cyan!");
				cyanLaser.activateAt(greenBlueCollision);
				cyanLaser.setAngle((greenLaser.angle + blueLaser.angle)/2);
				cyanLaser.setColorAdditively(greenLaser.color, blueLaser.color);

				magentaLaser.deactivate();
				yellowLaser.deactivate();

				greenLaser.setEndPosition(greenBlueCollision);
				blueLaser.setEndPosition(greenBlueCollision);

				if (cyanLaser.doesIntersectWith(redLaser)) {
					whiteLaser.setAngle((2*cyanLaser.angle + redLaser.angle)/3);
					Vector3 whiteCollision = cyanLaser.getIntersectionWith(redLaser);
					whiteLaser.activateAt(whiteCollision);
					whiteLaser.setColorAdditively(cyanLaser.color, redLaser.color);
					
					cyanLaser.setEndPosition(whiteCollision);
					redLaser.setEndPosition(whiteCollision);
				}
				else {
					whiteLaser.deactivate();
				}
			}
			else {
//				Debug.Log("Yellow!");

				yellowLaser.activateAt(redGreenCollision);
				yellowLaser.setAngle((greenLaser.angle + redLaser.angle)/2);
				yellowLaser.setColorAdditively(greenLaser.color, redLaser.color);

				magentaLaser.deactivate();
				cyanLaser.deactivate();

				greenLaser.setEndPosition(redGreenCollision);
				redLaser.setEndPosition(redGreenCollision);
				
				if (yellowLaser.doesIntersectWith(blueLaser)) {
					whiteLaser.setAngle((2*yellowLaser.angle + blueLaser.angle)/3);
					Vector3 whiteCollision = yellowLaser.getIntersectionWith(blueLaser);
					whiteLaser.activateAt(whiteCollision);
					whiteLaser.setColorAdditively(yellowLaser.color, blueLaser.color);

					
					yellowLaser.setEndPosition(whiteCollision);
					blueLaser.setEndPosition(whiteCollision);
				}
				else {
					whiteLaser.deactivate();
				}
			}
		}

		// Red/green collision farthest -> Magenta or Cyan
		else {
			// Cyan!
//			Debug.Log ("Cyan!");
			if (blueToGreenDist < redToBlueDist) {
				cyanLaser.activateAt(greenBlueCollision);
				cyanLaser.setAngle((greenLaser.angle + blueLaser.angle)/2);
				cyanLaser.setColorAdditively(greenLaser.color, blueLaser.color);

				magentaLaser.deactivate();
				yellowLaser.deactivate();

				greenLaser.setEndPosition(greenBlueCollision);
				blueLaser.setEndPosition(greenBlueCollision);
				
				if (cyanLaser.doesIntersectWith(redLaser)) {
					whiteLaser.setAngle((2*cyanLaser.angle + redLaser.angle)/3);
					Vector3 whiteCollision = cyanLaser.getIntersectionWith(redLaser);
					whiteLaser.activateAt(whiteCollision);
					whiteLaser.setColorAdditively(cyanLaser.color, redLaser.color);
					
					cyanLaser.setEndPosition(whiteCollision);
					redLaser.setEndPosition(whiteCollision);

					Debug.Log (cyanLaser.endPosition);

				}
				else {
					whiteLaser.deactivate();
				}
			}
			// Magenta!
			else {
				Debug.Log ("Magenta!");
				magentaLaser.activateAt(redBlueCollision);
				magentaLaser.setAngle((blueLaser.angle + redLaser.angle)/2);
				yellowLaser.deactivate();
				cyanLaser.deactivate();

				redLaser.setEndPosition(redBlueCollision);
				blueLaser.setEndPosition(redBlueCollision);
				
				if (magentaLaser.doesIntersectWith(greenLaser)) {
					whiteLaser.setAngle((2*magentaLaser.angle + greenLaser.angle)/3);
					Vector3 whiteCollision = magentaLaser.getIntersectionWith(greenLaser);
					whiteLaser.activateAt(whiteCollision);
					whiteLaser.setColorAdditively(magentaLaser.color, greenLaser.color);

					
					magentaLaser.setEndPosition(whiteCollision);
					greenLaser.setEndPosition(whiteCollision);
				}
				else {
					whiteLaser.deactivate();
				}
			}
		} 
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
	public Color baseColor;
	public Color color;
	public float width;

	// This constructor is used the most often
	public Laser (GameObject obj) {
		gameObject = obj;
		startPosition = gameObject.transform.position;
		angle = gameObject.transform.eulerAngles.z;
		width = gameObject.transform.localScale.y;
		setTrajectoryAndEndPositionFromAngle ();
		baseColor = gameObject.GetComponent<SpriteRenderer>().color;
		color = baseColor;
		active = gameObject.activeSelf;

	}

	public void setEndPosition(Vector3 pos) {
		endPosition = pos;
		gameObject.transform.localScale = new Vector3(Vector3.Distance (startPosition, endPosition), width, 1);
	}

	public void setTrajectoryAndEndPositionFromAngle() {
		//float length = gameObject.transform.localScale.x;// * LaserControl.LASER_SPRITE_LENGTH;
		float length = 15;
		trajectory = new Vector3 (length, 0, 0);
		trajectory = Quaternion.Euler (0, 0, angle) * trajectory;
		setEndPosition(startPosition + trajectory);
	}
	public void activateAt(Vector3 startPos) {
		active = true;
		gameObject.SetActive (true);
		setStartPosition (startPos);
	}
	
	public void deactivate() {
		active = false;
		gameObject.SetActive (false);
	}

	public void setStartPosition(Vector3 pos) {
		startPosition = pos;
		gameObject.transform.position = startPosition;
	}

	public void setAngle(float theta) {
		angle = theta;
		gameObject.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	public void setColorIntensity(float distance) {
		float intensity = Mathf.Min(distance / LaserControl.MAX_STRENGTH_DISTANCE,1);
		color = new Color (baseColor.r * intensity, baseColor.g * intensity, baseColor.b * intensity);
		gameObject.GetComponent<SpriteRenderer>().color = color;
	}

	public void setAlphaByDistance(float distance) {
		float intensity = Mathf.Min(distance / LaserControl.MAX_STRENGTH_DISTANCE,1);
		color = new Color (baseColor.r, baseColor.g, baseColor.b, intensity);
		gameObject.GetComponent<SpriteRenderer>().color = color;
	}

	public void setColorAdditively(Color c1, Color c2) {
		float totalRed = Mathf.Min (c1.r + c2.r, 255);
		float totalGreen = Mathf.Min (c1.g + c2.g, 255);
		float totalBlue = Mathf.Min (c1.b + c2.b, 255);
		color = new Color (totalRed, totalGreen, totalBlue);
		gameObject.GetComponent<SpriteRenderer>().color = color;
	}
	public void setColorAveraged(Color c1, Color c2, float ratio) {
		float totalRed = ratio * c1.r + (1 - ratio) * c2.r;
		float totalGreen = ratio * c1.g + (1-ratio) * c2.g;
		float totalBlue = ratio * c1.b + (1-ratio) * c2.b;
		color = new Color (totalRed, totalGreen, totalBlue);
		gameObject.GetComponent<SpriteRenderer>().color = color;

	}

	public Vector3 getPossibleIntersectionWith(Laser laser2) {
		float x1 = startPosition.x;
		float y1 = startPosition.y;
		float m1 = Mathf.Tan (angle*Mathf.Deg2Rad);
		float x2 = laser2.startPosition.x;
		float y2 = laser2.startPosition.y;
		float m2 = Mathf.Tan (laser2.angle*Mathf.Deg2Rad);

//		Debug.Log ("x1: " +x1+", y1: "+y1+", m1: "+m1+", x2: "+x2+", y2: "+y2+", m2: "+m2);

		float x = 9999;
		if (m1 != m2) {
			x = (m1 * x1 - m2 * x2 + y2 - y1) / (m1 - m2);
		}
		float y = y1 + m1*(x - x1);
		if (y < -2) {
//			Debug.Log ("x = (m1: " + m1 + ") * (x1: " + x1 + ") - (m2: " + m2 + ") * (x2: " + x2 + ") + (y2: " + y2 + ") - (y1: " + y1 + ")) / ((m1: " + m1 + ") - (m2: " + m2 + "))"); 
		}
		return new Vector3(x, y, 0);
	}
	public bool doesIntersectWith(Laser laser2) {
		if (!active || !laser2.active) {
//				Debug.Log ("Inactive");
				return false;
		} else {
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
				
//			if ((startX1 - x) * (endX1 - x) < 0 && (startY1 - y) * (endY1 - y) < 0)
//				Debug.Log ("In range of the first!");
//			if ((startX2 - x) * (endX2 - x) < 0 && (startY2 - y) * (endY2 - y) < 0)
//			Debug.Log ("startX2: "+startX2+", x: "+x+", endX2: " + endX2 +", startY2: "+startY2+", y: "+y+", endY2: " + endY2);
			return ((startX1 - x) * (endX1 - x) < 0 && (startY1 - y) * (endY1 - y) < 0 && (startX2 - x) * (endX2 - x) < 0 && (startY2 - y) * (endY2 - y) < 0);

		}
	}

	public Vector3 getIntersectionWith(Laser laser2) {
		if (doesIntersectWith(laser2)) {
			return getPossibleIntersectionWith(laser2);
		}
		else {
			return new Vector3(-100,-100,0); // Cuz why not
		}
	}
}



