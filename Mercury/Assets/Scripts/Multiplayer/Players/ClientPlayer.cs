using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayer : MonoBehaviour {

    public string clientUniqueID;

    void Update() {
        if (NetworkModel.instance.GetModel(clientUniqueID + "Position") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "Position");
            transform.position = v3.ToVector3();
        }
    }
}
