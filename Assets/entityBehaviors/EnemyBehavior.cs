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
		if (transform.position.y < bottomOfScreen) {
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
}
