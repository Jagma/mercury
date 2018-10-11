using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    //Dictionary<string, object> objectDictionary = new Dictionary<string, object>();

    public static ObjectManager instance;
    private void Awake() {
        instance = this;
    }

    void Start () {
        NetworkManager.instance.Subscribe(ReceiveMessage);
    }
	
    void ReceiveMessage (NetworkMessages.MessageBase message) {
        // Client messages only
        if (NetworkManager.isHost) {
            return;
        }

        if (message.GetType() == typeof(NetworkMessages.ObjectCreated)) {
            NetworkMessages.ObjectCreated objectCreated = (NetworkMessages.ObjectCreated)message;
            //GameObject result =  objectCreated.objectConstructor.Construct();
        }

        if (message.GetType() == typeof(NetworkMessages.ObjectDestroyed)) {
            // TODO: Destroy object
        }
    }
}
