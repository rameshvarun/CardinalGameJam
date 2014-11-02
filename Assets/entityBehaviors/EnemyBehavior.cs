using UnityEngine;
using System.Collections;

//Generic enemy behaviors.
public class EnemyBehavior : MonoBehaviour {
	public float health;
	public const float bottomOfScreen = -6f; //approximately...

	public static float ACCURACY = 0.8f;

	public int score;

	public Transform explosion;

	float colorDistance = -1;

	// Use this for initialization
	public void Start () {

	}
	
	// Update is called once per frame
	public void Update () {
		if (transform.position.y < bottomOfScreen || health <= 0) {
			Network.Destroy(networkView.viewID);
		}

		if(health < 0) {
			// Create explosion
			Network.Instantiate(explosion, transform.position, transform.rotation, 0);

			// Add to score
			GameObject.Find("GameController").networkView.RPC("AddScore", RPCMode.All, score);

			// Destroy enemy
			Network.Destroy(networkView.viewID);
		}
	}

	public void FixedUpdate() {
		if(colorDistance != -1 && colorDistance < ACCURACY) {
			int state = Mathf.FloorToInt(Time.realtimeSinceStartup * 5.0f);
			if(state % 2 == 0) GetComponents<SpriteRenderer> () [0].enabled = false;
			else GetComponents<SpriteRenderer> () [0].enabled = true;

			if(Network.isServer)
				health -= Time.deltaTime;
		} else {
			GetComponents<SpriteRenderer> () [0].enabled = true;
		}
		colorDistance = -1;
	}

	public void fireBullet(float power, Color color, Vector3 direction) {
		//spawn bullet of a certain color at certain direction
	}

	[RPC]
	public void SetColor(Vector3 color) {
		GetComponents<SpriteRenderer>()[0].color = new Color(color.x, color.y, color.z, 1.0f);
	}

	void OnTriggerStay2D(Collider2D other) {
		if (isLaser(other.tag)) {
			Color color = GetComponents<SpriteRenderer> () [0].color;
			Color color2 = other.GetComponent<SpriteRenderer> ().color;
			Vector3 colorVector = new Vector3 (color.r, color.g, color.b);
			Vector3 colorVector2 = new Vector3 (color2.r, color2.g, color2.b);
			colorDistance = Vector3.Distance (colorVector, colorVector2);
			//health -= (int)(Mathf.Ceil (10 / Mathf.Max (1, colorDistance)));
		}
		if(other.tag == "PlayerShip") {
			other.networkView.RPC("Hit", RPCMode.All);

			// Create explosion
			Network.Instantiate(explosion, transform.position, transform.rotation, 0);

			// Destroy enemy
			Network.Destroy(networkView.viewID);
		}
    }

	bool isLaser(string tag) {
		return (tag == "RedLaser" || tag == "GreenLaser" || tag == "BlueLaser" || tag == "YellowLaser" || tag == "MagentaLaser" || tag == "CyanLaser" || tag == "WhiteLaser");
	}
}

