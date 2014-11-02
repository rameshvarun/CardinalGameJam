using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public Color color;
	public int health;
	public const int maxHealth = 100;

	// Use this for initialization
	void Start () {
		color = GetComponents<SpriteRenderer> () [0].color;
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
