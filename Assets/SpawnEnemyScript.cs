using UnityEngine;
using System.Collections;

public class SpawnEnemyScript : MonoBehaviour {

	public int enemyCounter = 0;
	public int respawnTime = 300; //every 5 seconds
	public Queue levelQueue;
	public const float topOfScreen = 6f;
	public const float widthOfScreen = 6f;

	public Transform dolphinEnemy;

	// Use this for initialization
	void Start () {
	}

	Color generateRandPrimary() {
		double randSelect = Random.value * 3;
		Color newColor;
		if(randSelect < 1) {
			newColor = new Color(1,0,0);
		} else if(randSelect < 2) {
			newColor = new Color(0,1,0);
		} else {
			newColor = new Color(0,0,1);
		}
		return newColor;
	}
	
	Color generateRandSecondary() {
		double randSelect = Random.value * 3;
		Color newColor;
		if(randSelect < 1) {
			newColor = new Color(1,1,0);
		} else if(randSelect < 2) {
			newColor = new Color(1,0,1);
		} else {
			newColor = new Color(0,1,1);
		}
		return newColor;
	}
	
	// Update is called once per frame
	void Update () {
		enemyCounter++;
		if (enemyCounter > respawnTime) {
			Debug.Log ("BAM!");
			enemyCounter = 0;

			for(int i = 0; i < 10; i++) {
				Transform clone = Instantiate(dolphinEnemy,
				                              new Vector3(-(widthOfScreen - 2) / 2 + Random.value * (widthOfScreen - 2),topOfScreen + Random.value,0),
				                              Quaternion.identity) as Transform;
				DolphinBehavior dolphinClone = clone.GetComponent<DolphinBehavior>();
				dolphinClone.GetComponents<SpriteRenderer>()[0].color = generateRandPrimary();
			}
		}
	}

}
