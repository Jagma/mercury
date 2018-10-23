using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDeathmatch : MonoBehaviour {

    public static string gameUniqueID;
    public static string clientUniqueID = "-1";
    public static bool isServer = false;

    private void Awake() {
        NetworkManager.instance.Subscribe(ReceiveMessage);
    }

    void Start () {
        NetworkMessages.StartGame startGame = new NetworkMessages.StartGame();
        startGame.gameUniqueID = gameUniqueID;
        NetworkManager.instance.Send(startGame);

        GameObject input = new GameObject("Input");
        input.AddComponent<PlayerNetworkInput>();
    }
	

	void Update () {
	}

    public void ReceiveMessage (NetworkMessages.MessageBase message) {
        if (message.GetType() == typeof(NetworkMessages.GameStarted)) {
            NetworkMessages.GameStarted gameStarted = (NetworkMessages.GameStarted)message;
            isServer = gameStarted.isHost;
            clientUniqueID = gameStarted.thisClientID;

            if (isServer == true) {
                for (int i = 0; i < gameStarted.clientUniqueIDs.Length; i++) {

                    // Host player
                    Factory.HostPlayerConstructor hostPlayerConstructor = new Factory.HostPlayerConstructor();
                    hostPlayerConstructor.objectUniqueID = gameStarted.clientUniqueIDs[i];
                    hostPlayerConstructor.Construct();

                    // Client player
                    NetworkMessages.CreateObject createClientPlayer = new NetworkMessages.CreateObject();
                    createClientPlayer.gameUniqueID = gameUniqueID;

                    Factory.ClientPlayerConstructor clientPlayerConstructor = new Factory.ClientPlayerConstructor();
                    clientPlayerConstructor.objectUniqueID = gameStarted.clientUniqueIDs[i];

                    createClientPlayer.objectConstructor = clientPlayerConstructor;
                    NetworkManager.instance.Send(createClientPlayer);
                }
            }
        }
    }
}
