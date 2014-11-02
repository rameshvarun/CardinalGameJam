using UnityEngine;
using System.Collections;

//Generic enemy behaviors.
public class EnemyBehavior : MonoBehaviour {
	public Color color;
	public int health;

	// Use this for initialization
	public void Start () {
		color = GetComponents<SpriteRenderer> () [0].color;
	}
	
	// Update is called once per frame
	public void Update () {
	}

	public void fireShot(float power, Color color, Vector3 direction) {
		//spawn bullet of a certain color at certain direction
	}
}
