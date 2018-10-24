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
        if (NetworkModel.instance.GetModel(clientUniqueID + "Move") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "Move");
            playerActor.Move(v3.ToVector3());
        }

        // Aim
        if (NetworkModel.instance.GetModel(clientUniqueID + "Aim") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "Aim");
            playerActor.Aim(v3.ToVector3());
        }

        // Attack
        if (NetworkModel.instance.GetModel(clientUniqueID + "Attack") != null) {
            bool attack = (bool)NetworkModel.instance.GetModel(clientUniqueID + "Attack");
            playerActor.Attack();
        }

        NetworkModel.instance.SetModel(clientUniqueID + "Position", NetworkVector3.FromVector3(transform.position));
    }
}
