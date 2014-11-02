using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenuState {
	Main,
	ServerList,
	Lobby,
	Highscores,
	None
}

public enum ShipColor {
	Red,
	Green,
	Blue
}

public class MainMenu : MonoBehaviour {

	public static string gameID = "CrossBeams";
	public static int DEFAULT_PORT = 9050;

	private MenuState state = MenuState.Main;
	private MenuState nextState = MenuState.None;

	public Transform[] MainButtons;
	public Transform BackButton;
	public Transform LaunchButton;

	public static NetworkDisconnection info;

	private int players = 0;

	public static ShipColor myColor;

	public void OnDisconnectedFromServer(NetworkDisconnection info) {
		MainMenu.info = info;
		Debug.LogError("Disconnected from server: " + info);
		nextState = MenuState.Main;
	}

	// Use this for initialization
	void Start () {
		MasterServer.RequestHostList(gameID);
		Application.runInBackground = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(nextState != MenuState.None) {
			if(state == MenuState.Main) {
				foreach(Transform button in MainButtons ) {
					button.gameObject.SetActive(false);
				}
			}
			if(nextState == MenuState.Main) {
				foreach(Transform button in MainButtons ) {
					button.gameObject.SetActive(true);
				}
			}

			state = nextState;
			nextState = MenuState.None;
		}

		if(state == MenuState.Highscores || state == MenuState.Lobby || state == MenuState.ServerList) {
			BackButton.gameObject.SetActive(true);
		}else {
			BackButton.gameObject.SetActive(false);
		}
	}

	void JoinGame() {
		MasterServer.RequestHostList(gameID);
		nextState = MenuState.ServerList;
	}

	void HostGame() {
		Network.InitializeServer(2, DEFAULT_PORT, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameID, "Join My Game", "Ready to start.");
		nextState = MenuState.Lobby;

		players = 1;
	}

	[RPC]
	void StartLevel(int color, int seed) {
		myColor = (ShipColor)color;
		BackgroundSpawnScript.seed = seed;
		Application.LoadLevel(1);
	}

	void Highscore() {
		nextState = MenuState.Highscores;
	}

	void Back() {
		if(state == MenuState.Lobby) {
			Network.Disconnect();
		}
		nextState = MenuState.Main;
	}

	void OnPlayerConnected(NetworkPlayer player) {
		++players;
		networkView.RPC("SetPlayers", RPCMode.Others, players);
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		--players;
		networkView.RPC("SetPlayers", RPCMode.Others, players);
	}

	[RPC]
	void SetPlayers(int numPlayers) {
		players = numPlayers;
	}

	void Launch() {
		List<int> colors = new List<int>();
		colors.Add((int)ShipColor.Red);
		colors.Add((int)ShipColor.Green);
		colors.Add((int)ShipColor.Blue);
		Utils.Shuffle(colors);

		int seed = Random.Range(-100, 100);

		if(players >= 1) StartLevel(colors[0], seed);
		if(players >= 2) networkView.RPC("StartLevel", Network.connections[0], colors[1], seed);
		if(players >= 3) networkView.RPC("StartLevel", Network.connections[1], colors[2], seed);
	}

	void OnGUI() {
		if(state == MenuState.ServerList) {
			HostData[] data = MasterServer.PollHostList();

			// Go through all the hosts in the host list
			foreach (HostData element in data) {
				GUILayout.BeginHorizontal();	
				string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
				GUILayout.Label(name);	
				GUILayout.Space(5);
				
				string hostInfo;
				hostInfo = "[";
				foreach (string host in element.ip)
					hostInfo = hostInfo + host + ":" + element.port + " ";

				hostInfo = hostInfo + "]";
				GUILayout.Label(hostInfo);	
				GUILayout.Space(5);
				GUILayout.Label(element.comment);
				GUILayout.Space(5);
				GUILayout.FlexibleSpace();
				
				if (GUILayout.Button("Connect"))
				{
					// Connect to HostData struct, internally the correct method is used (GUID when using NAT).
					Network.Connect(element);
					nextState = MenuState.Lobby;
				}
				GUILayout.EndHorizontal();	
			}

			if(GUILayout.Button("Refresh")) {
				MasterServer.RequestHostList(gameID);
			}
		}


		if(state == MenuState.Lobby) {
			if(Network.isServer) {
				GUILayout.Label("You are hosting.");
			} else {
				GUILayout.Label("You are not hosting.");
			}

			GUILayout.Label(players.ToString());

			if(players >= GameManager.NUM_PLAYERS && Network.isServer) {
				LaunchButton.gameObject.SetActive(true);
			} else {
				LaunchButton.gameObject.SetActive(false);
			}

		} else {
			LaunchButton.gameObject.SetActive(false);
		}
	}
}
