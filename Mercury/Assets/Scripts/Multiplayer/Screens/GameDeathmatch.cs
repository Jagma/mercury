using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDeathmatch : MonoBehaviour {

    public static GameDeathmatch instance;

    public static string gameUniqueID;
    public static string clientUniqueID = "-1";
    public static bool isServer = false;

    public Texture2D deathmatchDesign;
    List<PlayerActor> playerActorList = new List<PlayerActor>();
    List<Transform> spawnList = new List<Transform>();

    private void Awake() {
        NetworkManager.instance.Subscribe(ReceiveMessage);
        instance = this;
    }

    PlayerActor myActor;
    public PlayerActor GetPlayerActor(string playerID) {
        return myActor;
    }

    void Start () {
        NetworkMessages.StartGame startGame = new NetworkMessages.StartGame();
        startGame.gameUniqueID = gameUniqueID;
        NetworkManager.instance.Send(startGame);

        GameObject input = new GameObject("Input");
        input.AddComponent<PlayerNetworkInput>();

        GenerateLevel();
    }
	

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MultiplayerHome");
        }
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
                    GameObject player = hostPlayerConstructor.Construct();

                    if (gameStarted.clientUniqueIDs[i] == gameStarted.thisClientID) {
                        myActor = player.GetComponent<PlayerActor>();
                    }

                    SpawnNextPlayer(player);

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


    public void GenerateLevel () {
        Transform levelRoot = new GameObject("LevelRoot").transform;
        AudioManager.instance.PlayAudio("Game_music_Magellanic_clouds", .4f, true);
        for (int z = 0; z < deathmatchDesign.height; z++) {
            for (int x = 0; x < deathmatchDesign.width; x++) {
                Color col = deathmatchDesign.GetPixel(x, z);
                if (col == new Color(1, 0, 0)) {
                    GameObject floorGO = Factory.instance.CreateFloor("Spaceship", 0); // Floor
                    floorGO.transform.parent = levelRoot;
                    floorGO.transform.position = new Vector3(x, 0, z);
                }
                if (col == new Color(0, 1, 0)) {
                    GameObject floorGO = Factory.instance.CreateFloor("Spaceship", 2); // Spawn
                    floorGO.transform.localEulerAngles = new Vector3(0, Random.Range(0, 4) * 90, 0);
                    floorGO.transform.parent = levelRoot;
                    floorGO.transform.position = new Vector3(x, 0, z);
                    spawnList.Add(floorGO.transform);
                }
                if (col == new Color(0, 0, 1)) {
                    GameObject wallGO = Factory.instance.CreateWall("Spaceship", 0); // Wall
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                    wallGO.GetComponent<Wall>().health = int.MaxValue;
                }
            }
        }
    }

    int playerNum = 0;
    public void SpawnNextPlayer (GameObject player) {
        Transform spawn = spawnList[0];
        spawnList.RemoveAt(0);
        
        player.transform.position = spawn.position + Vector3.up;
        playerActorList.Add(player.GetComponent<PlayerActor>());
        playerNum++;

        CameraSystem.instance.SubscribeToTracking(player.transform);
    }

}
