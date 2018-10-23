using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayer : MonoBehaviour {

    public string clientUniqueID;
	void Start () {
		
	}

	void Update () {
        if (NetworkModel.instance.GetModel(clientUniqueID + "Move") != null) {
            NetworkVector3 v3 = (NetworkVector3)NetworkModel.instance.GetModel(clientUniqueID + "Move");
            Move(v3.ToVector3());
        }

        NetworkModel.instance.SetModel(clientUniqueID + "Position", NetworkVector3.FromVector3(transform.position));
    }

    public void Move(Vector2 moveDirection) {
        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * Time.deltaTime * 5;
    }
}
