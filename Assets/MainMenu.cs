using UnityEngine;
using System.Collections;


public enum MenuState {
	Main,
	ServerList,
	Lobby,
	Highscores,
	None
}

public class MainMenu : MonoBehaviour {

	public static string gameID = "CrossBeams";
	public static int DEFAULT_PORT = 9050;

	private MenuState state = MenuState.Main;
	private MenuState nextState = MenuState.None;

	public Transform[] MainButtons;
	public Transform BackButton;
	public Transform LaunchButton;

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
		Network.InitializeServer(32, DEFAULT_PORT, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameID, "Join My Game", "Ready to start.");
		nextState = MenuState.Lobby;
	}

	[RPC]
	void startLevel() {
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

			int players = Network.connections.Length + 1;
			GUILayout.Label(players.ToString());
		} else {
			LaunchButton.gameObject.SetActive(false);
		}
	}
}
