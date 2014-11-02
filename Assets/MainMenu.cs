using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

	public Transform GamePanel;

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
		Application.runInBackground = true;
	}

	int dots = 0;
	float dotTimer = 0;
	
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

		if(state == MenuState.ServerList) {
			GamePanel.gameObject.SetActive(true);
		} else {
			GamePanel.gameObject.SetActive(false);
		}

		if(state == MenuState.Lobby) {
			dotTimer += Time.deltaTime;
			if(dotTimer > 1.0f) {
				++dots;
				dotTimer = 0.0f;
			}

			string dotsText = "";
			for(int i = 0; i < dots % 4; ++ i)
				dotsText += ".";

			if(players < GameManager.NUM_PLAYERS)
				GameObject.Find("LobbyStatus").GetComponentInChildren<Text>().text = "Waiting for " + (GameManager.NUM_PLAYERS - players) + " players" + dotsText;
			else {
				if(Network.isServer) {
					GameObject.Find("LobbyStatus").GetComponentInChildren<Text>().text = "Waiting to launch" + dotsText;
				} else {
					GameObject.Find("LobbyStatus").GetComponentInChildren<Text>().text = "Waiting for host" + dotsText;
				}
			}
		} else {
			GameObject.Find("LobbyStatus").GetComponentInChildren<Text>().text = "";
		}
	}

	void JoinGame() {
		RefreshGames();
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

	public Transform ServerPanel;
	void RefreshGames() {
		GamePanel.gameObject.SetActive(true);
		MasterServer.RequestHostList(gameID);

		HostData[] data = MasterServer.PollHostList();

		GameObject servers = GameObject.Find("Servers");
		for(int j = 0; j < servers.transform.childCount; ++j) {
			Destroy(servers.transform.GetChild(j).gameObject);
		}
		
		// Go through all the hosts in the host list
		int i = 0;
		foreach (HostData element in data) {
			//GUILayout.BeginHorizontal();	
			string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
			//GUILayout.Label(name);	
			//GUILayout.Space(5);
			
			string hostInfo;
			hostInfo = "[";
			foreach (string host in element.ip)
				hostInfo = hostInfo + host + ":" + element.port + " ";
			
			hostInfo = hostInfo + "]";

			Transform panel = Instantiate(ServerPanel, servers.transform.position, Quaternion.identity) as Transform;
			panel.GetComponentInChildren<Text>().text = name;
			panel.parent = servers.transform;
			panel.localScale = new Vector3(1, 1, 1);
			panel.Translate(0, 2.5f, 0);
			panel.Translate(0, -1.0f * i, 0);
			++i;

			panel.GetComponent<Button>().onClick.AddListener(
				() => {Network.Connect(element); nextState = MenuState.Lobby; }
			);
		}
	}
	
	void OnGUI() {
		if(state == MenuState.ServerList) {

		}


		if(state == MenuState.Lobby) {
			/*if(Network.isServer) {
				GUILayout.Label("You are hosting.");
			} else {
				GUILayout.Label("You are not hosting.");
			}*/

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
