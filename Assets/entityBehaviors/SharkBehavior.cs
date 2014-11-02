using UnityEngine;
using System.Collections;

public class SharkBehavior : EnemyBehavior {
	public bool vicinity = false;
	public const float fastSpeed = 0.05f; //when engaging in attack
	public const float slowSpeed = 0.01f; //when firing slow bullets
	public const float topLimit = 3;
	public const float bottomLimit = 0;
	public float angle;
	public int shotCounter;
	public const int maxCounter = 300;
	
	// Use this for initialization
	void Start () {
		angle = Random.value * 20 + 80;
		shotCounter = (int)(maxCounter * Random.value);
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		float newY = transform.position.y;
		float newX = transform.position.x;
		if (transform.position.y > topLimit) {
			newY -= fastSpeed;
		} else if (transform.position.y > bottomLimit) {
			newX = transform.position.x + slowSpeed * Mathf.Cos (Mathf.PI / 180.0f * angle);
			newY = transform.position.y - slowSpeed * Mathf.Sin (Mathf.PI / 180.0f * angle);
		} else {
			newX = transform.position.x + fastSpeed * Mathf.Cos (Mathf.PI / 180.0f * angle);
			newY = transform.position.y - fastSpeed * Mathf.Sin (Mathf.PI / 180.0f * angle);
		}
		transform.position = new Vector3 (newX, newY, transform.position.z);

		shotCounter++;
		if (shotCounter >= maxCounter) {
			shotCounter = 0;
			fireBullet ();
		}
		base.Update ();
	}
}
