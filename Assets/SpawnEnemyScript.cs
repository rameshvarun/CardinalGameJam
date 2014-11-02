using UnityEngine;
using System.Collections;

public class SpawnEnemyScript : MonoBehaviour {

	public int enemyCounter = 0;
	public int respawnTime = 300; //every 5 seconds
	public Queue levelQueue;
	public const float topOfScreen = 6f;
	public const float widthOfScreen = 6f;

	public Transform dolphinEnemy;
	public Transform sharkEnemy;
	public Transform narwhalEnemy;
	public Transform whaleEnemy;

	// Use this for initialization
	void Start () {
	}

	//Input the relative ratios of each of the colors
	Color generateRandColor(int primary, int secondary, int tertiary, int white) {
		double randSelect = Random.value * (primary + secondary + tertiary + white);
		if (randSelect < primary) {
			return generateRandPrimary ();
		} else if (randSelect < primary + secondary) {
			return generateRandSecondary ();
		} else if (randSelect < primary + secondary + tertiary) {
			return generateRandTertiary ();
		} else {
			return new Color(1,1,1);
		}
	}

	Color generateRandPrimary() {
		double randSelect = Random.value * 3;
		if(randSelect < 1) {
			return new Color(1,0,0);
		} else if(randSelect < 2) {
			return new Color(0,1,0);
		} else {
			return new Color(0,0,1);
		}
	}
	
	Color generateRandSecondary() {
		double randSelect = Random.value * 3;
		if(randSelect < 1) {
			return new Color(1,1,0);
		} else if(randSelect < 2) {
			return new Color(1,0,1);
		} else {
			return new Color(0,1,1);
		}
	}

	Color generateRandTertiary() {
		double randSelect = Random.value * 6;
		if(randSelect < 1) {
			return new Color(1.0f,0.5f,0);
		} else if(randSelect < 2) {
			return new Color(0.5f,1.0f,0);
		} else if(randSelect < 3) {
			return new Color(1.0f,0,0.5f);
		} else if(randSelect < 4) {
			return new Color(0.5f,0,1.0f);
		} else if(randSelect < 5) {
			return new Color(0,1.0f,0.5f);
		} else {
			return new Color(0,0.5f,1.0f);
		}
	}

	Transform generateRandEnemy(int dolphin, int whale, int narwhal, int shark) {
		double randSelect = Random.value * (dolphin + whale + narwhal + shark);
		Transform target;
		if (randSelect < dolphin) {
			target = dolphinEnemy;
		} else if (randSelect < dolphin + whale) {
			target = whaleEnemy;
		} else if (randSelect < dolphin + whale + narwhal) {
			target = narwhalEnemy;
		} else {
			target = sharkEnemy;
		}

		return Network.Instantiate(target,
		                   new Vector3(-(widthOfScreen - 2) / 2 + Random.value * (widthOfScreen - 2),topOfScreen + Random.value,0),
		                   Quaternion.identity, 0) as Transform;
	}
	
	// Update is called once per frame
	void Update () {
		enemyCounter++;
		if (enemyCounter > respawnTime) {
			enemyCounter = 0;

			int numEnemies = 4;
			for(int i = 0; i < numEnemies; i++) {
				Transform clone = generateRandEnemy(0,1,0,1);
				EnemyBehavior actualClone = clone.GetComponent<EnemyBehavior>();

				Color col = generateRandColor(2,1,0,0);
				actualClone.networkView.RPC("SetColor", RPCMode.All, new Vector3(col.r, col.g, col.b));
				//actualClone.GetComponents<SpriteRenderer>()[0].color = ;

				/*EnemyBehavior dolphinClone = clone.GetComponent<>();
				dolphinClone.GetComponents<SpriteRenderer>()[0].color = generateRandColor(0,0,1,0);*/
				/*Transform clone = Instantiate(dolphinEnemy,
				                              new Vector3(-(widthOfScreen - 2) / 2 + Random.value * (widthOfScreen - 2),topOfScreen + Random.value,0),
				                              Quaternion.identity) as Transform;
				DolphinBehavior dolphinClone = clone.GetComponent<DolphinBehavior>();
				dolphinClone.GetComponents<SpriteRenderer>()[0].color = generateRandColor(0,0,1,0);*/
			}
		}
	}

}
