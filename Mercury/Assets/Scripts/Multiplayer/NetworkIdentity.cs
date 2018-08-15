using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkIdentity : MonoBehaviour {
    public string ownerID = "-1";
    public string uniqueID = "-1";

    public void SetModel (string key, object value) {
        NetworkModel.instance.SetModel(ownerID + "|" + uniqueID + "|" + key, value);
    }

    public object GetModel(string key) {
        return NetworkModel.instance.GetModel(ownerID + "|" + uniqueID + "|" + key);
    }

}
