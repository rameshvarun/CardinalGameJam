using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	/// <summary>
	/// In release, set this to 3.
	/// </summary>
	public static int NUM_PLAYERS = 2;


	int playersLoaded = 0;

	/// <summary>
	/// Called on a client when it loses connection with the server.
	/// </summary>
	/// <param name="info">Info.</param>
	public void OnDisconnectedFromServer(NetworkDisconnection info) {
		MainMenu.info = info;
		Debug.LogError("Disconnected from server: " + info);
		Application.LoadLevel(0);
	}

	/// <summary>
	/// Called on the server when it loses connection with the client.
	/// </summary>
	/// <param name="player">Player.</param>
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.LogError("A Player disconnected.");
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
		if(playersLoaded >= GameManager.NUM_PLAYERS) {
			networkView.RPC("AllPlayersLoaded", RPCMode.All);
		}
	}

	/// <summary>
	/// A delayed load call that waits for all players to enter this scene.
	/// </summary>
	[RPC]
	void AllPlayersLoaded() {
		Debug.Log("All players loaded scene.");
		// Spawn players
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer) {
			// Spawn enemies and stuff
		}
	}
}
