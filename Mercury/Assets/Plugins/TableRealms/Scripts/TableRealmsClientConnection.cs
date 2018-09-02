using UnityEngine;
using System;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using tablerealms.comms.message;

public class TableRealmsClientConnection : MonoBehaviour  {
    const float SEND_HEARTBEAT_AT_LEAST_EVERY_N_SECONDS = 5;
    const float SEND_HEARTBEAT_IF_IDLE_FOR_N_SECONDS = 1;

    private IClientConnection clientConnection;
    private StringBuilder messageBuilder = new StringBuilder();
    private bool closed = false;
	public bool debugMessages = false;
    public string id;

    public enum State {
        Connecting,
        Loading,
        UpdatingModel,
        Active,
        Disconnected
    }

    private Queue<string> messagesRecieved = new Queue<string>();
    private float lastMessageSendOrRecieved;
    private float lastHeartbeat;

    private static Dictionary<Type, CommandInterface> messageCommands = new Dictionary<Type, CommandInterface>();

    private static Dictionary<string, Type> messageTypesMap = new Dictionary<string, Type>();
    private Type playerCommandScript;

    public State state = State.Connecting;

    private Thread unityThread = null;

    void Start() {
        unityThread = Thread.CurrentThread;

        lastMessageSendOrRecieved = Time.realtimeSinceStartup;
        lastHeartbeat = Time.realtimeSinceStartup - SEND_HEARTBEAT_AT_LEAST_EVERY_N_SECONDS;

        if (messageTypesMap.Count == 0) { 
            List<Type> messageTypes = new List<Type>();
            messageTypes.Add(typeof(HeartbeatMessage));
            messageTypes.Add(typeof(CommandMessage));
            messageTypes.Add(typeof(UpdateModelMessage));
            messageTypes.Add(typeof(DesignMessage));
            
            foreach (Type type in messageTypes) {
                messageTypesMap.Add(type.Name, type);
            }

            messageCommands.Add(typeof(HeartbeatMessage), new HeartbeatCommand());
            messageCommands.Add(typeof(DesignMessage), new DesignCommand());
            messageCommands.Add(typeof(UpdateModelMessage), new UpdateModelCommand());
            messageCommands.Add(typeof(CommandMessage), new CommandCommand());
        }
    }
    
    public void SetPlayerCommandScript(Type playerCommandScript) {
        this.playerCommandScript = playerCommandScript;
    }

    void Update() {
        while (messagesRecieved.Count > 0){
            lastMessageSendOrRecieved = Time.realtimeSinceStartup;
            ProcessMessage(messagesRecieved.Dequeue());
        }

        if (!clientConnection.IsConnected()) {
            Debug.Log("TableRealms: local client disconected from " + clientConnection.GetRemoteEndPoint());
            GameObject.Destroy(gameObject);
        }

        if (Time.realtimeSinceStartup-lastMessageSendOrRecieved >= SEND_HEARTBEAT_IF_IDLE_FOR_N_SECONDS || Time.realtimeSinceStartup - lastHeartbeat >= SEND_HEARTBEAT_AT_LEAST_EVERY_N_SECONDS) {
            SendHeartBeat();
        }
    }

    public void SetState(State state) {
        this.state = state;
        switch (state) {
            case State.Connecting:
                break;
            case State.Loading:
                break;
            case State.UpdatingModel:
                SendPlayerGlobalModel();
                break;
            case State.Active:
                AddPlayerCommandScriptToGameObject();
                break;
            case State.Disconnected:
                break;
        }
    }

    private void AddPlayerCommandScriptToGameObject() {
        if (playerCommandScript != null) {
            Debug.LogWarning("TableRealms: Client script available and added." + playerCommandScript);
            gameObject.AddComponent(playerCommandScript);
        } else {
            Debug.LogWarning("TableRealms: No client script available to be added.");
        }
    }

    private void SendPlayerGlobalModel() {
        foreach (string key in TableRealmsModel.instance.globalKeys) {
            object value = TableRealmsModel.instance.GetDataRaw(key);
            String type = ModelType.TypeString.ToString();
            if (value is String) {
                type = ModelType.TypeString.ToString();
            } else if (value is int || value is long || value is float || value is double) {
                type = ModelType.TypeDouble.ToString();
            } else if (value is bool) {
                type = ModelType.TypeBoolean.ToString();
            }
            SendClientMessage(new UpdateModelMessage(true, key, type, value.ToString()));
        }

        UpdateModelMessage updateModelMessage = new UpdateModelMessage(false, "state", ModelType.TypeString.ToString(), "Active");
        SetState(State.Active);
        SendClientMessage(updateModelMessage);
    }

    public void SendHeartBeat() {
        lastHeartbeat = Time.realtimeSinceStartup;
        SendClientMessage(new HeartbeatMessage((long)(lastHeartbeat * 1000)));
    }

    public void SetHeartbeat(HeartbeatMessage heartbeatMessage) {
        SetModelData("latency", (long)((Time.realtimeSinceStartup * 1000) - heartbeatMessage.tick) / 2L);
    }

    public void SetModelData(string key, long value) {
        TableRealmsModel.instance.SetData(id + "." + key, value);
    }

    public void SetModelData(string key, float value) {
        TableRealmsModel.instance.SetData(id + "." + key, value);
    }

    public oftype GetModelData<oftype>(string key) {
        return TableRealmsModel.instance.GetData<oftype>(id + "." + key);
    }

    public void OnDestroy() {

        if (!clientConnection.IsConnected()) {
            try {
                clientConnection.Close();
            } catch (Exception e) {
                Debug.LogError("TableRealms: "+e.Message);
            }
        }

        if (TableRealmsGameNetwork.instance != null) {
            TableRealmsGameNetwork.instance.RemoveConnection(this);
        } else {
            Debug.LogError("TableRealms: Destroy called but no TableRealmsGameNetwork instance can be found.");
        }
    }

    public string GetMessageName(Message message) {
        return message.GetType().Name;
    }

    public void SendClientMessage(Message message) {
        if (Thread.CurrentThread == unityThread) {
            lastMessageSendOrRecieved = Time.realtimeSinceStartup;
        }
        SendClientMessage(GetMessageName(message) + "|" + JsonUtility.ToJson(message, false) + "\n");
    }

    public void SendClientMessage(string message) {
        if (debugMessages && !message.StartsWith("HeartbeatMessage|") && !message.StartsWith("DesignMessage|")) {
            Debug.Log("TableRealms: Sending " + message);
        }

        clientConnection.Write(message);
    }

    private void ProcessMessage(string message) {
        if (debugMessages && !message.StartsWith("HeartbeatMessage|") && !message.StartsWith("DesignMessage|")) {
            Debug.Log("TableRealms: Recieved " + message);
        }

        int idx = message.IndexOf("|");
        if (idx != -1) {
            Type messageType = messageTypesMap[message.Substring(0, idx)];
            if (messageType != null) {
                ProcessMessage((Message)JsonUtility.FromJson(message.Substring(idx + 1), messageType));
            } else {
                Debug.LogError("TableRealms: Unable to read message unknown type '" + message + "'");
            }
        } else {
            Debug.LogError("TableRealms: Unable to read message badly formated '" + message + "'");
        }

    }

    private void ProcessMessage(Message message) {
        CommandInterface commandInterface = messageCommands[message.GetType()];
        if (commandInterface != null) {
            commandInterface.ProcessMessage(message,this);
        } else {
            Debug.LogError("TableRealms: Unable to process messsage type "+ message.GetType());
        }

    }

    public void SetClientConnection(IClientConnection clientConnection) {
        this.clientConnection = clientConnection;
        Debug.Log("TableRealms: local client accepted from " + clientConnection.GetRemoteEndPoint());
        clientConnection.ReadAndDeliverTo(RecieveBytes);
    }

    private bool RecieveBytes(string newText) {
        for (int cr = newText.IndexOf("\n"); cr != -1; cr = newText.IndexOf("\n")) {
            messageBuilder.Append(newText, 0, cr);

            string message = messageBuilder.ToString();
            if (message.StartsWith("DesignMessage|")) {
                ProcessMessage(message);
            } else {
                messagesRecieved.Enqueue(message);
            }

            messageBuilder = new StringBuilder();
            newText = newText.Substring(cr + 1);
        }
        messageBuilder.Append(newText);

        return true;
    }

}
