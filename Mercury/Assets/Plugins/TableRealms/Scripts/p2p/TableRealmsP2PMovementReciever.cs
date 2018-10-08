using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tablerealms.comms.message;

public class TableRealmsP2PMovementReciever : MonoBehaviour {

    private MP2PM target;

    // Use this for initialization
    void Start () {
        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        Rigidbody2D myRigidbody2D = GetComponent<Rigidbody2D>();
        if (myRigidbody != null) {
            myRigidbody.useGravity = false;
        }
        if (myRigidbody2D != null) {
            myRigidbody2D.gravityScale = 0;
        }
    }

    // Update is called once per frame
    void Update () {
        if (target != null) {
            transform.position = target.p;
            transform.rotation = Quaternion.Euler(target.r);
        }
    }

    public void RecieveNewScale(ScP2PM message) {
        if (transform.localScale != message.s) {
            transform.localScale = message.s;
        }
    }

    public void RecieveNextTarget(MP2PM message) {
        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        Rigidbody2D myRigidbody2D = GetComponent<Rigidbody2D>();
        if (myRigidbody != null) {
            target = null;
            transform.position = message.p;
            transform.rotation = Quaternion.Euler(message.r);
            myRigidbody.velocity = message.v;
            myRigidbody.angularVelocity = message.rv;
        } else if (myRigidbody2D != null) {
            target = null;
            transform.position = message.p;
            transform.rotation = Quaternion.Euler(message.r);
            myRigidbody2D.velocity = message.v;
            myRigidbody2D.angularVelocity = message.rv.x;
        } else {
            target = message;
        }

    }
}
