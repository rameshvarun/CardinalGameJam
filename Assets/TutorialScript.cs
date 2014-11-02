using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {
	public Vector2 range;
	private float timer = 0;
	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > range.x && timer < range.y) {
			target.gameObject.SetActive(true);
		} else {
			target.gameObject.SetActive(false);
		}
	}
}
