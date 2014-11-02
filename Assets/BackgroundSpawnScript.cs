using UnityEngine;
using System.Collections;

public class BackgroundSpawnScript : MonoBehaviour {

	public Transform stars1;
	private Transform currentStar1;

	public Transform stars2;
	private Transform currentStar2;

	public Transform stars3;
	private Transform currentStar3;

	public Transform[] entities;
	//private float timer = 0.0f;
	//private float nextTime = 0.0f;

	public static int seed = 10;
	private System.Random random;
	public Vector2 nextRange;
	private float nextTime;

	private int lastNum = 0;

	// Use this for initialization
	void Start () {

		currentStar1 = Instantiate(stars1, getMiddlePosition(), Quaternion.identity) as Transform;
		currentStar2 = Instantiate(stars2, getMiddlePosition(), Quaternion.identity) as Transform;
		currentStar3 = Instantiate(stars3, getMiddlePosition(), Quaternion.identity) as Transform;

		random = new System.Random(seed);
		SpawnEntity(getMiddlePosition());

		Application.runInBackground = true;
	}

	void SpawnEntity(Vector3 position) {
		position += new Vector3(0,0,0);

		int num = random.Next(0, entities.Length);
		if(num == lastNum) num = (num + 1) % entities.Length;
		bool flipped = random.Next(0, 2) == 0;

		Transform entity = entities[num];
		Transform spawned = Instantiate(entity, position, Quaternion.identity) as Transform;
		if(flipped)
			spawned.localScale = new Vector3(-spawned.localScale.x, spawned.localScale.y, spawned.localScale.z);

		nextTime = Time.realtimeSinceStartup + ((float)random.NextDouble()) * (nextRange.y - nextRange.x) + nextRange.x;
		lastNum = num;
	}

	Vector3 getMiddlePosition() {
		Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.transform.position.z * -2.0f));
		Debug.Log (Camera.main.transform.position.z * -2.0f);
		return position;
	}

	public static Vector3 getSpawnPosition() {
		Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.transform.position.z * -2.0f));
		position += new Vector3(0, 11, 0);
		return position;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentStar1.transform.position.y < 0) {
			currentStar1 = Instantiate(stars1, getSpawnPosition() + new Vector3(0,0,0), Quaternion.identity) as Transform;
		}

		if(currentStar2.transform.position.y < 0) {
			currentStar2 = Instantiate(stars2, getSpawnPosition() + new Vector3(0,0,0), Quaternion.identity) as Transform;
		}

		if(currentStar3.transform.position.y < 0) {
			currentStar3 = Instantiate(stars3, getSpawnPosition() + new Vector3(0,0,0), Quaternion.identity) as Transform;
		}

		if(Time.realtimeSinceStartup > nextTime) {
			SpawnEntity(getSpawnPosition());
		}
	}
}
