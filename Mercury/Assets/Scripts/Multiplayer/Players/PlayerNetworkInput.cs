using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkInput : MonoBehaviour {

	void Update () {
        Vector2 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) {
            moveDir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDir += Vector2.right;
        }

        moveDir.Normalize();

        NetworkModel.instance.SetModel(GameDeathmatch.clientUniqueID + "Move", NetworkVector3.FromVector3(moveDir));
    }
}
