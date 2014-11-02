using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	/// <summary>
	/// In release, set this to 3.
	/// </summary>
	public static int NUM_PLAYERS = 3;
	public SpawnEnemyScript enemySpawner;


	int playersLoaded = 0;
	bool isLoaded = false;

	public Transform redPlayer;
	public Transform greenPlayer;
	public Transform bluePlayer;

	public int score = 0;

	/// <summary>
	/// Called on a client when it loses connection with the server.
	/// </summary>
	/// <param name="info">Info.</param>
	public void OnDisconnectedFromServer(NetworkDisconnection info) {
		MainMenu.info = info;
		Debug.LogError("Disconnected from server: " + info);
		Application.LoadLevel(0);
	}

	[RPC]
	void AddScore(int addition) {
		score += addition;
	}

	/// <summary>
	/// Called on the server when it loses connection with the client.
	/// </summary>
	/// <param name="player">Player.</param>
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.LogError("A Player disconnected.");
		Network.Disconnect();
		Application.LoadLevel(0);
	}


	// Use this for initialization
	void Start () {
		if(Network.isClient)
			networkView.RPC("PlayerLoaded", RPCMode.Server);
		else
			PlayerLoaded();
	}

	/// <summary>
	/// All players notify the server that they have loaded the scene with this RPC.
	/// </summary>
	[RPC]
	void PlayerLoaded() {
		++playersLoaded;
		Debug.Log(playersLoaded + " players loaded.");
		if(playersLoaded >= GameManager.NUM_PLAYERS && !isLoaded) {
			networkView.RPC("AllPlayersLoaded", RPCMode.All);
		}
	}

	/// <summary>
	/// A delayed load call that waits for all players to enter this scene.
	/// </summary>
	[RPC]
	void AllPlayersLoaded() {
		Debug.Log("All players loaded scene.");
		isLoaded = true;

		if (Network.isServer)
						GetComponent<SpawnEnemyScript> ().enabled = true;

		// Spawn players
		switch(MainMenu.myColor){
		case ShipColor.Red:
			Network.Instantiate(redPlayer, GameObject.Find("RedSpawn").transform.position, GameObject.Find ("RedSpawn").transform.rotation, 0);
			break;
		case ShipColor.Blue:
			Network.Instantiate(bluePlayer, GameObject.Find("BlueSpawn").transform.position, GameObject.Find ("RedSpawn").transform.rotation, 0);
			break;
		case ShipColor.Green:
			Network.Instantiate(greenPlayer, GameObject.Find("GreenSpawn").transform.position, GameObject.Find ("RedSpawn").transform.rotation, 0);
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer && isLoaded) {
		}

		GameObject.Find("Score").GetComponent<Text>().text = "Score: " + score;
		GameObject.Find("ScoreShadow").GetComponent<Text>().text = "Score: " + score;
	}
}
