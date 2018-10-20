using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerC : NetworkBehaviour {

    Rigidbody rigid;

	void Start () {
        rigid = GetComponent<Rigidbody>();

        if (isServer == false) {
            rigid.isKinematic = true;            
        }
    }

    // Client
    Vector3 targetPosition;
    Vector3 targetRotation;

    // Server
    Vector3 lastPosition = Vector3.zero;
    Vector3 lastRotation = Vector3.zero;
    void FixedUpdate() {
        // Server side
        if (isServer) {
            if (lastPosition != transform.position) {
                lastPosition = transform.position;
                RpcSetPosition(transform.position);
            }

            if (lastRotation != transform.localEulerAngles) {
                lastRotation = transform.localEulerAngles;
                RpcSetRotation(transform.localEulerAngles);
            }

        }

        // Client side
        // Local object
        if (isLocalPlayer) {
            Vector3 movement = Vector3.zero;
            movement.x = Input.GetAxis("Horizontal") * Time.deltaTime * 3;
            movement.z = Input.GetAxis("Vertical") * Time.deltaTime * 3;

            // Send input to server
            CmdMove(movement);

            // Estimate movement
            targetPosition += movement;
        }

        // Remote object
        if (isServer == false) {
            // Interpolate to last known true postion
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.3f);
            float lerpTime = 0.2f;
            transform.localEulerAngles = new Vector3(
                Mathf.LerpAngle(transform.localEulerAngles.x, targetRotation.x, lerpTime),
                Mathf.LerpAngle(transform.localEulerAngles.y, targetRotation.y, lerpTime),
                Mathf.LerpAngle(transform.localEulerAngles.z, targetRotation.z, lerpTime));
        }
    }

    // Server side
    [Command]
    void CmdMove(Vector3 movement) {
        transform.position += movement;
    }

    [Command]
    void CmdRequestUpdate () {
        lastPosition = Vector3.one;
        lastRotation = Vector3.one;
    }

    // Client side
    [ClientRpc]
    void RpcSetPosition (Vector3 position) {
        targetPosition = position;
    }

    [ClientRpc]
    void RpcSetRotation(Vector3 rotation) {
        targetRotation = rotation;
    }
}
