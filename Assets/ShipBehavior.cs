using UnityEngine;
using System.Collections;

public class ShipBehavior : MonoBehaviour {

	public boolean isPlayer;
	public Vector3 color;
	public int health;
	public const int maxHealth = 100;

	// Use this for initialization
	void Start () {
		health = maxHealth;

		//retrieve data about the player
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
