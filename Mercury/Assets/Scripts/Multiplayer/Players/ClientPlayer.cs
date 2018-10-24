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

    Vector3 targetPos = Vector3.zero;
    Vector3 targetAim = Vector3.up;
    void Update() {
        // Position
        if (NetworkModel.instance.GetModel(clientUniqueID + "Position") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "Position");
            targetPos = v3.ToVector3();
        }

        // Look direction
        if (NetworkModel.instance.GetModel(clientUniqueID + "LookDirection") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "LookDirection");
            targetAim = v3.ToVector3();
        }

        // Attack
        if (NetworkModel.instance.GetModel(clientUniqueID + "Attack") != null) {
            NetworkBool attack = (NetworkBool)NetworkModel.instance.GetModel(clientUniqueID + "Attack");
            if (attack.ToBool() == true) {
                playerActor.Attack();
            }
        }

        // Interpolation
        if (Vector3.Distance(transform.position, targetPos) > 1) {
            transform.position = targetPos;
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.4f);

        playerActor.Aim(Vector3.Lerp(playerActor.model.lookDirection, targetAim, 0.4f));
    }
}
