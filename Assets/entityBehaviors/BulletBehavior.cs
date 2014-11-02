using UnityEngine;
using System.Collections;

public class BulletBehavior : EnemyBehavior {

	public Color color;
	public float angle;
	public const float speed = 0.03f;

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		/*float newX = transform.position.x + speed * Mathf.Cos (Mathf.PI / 180.0f * angle);*/
		float newX = transform.position.x;
		float newY = transform.position.y - speed;/* * Mathf.Sin (Mathf.PI / 180.0f * angle);*/
		transform.position = new Vector3 (newX, newY, transform.position.z);
		base.Update ();
	}
}
