using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInputManager : MonoBehaviour {
    public string playerID = "-1";
    public NetworkIdentity netID;
    void Update() {
        netID.SetModel("Input_Move", NetworkVector3.FromVector3(InputManager.instance.GetMoveDirection(playerID)));
        netID.SetModel("Input_Aim", NetworkVector3.FromVector3(InputManager.instance.GetAimDirection(playerID)));
        netID.SetModel("Input_InteractPressed", NetworkBool.FromBool(InputManager.instance.GetInteractPressed(playerID)));
        netID.SetModel("Input_UseAbility", NetworkBool.FromBool(InputManager.instance.GetUseAbility(playerID)));
    }
}
