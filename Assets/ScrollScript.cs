using UnityEngine;
using System.Collections;

public class ScrollScript : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, - speed * Time.deltaTime, 0);
		if(transform.position.y < -15) {
			Destroy(this.gameObject);
		}
	}
}
