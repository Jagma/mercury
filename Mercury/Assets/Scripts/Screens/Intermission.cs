using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intermission : MonoBehaviour {

    static List<PlayerModel> playerList = new List<PlayerModel>();

    public static void SetPlayers (List<PlayerModel> playerList) {
        Intermission.playerList = playerList;
    }

    public static Intermission instance;
    private void Awake() {
        instance = this;
    }

    public Texture2D spaceshipDesign;
	void Start () {
        Transform levelRoot = new GameObject("LevelRoot").transform;
        AudioManager.instance.PlayAudio("Game_music_Magellanic_clouds", .4f, true);
        for (int z = 0; z < spaceshipDesign.height; z++) {
            for (int x = 0; x < spaceshipDesign.width; x++) {
                Color col = spaceshipDesign.GetPixel(x, z);
                if (col == new Color(1, 1, 0)) {
                    GameObject floorGO = Factory.instance.CreateFloor("Spaceship", 0); // Floor
                    floorGO.transform.parent = levelRoot;
                    floorGO.transform.position = new Vector3(x, 0, z);
                }
                if (col == new Color(0, 1, 0)) {
                    GameObject floorGO = Factory.instance.CreateFloor("Spaceship", 1); // Upgrade
                    floorGO.transform.localEulerAngles = new Vector3(0, Random.Range(0, 4) * 90, 0);
                    floorGO.transform.parent = levelRoot;
                    floorGO.transform.position = new Vector3(x, 0, z);
                }
                if (col == new Color(0, 1, 1)) {
                    GameObject floorGO = Factory.instance.CreateFloor("Spaceship", 2); // Spawn
                    floorGO.transform.parent = levelRoot;
                    floorGO.transform.position = new Vector3(x, 0, z);
                    floorGO.transform.localEulerAngles = new Vector3(0, Random.Range(0, 4) * 90, 0);

                    SpawnNextPlayer(new Vector3(x, 2, z));
                }

                if (col == new Color(1, 1, 1)) {
                    GameObject wallGO = Factory.instance.CreateWall("Spaceship", 0); // Wall
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);                    
                }
                if (col == new Color(0, 0, 1)) {
                    GameObject wallGO = Factory.instance.CreateWall("Spaceship", 1); // Computer
                    wallGO.transform.parent = levelRoot;
                    wallGO.transform.position = new Vector3(x, 1, z);
                    wallGO.transform.localEulerAngles = new Vector3(0, Random.Range(1, 4) * 90, 0);
                }
            }
        }
    }

    int teleportCount = 0;
    public void PlayerTeleport (PlayerActor actor) {
        teleportCount++;

        if (teleportCount >= playerList.Count) {
            AudioManager.instance.StopAudio("Game_music_Magellanic_clouds");
            ProgressionState.NextLevel();
            if (ProgressionState.level >= 6) {
                SceneManager.LoadScene("GameComplete");
            } else {
                SceneManager.LoadScene("GameCampaign");
            }            
        }
    }

    List<PlayerActor> playerActorList = new List<PlayerActor>();

    public PlayerActor GetPlayerActor(string playerID) {
        for (int i = 0; i < playerActorList.Count; i++) {
            if (playerActorList[i].model.playerID == playerID) {
                return playerActorList[i];
            }
        }

        return null;
    }

    int playerNum = 0;
    void SpawnNextPlayer (Vector3 position) {
        if (playerNum >= playerList.Count) {
            return;
        }

        GameObject player = null;
        if (playerList[playerNum].ability.GetType() == typeof(AbilityTrump)) {
            player = Factory.instance.CreatePlayerTrump();
        }
        if (playerList[playerNum].ability.GetType() == typeof(AbilityBinLaden)) {
            player = Factory.instance.CreatePlayerBinLaden();
        }
        if (playerList[playerNum].ability.GetType() == typeof(AbilityOprah)) {
            player = Factory.instance.CreatePlayerOprah();
        }
        if (playerList[playerNum].ability.GetType() == typeof(AbilityPope)) {
            player = Factory.instance.CreatePlayerPope();
        }

        player.transform.position = position;
        playerActorList.Add(player.GetComponent<PlayerActor>());
        player.GetComponent<PlayerActor>().model = playerList[playerNum];
        playerNum++;

        CameraSystem.instance.SubscribeToTracking(player.transform);
    }

    public void PlayerReady(PlayerActor actor) {

    }
}
