using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TableRealmsPeerToPeerNetwork : MonoBehaviour {

    protected bool iAmTheHost = false;

    public static TableRealmsPeerToPeerNetwork instance = null;

    virtual public void Start() {
        if (instance != null) {
            Debug.LogError("TableRealms: More then one instance of TableRealmsPeerToPeerNetwork created.");
            Destroy(this);
        } else {
            GameObject.DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public bool AmITheHost() {
        return iAmTheHost;
    }

    public abstract string GetSessionId();
    public abstract void RecieveMessageString(string messageString);

}
