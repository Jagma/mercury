using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    /*  TODO List 
     *  2)  Implement LeaveGame and GameLeft messages.
     */ 

    Dictionary<string, object> objectDictionary = new Dictionary<string, object>();

    public static ObjectManager instance;
    private void Awake() {
        instance = this;
    }

    void Start () {
        NetworkManager.instance.Subscribe(ReceiveMessage);
    }
	
    void ReceiveMessage (NetworkMessages.MessageBase message) {
        // Client messages only
        if (GameDeathmatch.isServer) {
            return;
        }

        if (message.GetType() == typeof(NetworkMessages.ObjectCreated)) {
            NetworkMessages.ObjectCreated objectCreated = (NetworkMessages.ObjectCreated)message;
            GameObject result =  objectCreated.objectConstructor.Construct();
        }

        if (message.GetType() == typeof(NetworkMessages.ObjectDestroyed)) {
            // TODO: Destroy object
        }
    }
}
