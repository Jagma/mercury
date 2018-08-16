﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (InputManager.instance == null)
        {
            return;
        }
        LevelGeneration.instance.Generate();


        Dictionary<string, PlayerInput> playerDictionary = InputManager.instance.GetPlayerInputDictionary();
        foreach(KeyValuePair<string, PlayerInput> playerInputKVP in playerDictionary)
        {
            //Trump character
            //GameObject playerGO = Factory.instance.CreatePlayerTrump();

            //Oprah character
            GameObject playerGO = Factory.instance.CreatePlayerOprah();

            //Set starting position
            playerGO.transform.position = LevelGeneration.instance.playerSpawnPosition;

            PlayerActor playerActor = playerGO.GetComponent<PlayerActor>();
            playerActor.model.playerID = playerInputKVP.Value.playerID; ;

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

}
