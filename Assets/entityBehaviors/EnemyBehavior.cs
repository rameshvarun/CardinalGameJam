using UnityEngine;
using System.Collections;

//Generic enemy behaviors.
public class EnemyBehavior : MonoBehaviour {
	public Color color;
	public int health;
	public const float bottomOfScreen = -6f; //approximately...

	// Use this for initialization
	public void Start () {
		color = GetComponents<SpriteRenderer> () [0].color;
	}
	
	// Update is called once per frame
	public void Update () {
		if (transform.position.y < bottomOfScreen || health <= 0) {
			Destroy(this.gameObject);
		}
	}

	public void fireBullet(float power, Color color, Vector3 direction) {
		//spawn bullet of a certain color at certain direction
	}

	[RPC]
	public void SetColor(Vector3 color) {
		GetComponents<SpriteRenderer>()[0].color = new Color(color.x, color.y, color.z, 1.0f);
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (isLaser(coll.gameObject.tag) {
			Color color2 = coll.gameObject.GetComponent<SpriteRenderer> ().color;
			Vector3 colorVector = new Vector3 (color.r, color.g, color.b);
			Vector3 colorVector2 = new Vector3 (color2.r, color2.g, color2.b);
			float colorDistance = Vector3.Distance (colorVector, colorVector2);
			health -= (int)(Mathf.Ceil (10 / Mathf.Max (1, colorDistance)));
		}
    }

	bool isLaser(string tag) {
		return (tag == "RedLaser" || tag == "GreenLaser" || tag == "BlueLaser" || tag == "YellowLaser" || tag == "MagentaLaser" || tag == "CyanLaser" || tag == "WhiteLaser");
	}
}

