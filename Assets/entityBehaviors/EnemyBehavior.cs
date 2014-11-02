using UnityEngine;
using System.Collections;

//Generic enemy behaviors.
public class EnemyBehavior : MonoBehaviour {
	public Color color;

	// Use this for initialization
	void Start () {
		color = GetComponents<SpriteRenderer> () [0].color;
	}
	
	// Update is called once per frame
	void Update () {
		//shoot bullets every once in a while
	}
}
