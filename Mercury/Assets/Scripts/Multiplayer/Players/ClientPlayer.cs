using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayer : MonoBehaviour {

    public string clientUniqueID;

    public PlayerActor playerActor;

    private void Start() {        
        if (GameDeathmatch.clientUniqueID == clientUniqueID) {
            CameraSystem.instance.SubscribeToTracking(transform);
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
            NetworkBool attack = (NetworkBool)NetworkModel.instance.GetModel(clientUniqueID + "Attack");
            if (attack.ToBool() == true) {
                playerActor.Attack();
            }
        }
    }
}
