using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game instance;
    private void Awake()
    {
        instance = this;
    }

    List<PlayerActor> playerActorList = new List<PlayerActor>();


	void Start ()
    {
        AudioManager.instance.PlayAudio("Game_music_Moon_garden", .5f, true);

        if (InputManager.instance == null)
        {
            return;
        }
        LevelGeneration.instance.Generate();



        Dictionary<string, PlayerInput> playerDictionary = InputManager.instance.GetPlayerInputDictionary();
        foreach(KeyValuePair<string, PlayerInput> playerInputKVP in playerDictionary)
        {
            if (playerInputKVP.Value.inputType == PlayerInput.InputType.TableRealms) {
                TableRealmsDevice device = TableRealmsManager.instance.GetDevice(playerInputKVP.Value.playerID);
                device.DisplayPage("Controller");
            }

            GameObject playerGO = new GameObject();
            if (PlayerData.GetCharacterName(playerInputKVP.Value.playerID).Equals("Trump"))
            {
                playerGO = Factory.instance.CreatePlayerTrump();
            }

            if (PlayerData.GetCharacterName(playerInputKVP.Value.playerID).Equals("The Pope"))
            {
                playerGO = Factory.instance.CreatePlayerPope();
            }

            if (PlayerData.GetCharacterName(playerInputKVP.Value.playerID).Equals("Oprah"))
            {
                playerGO = Factory.instance.CreatePlayerOprah();
            }
            if (PlayerData.GetCharacterName(playerInputKVP.Value.playerID).Equals("Bin Laden"))
            {
                playerGO = Factory.instance.CreatePlayerBinLaden();
            }

            playerGO.transform.position = LevelGeneration.instance.playerSpawnPosition;

            PlayerActor playerActor = playerGO.GetComponent<PlayerActor>();
            playerActor.model.playerID = playerInputKVP.Value.playerID;

            playerActorList.Add(playerActor);

            CameraSystem.instance.SubscribeToTracking(playerGO.transform);
        }
    }

    public PlayerActor GetPlayerActor (string playerID)
    {
        for (int i=0; i < playerActorList.Count; i++)
        {
            if (playerActorList[i].model.playerID == playerID)
            {
                return playerActorList[i];
            }
        }

        return null;
    }

    int gameStage = 0;
    private void Update() {
        if (gameStage == 0 && EnemyManager.instance.GetEnemyCount() <= 0) {
            gameStage++;

            Vector3 playerMid = Vector3.zero;
            for (int i=0; i < playerActorList.Count; i ++) {
                playerMid += playerActorList[i].transform.position;
            }

            playerMid /= playerActorList.Count;

            LevelGeneration.instance.SpawnMartianBoss(playerMid);
        }
        if (gameStage == 1 && EnemyManager.instance.GetEnemyCount() <= 0) {
            NavigateIntermission();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            NavigateIntermission();
        }
    }

    void NavigateIntermission () {
        List<PlayerModel> modelList = new List<PlayerModel>();

        for (int i=0; i < playerActorList.Count; i ++) {
            modelList.Add(playerActorList[i].model);
        }

        Intermission.SetPlayers(modelList);

        AudioManager.instance.StopAudio("Game_music_Moon_garden");
        SceneManager.LoadScene("Intermission");
    }
}
