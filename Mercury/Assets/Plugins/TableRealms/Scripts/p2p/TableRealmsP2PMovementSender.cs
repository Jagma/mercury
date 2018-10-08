using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tablerealms.comms.message;

public class TableRealmsP2PMovementSender : MonoBehaviour {

    private Vector3 oldLocation;
    private Vector3 oldRotaion;
    private Vector3 oldScale;
    private Vector3 oldVelocity = Vector3.zero;
    private Vector3 oldAngularVelocity = Vector3.zero;

    // Update is called once per frame
    void Update() {
        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        Rigidbody2D myRigidbody2D = GetComponent<Rigidbody2D>();
        if (myRigidbody != null) {
            Vector3 newVelocity = myRigidbody.velocity;
            Vector3 newAngularVelocity = myRigidbody.angularVelocity;
            if (oldVelocity != newVelocity || oldAngularVelocity != newAngularVelocity || (oldVelocity == Vector3.zero && oldLocation != transform.position) || (oldAngularVelocity == Vector3.zero && oldRotaion != transform.rotation.eulerAngles)) {
                oldRotaion = transform.rotation.eulerAngles;
                oldLocation = transform.position;
                TableRealmsPeerToPeerNetwork.instance.DeliverMessage(new MP2PM(GetComponent<TableRealmsPlayerId>().guid, Time.time, oldLocation, oldRotaion, newVelocity, newAngularVelocity));
                oldVelocity = newVelocity;
                oldAngularVelocity = newAngularVelocity;
            }

        } else if (myRigidbody2D != null) {
            Vector3 newVelocity = myRigidbody2D.velocity;
            Vector3 newAngularVelocity = new Vector3(myRigidbody2D.angularVelocity,0,0);
            if (oldVelocity != newVelocity || oldAngularVelocity != newAngularVelocity || (oldVelocity == Vector3.zero && oldLocation != transform.position) || (oldAngularVelocity == Vector3.zero && oldRotaion != transform.rotation.eulerAngles)) {
                oldRotaion = transform.rotation.eulerAngles;
                oldLocation = transform.position;
                TableRealmsPeerToPeerNetwork.instance.DeliverMessage(new MP2PM(GetComponent<TableRealmsPlayerId>().guid, Time.time, oldLocation, oldRotaion, newVelocity, newAngularVelocity));
                oldVelocity = newVelocity;
                oldAngularVelocity = newAngularVelocity;
            }
        } else {
            if (oldLocation != transform.position || oldRotaion != transform.rotation.eulerAngles) {
                oldLocation = transform.position;
                oldRotaion = transform.rotation.eulerAngles;
                TableRealmsPeerToPeerNetwork.instance.DeliverMessage(new MP2PM(GetComponent<TableRealmsPlayerId>().guid, Time.time, oldLocation, oldRotaion, Vector3.zero, Vector3.zero));
            }
        }
        if (oldScale != transform.localScale) {
            oldScale = transform.localScale;
            TableRealmsPeerToPeerNetwork.instance.DeliverMessage(new ScP2PM(GetComponent<TableRealmsPlayerId>().guid, Time.time, oldScale));
        }
    }


}
