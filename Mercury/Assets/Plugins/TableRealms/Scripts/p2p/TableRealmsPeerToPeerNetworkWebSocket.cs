using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using tablerealms.comms.message;

public class TableRealmsPeerToPeerNetworkWebSocket : TableRealmsPeerToPeerNetwork {
#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern string TableRealmsGetProtocol();

    [DllImport("__Internal")]
    private static extern string TableRealmsAmIHost(string gameObjectName);
#endif
    
    private static Dictionary<Type, P2PCommandInterface> messageCommands = new Dictionary<Type, P2PCommandInterface>();
    private static Dictionary<string, Type> messageTypesMap = new Dictionary<string, Type>();

    private string sessionId;
    private bool calledHost=false;
    public bool debugMessages=true;

    static public TableRealmsPeerToPeerNetworkWebSocket instance = null;

    override public void Start() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
#if UNITY_WEBGL && !UNITY_EDITOR
            instance=this;
            GameObject.DontDestroyOnLoad(gameObject);

            // Are we running in Peer2Peer mode? If not then remove flag us as the host
            if (!"EmbeddedP2P".Equals(TableRealmsGetProtocol())) {
                Destroy(this);
                TableRealmsGameNetwork.p2p=false;
                TableRealmsGameNetwork.host=true;
                Debug.Log("TableRealms: P2P Not Enabled. Wrong protocol '"+ TableRealmsGetProtocol() + "'");
            } else {
                TableRealmsGameNetwork.p2p=true;
                TableRealmsGameNetwork.host=false;
                base.Start();
                SceneManager.activeSceneChanged += ChangedActiveScene;
                Debug.Log("TableRealms: P2P Enabled.");
            }
#else
            TableRealmsGameNetwork.p2p = false;
            TableRealmsGameNetwork.host = true;
            Destroy(this);
#endif
        }

    }

    public void Update() {
        if (!calledHost) {
            calledHost = true;
#if UNITY_WEBGL && !UNITY_EDITOR
            TableRealmsAmIHost(gameObject.name);
#endif
        }
    }

    public void InitTables() {
        if (messageTypesMap.Count == 0) {
            List<Type> messageTypes = new List<Type>();
            messageTypes.Add(typeof(SceneP2PMessage));

            foreach (Type type in messageTypes) {
                messageTypesMap.Add(type.Name, type);
            }

            messageCommands.Add(typeof(SceneP2PMessage), new SceneP2PCommand());
        }
    }

    override public string GetSessionId() {
        return sessionId;
    }

    public void YouAreTheHost() {
        iAmTheHost = true;
        TableRealmsGameNetwork.host = true;
    }

    public void SetHostSessionId(string sessionId) {
        this.sessionId = sessionId;
    }

    public string GetMessageName(P2PMessage message) {
        InitTables();

        return message.GetType().Name;
    }

    public void DeliverMessage(P2PMessage message) {
        InitTables();

        TableRealmsGameNetwork.instance.SendAllClientsP2PMessage(GetMessageName(message) + "|" + JsonUtility.ToJson(message, false) + "\n");
    }

    override public void RecieveMessageString(string message) {
        InitTables();

        if (debugMessages) {
            Debug.Log("TableRealms: Recieved " + message);
        }

        int idx = message.IndexOf("|");
        if (idx != -1) {
            string messageKey = message.Substring(0, idx);
            if (!messageTypesMap.ContainsKey(messageKey)) {
                Debug.LogError("TableRealms:  Unable to read message unknown type '" + message + "'");
            } else {
                Type messageType = messageTypesMap[messageKey];
                ProcessMessage((P2PMessage)JsonUtility.FromJson(message.Substring(idx + 1), messageType));
            }
        } else {
            Debug.LogError("TableRealms: Unable to read message badly formated '" + message + "'");
        }

    }

    private void ProcessMessage(P2PMessage message) {
        InitTables();
        P2PCommandInterface commandInterface = messageCommands[message.GetType()];
        if (commandInterface != null) {
            commandInterface.ProcessMessage(message);
        } else {
            Debug.LogError("TableRealms: Unable to process messsage type " + message.GetType());
        }

    }

    private void ChangedActiveScene(Scene current, Scene next) {
        if (iAmTheHost) {
            Debug.Log("TableRealms: Scene Change detected moving to '" + next.name + "'.");
            DeliverMessage(new SceneP2PMessage(next.name));
        }
    }

}
