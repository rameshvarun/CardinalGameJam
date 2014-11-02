using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Have new enemy position change randomly until not overlapping with any that have already been placed
public class SpawnEnemyScript : MonoBehaviour {

	public int enemyCounter = 0;
	public int respawnTime = 300; //every 5 seconds
	public Queue<Wave> levelQueue;
	public const float topOfScreen = 6f;
	public const float widthOfScreen = 6f;

	public bool structuredMode;

	public Transform dolphinEnemy;
	public Transform sharkEnemy;
	public Transform narwhalEnemy;
	public Transform whaleEnemy;

	// Use this for initialization
	void Start () {

		levelQueue = new Queue<Wave> ();
		// One red
		Wave thisWave = new Wave (300);
		thisWave.enemies.Enqueue(new EnemySave (whaleEnemy, -1, new Color (1, 0, 0), null, null));
		levelQueue.Enqueue (thisWave);

		// One of each of the others
		thisWave = new Wave (200);
		thisWave.enemies.Enqueue (new EnemySave (whaleEnemy, -1, new Color (0, 0, 1), null, null));
		thisWave.enemies.Enqueue (new EnemySave (whaleEnemy, -1, new Color (0, 1, 0), null, null));
		levelQueue.Enqueue (thisWave);

		thisWave = new Wave (100);
		thisWave.enemies.Enqueue (new EnemySave (whaleEnemy, -1, new Color (0, 0, 1), null, null));
		thisWave.enemies.Enqueue (new EnemySave (whaleEnemy, -1, new Color (0, 1, 0), null, null));
		thisWave.enemies.Enqueue (new EnemySave (whaleEnemy, -1, new Color (1, 0, 1), null, null)););
		levelQueue.Enqueue (thisWave);
		thisWave = new Wave (300);
		thisWave.enemies.Enqueue (new EnemySave (whaleEnemy, -1, new Color (1, 1, 0), null, null));
		levelQueue.Enqueue (thisWave);

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
		Vector3 randomPosition = new Vector3 (-(widthOfScreen - 2) / 2 + Random.value * (widthOfScreen - 2), topOfScreen + Random.value, 0);
		return generateRandEnemyAtPosition(dolphin, whale, narwhal, shark, randomPosition); 
	}

	Transform generateRandEnemyAtX(int dolphin, int whale, int narwhal, int shark, float x) {
		Vector3 position = new Vector3(x, topOfScreen, 0);
		return generateRandEnemyAtPosition(dolphin, whale, narwhal, shark, position); 
	}

	Transform generateRandEnemyAtPosition(int dolphin, int whale, int narwhal, int shark, Vector3 pos) {
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

		return generateEnemyAtPosition (target, pos);
	}

	Transform generateEnemyAtPosition(Transform target, Vector3 pos) {
		return Network.Instantiate (target, pos, Quaternion.identity, 0) as Transform;
	}

	Transform generateEnemyAtRandomPosition(Transform target) {
		Vector3 randomPosition = new Vector3 (-(widthOfScreen - 2) / 2 + Random.value * (widthOfScreen - 2), topOfScreen + Random.value, 0);
		return Network.Instantiate (target, randomPosition, Quaternion.identity, 0) as Transform;
	}

	// Update is called once per frame
	void Update () {
		if (structuredMode) {
			enemyCounter++;
			if (enemyCounter > respawnTime) {
				enemyCounter = 0;
				if (levelQueue.Count == 0) {
					// Final boss!
				}
				else {
					Wave thisWave = levelQueue.Dequeue();
					while (thisWave.enemies.Count > 0) {
						EnemySave enemy = thisWave.enemies.Dequeue();

						// Generates clone at given or random position and with given or random transform
						Transform clone;
						if (enemy.xPosition == -1) {
							if (enemy.transform == null) {
								clone = generateRandEnemy (enemy.typeRandomizers[0], enemy.typeRandomizers[1], enemy.typeRandomizers[2], enemy.typeRandomizers[3]);
							} else {
								clone = generateEnemyAtRandomPosition (enemy.transform);
							}
						}
						else {
							if (enemy.transform == null) {
								clone = generateRandEnemyAtX (enemy.typeRandomizers[0], enemy.typeRandomizers[1], enemy.typeRandomizers[2], enemy.typeRandomizers[3], enemy.xPosition);
							} else {
								clone = generateEnemyAtPosition (enemy.transform, new Vector3(enemy.xPosition, topOfScreen, 0));
							}
						}

						// Set color to given color or randomly
						Color col;
						if (enemy.color == Color.black) {
							col = generateRandColor(enemy.colorRandomizers[0], enemy.colorRandomizers[1], enemy.colorRandomizers[2], enemy.colorRandomizers[3]);  
						} else {
							col = enemy.color;
						}
						
						EnemyBehavior actualClone = clone.GetComponent<EnemyBehavior> ();
						actualClone.networkView.RPC ("SetColor", RPCMode.All, new Vector3 (col.r, col.g, col.b));

					}
				}
			}
		}
		else {
			enemyCounter++;
			if (enemyCounter > respawnTime) {
					enemyCounter = 0;

					int numEnemies = 4;
					for (int i = 0; i < numEnemies; i++) {
						Transform clone = generateRandEnemy (0, 1, 0, 1);
						EnemyBehavior actualClone = clone.GetComponent<EnemyBehavior> ();

						Color col = generateRandColor (2, 1, 0, 0);
						actualClone.networkView.RPC ("SetColor", RPCMode.All, new Vector3 (col.r, col.g, col.b));
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
}

public class Wave {
	public Queue<EnemySave> enemies;
	public int respawnTime;
	public Wave (int time) {
		enemies = new Queue<EnemySave> ();
		respawnTime = time;
	}
}

public class EnemySave {
	public Transform transform;
	public float xPosition;
	public Color color; 

	public int[] typeRandomizers; // if type = -1, use this
	public int[] colorRandomizers; // if color is null, use this
	public EnemySave () {
		transform = null;
		xPosition = -1;
		color = Color.black;
		typeRandomizers = new int[4];
		colorRandomizers = new int[4];
	}
	public EnemySave (Transform transf, float x, Color c, int[] typeR, int[] colorR) {
		transform = transf;
		xPosition = x;
		color = c;
		typeRandomizers = typeR;
		colorRandomizers = colorR;
	}
}
