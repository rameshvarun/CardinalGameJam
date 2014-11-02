using UnityEngine;
using System.Collections;

public class DolphinBehavior : EnemyBehavior {

	public const int maxHealth = 100;
	public float offset;
	public float originalX;
	public float speed = 0.02f;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		offset = Mathf.PI;
		originalX = transform.position.x;
		base.Start ();
	}

//	[RPC]
//	void SetStartSeed (float val) {
//		//offset = Mathf.PI * 2 * val;
//		offset = Mathf.PI;
//	}
	// Update is called once per frame
	void Update () {
		float newY = transform.position.y - speed;
		float newX = originalX + 0 * Mathf.Sin (transform.position.y + offset);
		transform.position = new Vector3 (newX, newY, transform.position.z);
		base.Update ();
	}
}
