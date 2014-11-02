using UnityEngine;
using System.Collections;

public class WhaleBehavior : EnemyBehavior {

	public const int maxHealth = 500;
	public float angle;
	public const float speed = 0.01f;
	
	// Use this for initialization
	void Start () {
		health = maxHealth;
		angle = Random.value * 20 + 80;
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		float newX = transform.position.x + speed * Mathf.Cos (Mathf.PI / 180.0f * angle);
		float newY = transform.position.y - speed * Mathf.Sin (Mathf.PI / 180.0f * angle);
		transform.position = new Vector3 (newX, newY, transform.position.z);
		base.Update ();
		
	}
}
