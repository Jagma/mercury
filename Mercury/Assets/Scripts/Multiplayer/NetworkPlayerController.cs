using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerController : MonoBehaviour {

    public NetworkIdentity netID;
    public PlayerActor actor;
    
	void Update () {
        if (netID.GetModel("Input_Move") != null) {
            NetworkVector3 netv3 = (NetworkVector3)netID.GetModel("Input_Move");
            actor.Move(netv3.ToVector3());
        }

        if (netID.GetModel("Input_Aim") != null) {
            NetworkVector3 netv3 = (NetworkVector3)netID.GetModel("Input_Aim");
            actor.Aim(netv3.ToVector3());
        }

        if (netID.GetModel("Input_InteractPressed") != null) {
            NetworkBool netBool = (NetworkBool)netID.GetModel("Input_InteractPressed");
            if (netBool.ToBool() == true) {
                actor.Interact();
            }
        }

        if (netID.GetModel("Input_Attack") != null) {
            NetworkBool netBool = (NetworkBool)netID.GetModel("Input_Attack");
            if (netBool.ToBool() == true) {
                actor.Attack();
            }
        }

        if (netID.GetModel("Input_UseAbility") != null) {
            NetworkBool netBool = (NetworkBool)netID.GetModel("Input_UseAbility");
            if (netBool.ToBool() == true) {
                actor.UseAbility();
            }
        }
    }
}
