﻿using System;
using System.Collections.Generic;
using UnityEngine;
using tablerealms.comms.message;
using UnityEngine.SceneManagement;

public abstract class TableRealmsPeerToPeerNetwork : MonoBehaviour {

    protected bool iAmTheHost = false;

    public static TableRealmsPeerToPeerNetwork instance = null;
    public bool debugMessages = true;

    private static HashSet<Type> customMessageTypes = new HashSet<Type>();
    private static Dictionary<Type, P2PCommandInterface> messageCommands = new Dictionary<Type, P2PCommandInterface>();
    private static Dictionary<string, Type> messageTypesMap = new Dictionary<string, Type>();
    private Queue<P2PMessage> messagesRecieved = new Queue<P2PMessage>();

    public GameObject[] networkedPrefabs;
    private Dictionary<long, TableRealmsPlayerId> networkedGameObjects = new Dictionary<long, TableRealmsPlayerId>();
    private Dictionary<string, GameObject> networkedPrefabsDictionary = new Dictionary<string, GameObject>();

    virtual public void Awake() {
        // Since we are a p2p game make the host flag flase it will be set later whent the protocol is learnt.
        TableRealmsGameNetwork.host = false;
    }

    virtual public void Start() {
        if (instance != null) {
            Debug.LogError("TableRealms: More then one instance of TableRealmsPeerToPeerNetwork created.");
            Destroy(this);
        } else {
            GameObject.DontDestroyOnLoad(gameObject);
            instance = this;
            foreach (GameObject go in networkedPrefabs) {
                networkedPrefabsDictionary.Add(go.name,go);
            }
        }
    }

    public virtual void Update() {
        while (messagesRecieved.Count > 0) {
            ProcessMessage(messagesRecieved.Dequeue());
        }
    }

    public void InitTables() {
        if (messageTypesMap.Count == 0) {
            List<Type> messageTypes = new List<Type>();
            messageTypes.Add(typeof(SP2PM));
            messageTypes.Add(typeof(IP2PM));
            messageTypes.Add(typeof(DP2PM));
            messageTypes.Add(typeof(MP2PM));
            messageTypes.Add(typeof(ScP2PM));
            messageTypes.Add(typeof(SMP2P));

            foreach (Type type in messageTypes) {
                messageTypesMap.Add(type.Name, type);
            }

            messageCommands.Add(typeof(SP2PM), new SceneP2PCommand());
            messageCommands.Add(typeof(IP2PM), new InstantiateP2PCommand());
            messageCommands.Add(typeof(DP2PM), new DestroyP2PCommand());
            messageCommands.Add(typeof(MP2PM), new MovementP2PCommand());
            messageCommands.Add(typeof(ScP2PM), new ScaleP2PCommand());
            messageCommands.Add(typeof(SMP2P), new SendMessageP2PCommand());
        }
    }

    public void AddCommand(Type type, P2PCommandInterface command) {
        InitTables();
        if (customMessageTypes.Contains(type)) {
            Debug.LogError("TableRealms: Unable to add command for type '"+ type.Name+"' is already mapped.");
        } else {
            customMessageTypes.Add(type);
            messageTypesMap.Add(type.Name, type);
            messageCommands.Add(type, command);
        }
    }

    public void RemoveCommand(P2PCommandInterface command) {
        List<string> typesToRemove = new List<string>();
        foreach (Type type in customMessageTypes) {
            if (messageCommands.ContainsKey(type)){
                messageCommands.Remove(type);
                typesToRemove.Add(type.Name);
            }
        }
        foreach (string typeName in typesToRemove) {
            messageTypesMap.Remove(typeName);
        }
    }

    public bool AmITheHost() {
        return iAmTheHost;
    }

    public void P2PSendMessage(long guid, string method) {
        if (networkedGameObjects.ContainsKey(guid)) {
            networkedGameObjects[guid].SendMessage(method, SendMessageOptions.RequireReceiver);
            if (TableRealmsGameNetwork.p2p && TableRealmsGameNetwork.host) {
                // Deliver message
                DeliverMessage(new SMP2P(guid,method));
            }
        } else {
            Debug.LogWarning("TableRealms: Unable to p2psendmessage '" + method + "' guid[" + guid + "] object not found");
        }
    }

    public void P2PSendMessage(long guid, string method, string param) {
        if (networkedGameObjects.ContainsKey(guid)) {
            networkedGameObjects[guid].SendMessage(method, param, SendMessageOptions.RequireReceiver);
            if (TableRealmsGameNetwork.host) {
                // Deliver message
                DeliverMessage(new SMP2P(guid, method, param));
            }
        } else {
            Debug.LogWarning("TableRealms: Unable to p2psendmessage '" + method + "' guid[" + guid + "] object not found");
        }
    }

    public void P2PSendMessage(long guid, string method, long param) {
        if (networkedGameObjects.ContainsKey(guid)) {
            networkedGameObjects[guid].SendMessage(method, param, SendMessageOptions.RequireReceiver);
            if (TableRealmsGameNetwork.host) {
                // Deliver message
                DeliverMessage(new SMP2P(guid, method, param));
            }
        } else {
            Debug.LogWarning("TableRealms: Unable to p2psendmessage '" + method + "' guid[" + guid + "] object not found");
        }
    }

    public void P2PSendMessage(long guid, string method, int param) {
        if (networkedGameObjects.ContainsKey(guid)) {
            networkedGameObjects[guid].SendMessage(method, param,SendMessageOptions.RequireReceiver);
            if (TableRealmsGameNetwork.host) {
                // Deliver message
                DeliverMessage(new SMP2P(guid, method, param));
            }
        } else {
            Debug.LogWarning("TableRealms: Unable to p2psendmessage '"+method+"' guid["+guid+"] object not found");
        }
    }

    public void P2PSendMessage(long guid, string method, float param) {
        if (networkedGameObjects.ContainsKey(guid)) {
            networkedGameObjects[guid].SendMessage(method, param, SendMessageOptions.RequireReceiver);
            if (TableRealmsGameNetwork.host) {
                // Deliver message
                DeliverMessage(new SMP2P(guid, method, param));
            }
        } else {
            Debug.LogWarning("TableRealms: Unable to p2psendmessage '" + method + "' guid[" + guid + "] object not found");
        }
    }

    public void P2PSendMessage(long guid, string method, double param) {
        if (networkedGameObjects.ContainsKey(guid)) {
            networkedGameObjects[guid].SendMessage(method, param, SendMessageOptions.RequireReceiver);
            if (TableRealmsGameNetwork.host) {
                // Deliver message
                DeliverMessage(new SMP2P(guid, method, param));
            }
        } else {
            Debug.LogWarning("TableRealms: Unable to p2psendmessage '" + method + "' guid[" + guid + "] object not found");
        }
    }

    public void P2PSendMessage(long guid, string method, Vector2 param) {
        if (networkedGameObjects.ContainsKey(guid)) {
            networkedGameObjects[guid].SendMessage(method, param);
            if (TableRealmsGameNetwork.host) {
                // Deliver message
                DeliverMessage(new SMP2P(guid, method, param));
            }
        }
    }

    public void P2PSendMessage(long guid, string method, Vector3 param) {
        if (networkedGameObjects.ContainsKey(guid)) {
            networkedGameObjects[guid].SendMessage(method, param);
            if (TableRealmsGameNetwork.host) {
                // Deliver message
                DeliverMessage(new SMP2P(guid, method, param));
            }
        }
    }

    public GameObject P2PInstantiate(string prefabName, string playerId, Vector3 location, Vector3 orientation) {
        return P2PInstantiate(prefabName, playerId, location, orientation, false);
    }

    public GameObject P2PInstantiate(string prefabName, string playerId, Vector3 location, Vector3 orientation, bool networkMovement) {
        if (TableRealmsGameNetwork.host) {
            return P2PInstantiateHost(prefabName, playerId, location, orientation, networkMovement);
        }
        return null;
    }

    public void P2PMovement(MP2PM message) {
        if (networkedGameObjects.ContainsKey(message.g)) {
            TableRealmsP2PMovementReciever tableRealmsP2PMovementReciever = networkedGameObjects[message.g].GetComponent<TableRealmsP2PMovementReciever>();
            if (tableRealmsP2PMovementReciever != null) {
                tableRealmsP2PMovementReciever.RecieveNextTarget(message);
            } else {
                Debug.LogError("TableRealms: Movement message recieved for guid[" + message.g + "] but the game object '" + networkedGameObjects[message.g].name + "' does not have a ");
            }
        }
    }

    public void P2PScale(ScP2PM message) {
        if (networkedGameObjects.ContainsKey(message.g)) {
            TableRealmsP2PMovementReciever tableRealmsP2PMovementReciever = networkedGameObjects[message.g].GetComponent<TableRealmsP2PMovementReciever>();
            if (tableRealmsP2PMovementReciever != null) {
                tableRealmsP2PMovementReciever.RecieveNewScale(message);
            } else {
                Debug.LogError("TableRealms: Scale message recieved for guid[" + message.g + "] but the game object '" + networkedGameObjects[message.g].name + "' does not have a ");
            }
        }
    }

    public GameObject P2PInstantiateClient(IP2PM message) {
        if (networkedPrefabsDictionary.ContainsKey(message.n)) {
            GameObject newObject = Instantiate(networkedPrefabsDictionary[message.n]);
            newObject.transform.position = message.l;
            newObject.transform.rotation = Quaternion.Euler(message.o.x, message.o.y, message.o.z);

            TableRealmsPlayerId tableRealmsPlayerId = newObject.AddComponent<TableRealmsPlayerId>();
            tableRealmsPlayerId.SetClientData(message);
            AddNetworkedGameObject(tableRealmsPlayerId);

            if (message.m) {
                newObject.AddComponent<TableRealmsP2PMovementReciever>();
            }
            newObject.SendMessage("StartP2P", SendMessageOptions.DontRequireReceiver);
            return newObject;
        } else {
            Debug.LogError("TableRealms: Unable to find prefab named '" + message.n + "' in prefab list.");
        }
        return null;
    }

    private GameObject P2PInstantiateHost(string prefabName, string playerId,Vector3 position, Vector3 orientation, bool networkMovement) {
        if (prefabName.EndsWith("(Clone)")) {
            prefabName = prefabName.Substring(0, prefabName.Length - 7);
        }
        if (networkedPrefabsDictionary.ContainsKey(prefabName)) {
            GameObject newObject = Instantiate(networkedPrefabsDictionary[prefabName]);
            TableRealmsPlayerId tableRealmsPlayerId = newObject.AddComponent<TableRealmsPlayerId>();
            tableRealmsPlayerId.player = playerId;
            AddNetworkedGameObject(tableRealmsPlayerId);
            newObject.transform.position = position;
            newObject.transform.rotation = Quaternion.Euler(orientation.x, orientation.y, orientation.z);


            if (networkMovement) {
                newObject.AddComponent<TableRealmsP2PMovementSender>();
            }

            // Send network instruction to create Game Object
            DeliverMessage(new IP2PM(playerId, prefabName, newObject.GetComponent<TableRealmsPlayerId>().guid, position, orientation,networkMovement));

            newObject.SendMessage("StartP2P", SendMessageOptions.DontRequireReceiver);

            return newObject;
        } else {
            Debug.LogError("TableRealms: Unable to find prefab named '" + prefabName + "' in prefab list.");
        }
        return null;
    }

    public void AddNetworkedGameObject(TableRealmsPlayerId tableRealmsPlayerId) {
        if (networkedGameObjects.ContainsKey(tableRealmsPlayerId.guid)) {
            RemoveNetworkedGameObject(tableRealmsPlayerId.guid);
        }
        networkedGameObjects.Add(tableRealmsPlayerId.guid, tableRealmsPlayerId);
    }

    public void RemoveNetworkedGameObject(long guid) {
        if (networkedGameObjects.ContainsKey(guid)){
            TableRealmsPlayerId tableRealmsPlayerId = networkedGameObjects[guid];
            networkedGameObjects.Remove(guid);
            // Send network instruction to destroy Game Object
            if (TableRealmsGameNetwork.p2p && TableRealmsGameNetwork.host) {
                DeliverMessage(new DP2PM(guid));
            } else {
                Destroy(tableRealmsPlayerId.gameObject);
            }
        } else {
            Debug.LogError("TableRealms: Unable to remove guid '" + guid + "' in prefab list.");
        }
    }

    public void RecieveMessageString(string message) {
        InitTables();

        if (debugMessages) {
            Debug.Log("TableRealms: Received " + message);
        }

        int idx = message.IndexOf("|");
        if (idx != -1) {
            string messageKey = message.Substring(0, idx);
            if (!messageTypesMap.ContainsKey(messageKey)) {
                Debug.LogError("TableRealms:  Unable to read message unknown type '" + message + "'");
            } else {
                Type messageType = messageTypesMap[messageKey];
                messagesRecieved.Enqueue((P2PMessage)JsonUtility.FromJson(message.Substring(idx + 1), messageType));
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

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        if (iAmTheHost) {
            Debug.Log("TableRealms: Scene Change loading to '" + sceneName + "'.");
            DeliverMessage(new SP2PM(sceneName,false));
        }
    }

    public void MergeScene(string sceneName) {
        SceneManager.LoadScene(sceneName,LoadSceneMode.Additive);
        if (iAmTheHost) {
            Debug.Log("TableRealms: Scene Change merging '" + sceneName + "'.");
            DeliverMessage(new SP2PM(sceneName,true));
        }
    }

    public string GetMessageName(P2PMessage message) {
        InitTables();

        return message.GetType().Name;
    }

    public void DeliverMessage(P2PMessage message) {
        if (customMessageTypes.Contains(message.GetType())) {
            ProcessMessage(message);
        }
        DeliverMessageToClients(message);
    }


    public abstract string GetSessionId();
    public abstract void DeliverMessageToClients(P2PMessage message);

    }
