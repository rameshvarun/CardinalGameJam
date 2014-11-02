using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
	public float swapTime;
	public float endTime;

	private float timer = 0.0f;

	public Sprite swapSprite;

	// Use this for initialization
	void Start () {
		Camera.main.SendMessage("Shake", CameraShake.SMALL_SHAKE);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > swapTime) {
			GetComponent<SpriteRenderer>().sprite = swapSprite;
		}
		if(timer > endTime) {
			Destroy(this.gameObject);
		}
	}
}
