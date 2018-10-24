using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayer : MonoBehaviour {

    public string clientUniqueID;

    public PlayerActor playerActor;

    private void Start() {
        CameraSystem.instance.SubscribeToTracking(transform);
        if (GameDeathmatch.clientUniqueID == clientUniqueID) {
            GameDeathmatch.instance.myActor = playerActor;
        }
    }

    void Update() {
        // Position
        if (NetworkModel.instance.GetModel(clientUniqueID + "Position") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "Position");
            transform.position = v3.ToVector3();
        }

        // Look direction
        if (NetworkModel.instance.GetModel(clientUniqueID + "LookDirection") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "LookDirection");
            playerActor.Aim(v3.ToVector3());
        }

        // Attack
        if (NetworkModel.instance.GetModel(clientUniqueID + "Attack") != null) {
            bool attack = (bool)NetworkModel.instance.GetModel(clientUniqueID + "Attack");
            if (attack) {
                playerActor.Attack();
            }            
        }
    }
}
