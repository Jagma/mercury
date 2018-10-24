using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayer : MonoBehaviour {

    public string clientUniqueID;
    public PlayerActor playerActor;

	void Start () {
		
	}

	void Update () {
        // Movement
        if (NetworkModel.instance.GetModel(clientUniqueID + "Input_Move") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "Input_Move");
            playerActor.Move(v3.ToVector3());
        }

        // Look
        if (NetworkModel.instance.GetModel(clientUniqueID + "Input_Look") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "Input_Look");
            playerActor.Aim(v3.ToVector3());
        }

        // Attack
        if (NetworkModel.instance.GetModel(clientUniqueID + "Input_Attack") != null) {
            NetworkBool attack = (NetworkBool)NetworkModel.instance.GetModel(clientUniqueID + "Input_Attack");
            if (attack.ToBool() == true) {
                playerActor.Attack();
            }
            
            NetworkModel.instance.SetModel(clientUniqueID + "Attack", attack);
        }

        NetworkModel.instance.SetModel(clientUniqueID + "Position", NetworkVector3.FromVector3(transform.position));
        NetworkModel.instance.SetModel(clientUniqueID + "LookDirection", NetworkVector3.FromVector3(playerActor.model.lookDirection));
        
    }
}
