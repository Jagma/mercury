﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class NetworkManager : MonoBehaviour {

    // TODO: Handle this in an elegant way :
    public static string gameUniqueID;
    public static string connectionUniqueID;
    public static bool isHost = false;
    // TODO_END

    public enum ConnectionStatus {
        Connected,
        Disconnected
    }

    public delegate void ReceiveMessage(NetworkMessages.MessageBase message);
    public List<ReceiveMessage> listeners = new List<ReceiveMessage>();

    WebSocket webSocket;
    string serverAddress = "ws://sleepy-brushlands-15753.herokuapp.com/lobby";
    ConnectionStatus connectionStatus = ConnectionStatus.Disconnected;
    
    JsonSerializerSettings jsonSettings;

    public static NetworkManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

            jsonSettings = new JsonSerializerSettings();
            jsonSettings.TypeNameHandling = TypeNameHandling.All;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        instance.Subscribe(OnReceiveMessage);
        StartCoroutine(EHeartbeat());
    }
    
    public void Subscribe(ReceiveMessage callback) {
        // Remove all duplicates and subscribe newest caller
        listeners.Remove(callback);
        for (int i = 0; i < listeners.Count; i++) {
            if (listeners[i].Target.ToString() == "null" ||
                (listeners[i].Target.ToString() + listeners[i].Method.Name ==
                callback.Target.ToString() + listeners[i].Method.Name)) {
                listeners.RemoveAt(i);
                i--;
            }
        }
        
        listeners.Add(callback);
    }

    public ConnectionStatus GetConnectionStatus () {
        return connectionStatus;
    }

    public void Send (object messageObject) {
        string messageJson = JsonConvert.SerializeObject(messageObject, jsonSettings);
        Debug.Log("Client message : " + messageJson);
        webSocket.SendString(messageJson);
    }

    void Receive (string messageJson) {
        NetworkMessages.MessageBase messageObject = JsonConvert.DeserializeObject<NetworkMessages.MessageBase>(messageJson, jsonSettings);

        for (int i=0; i < listeners.Count; i++) {
            listeners[i](messageObject);
        }
    }


    // Continuous update loop
    private void Update() {
        if (webSocket == null) {
            return;
        }

        for (int i=0; i < 20; i ++){
            string message = webSocket.RecvString();

            if (message == null) {
                break;
            }
            if (message != null) {
                Debug.Log("Server message : " + message);
                Receive(message);
            }

            if (webSocket.error != null) {
                Debug.LogError("Error: " + webSocket.error);
            }
        }
    }

    // Heartbeat service
    bool connected = false;
    bool connectionAlive = false;
    void OnReceiveMessage (NetworkMessages.MessageBase messageObject) {
        if (messageObject.GetType() == typeof(NetworkMessages.RequestHeartbeat)) {
            Send(new NetworkMessages.Heartbeat());
        }
        if (messageObject.GetType() == typeof(NetworkMessages.Heartbeat)) {
            connectionAlive = true;
        }
        if (messageObject.GetType() == typeof(NetworkMessages.ConnectionEstablished)) {
            connected = true;
            connectionAlive = true;
        }
    }

    IEnumerator EHeartbeat() {
        if (connected == false) {
            webSocket = new WebSocket(new System.Uri(serverAddress));
            StartCoroutine(webSocket.Connect());
            Debug.Log("Attempt connection");
        }

        if (connected == true) {
            connectionAlive = false;
            Send(new NetworkMessages.RequestHeartbeat());
        }

        yield return new WaitForSecondsRealtime(2f);

        if (connected == true && connectionAlive == false) {
            connected = false;
            Debug.Log("Disconected");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Multiplayer");
        }

        // TODO: Add more status such as timeout
        if (connected == true) {
            connectionStatus = ConnectionStatus.Connected;
        } else {
            connectionStatus = ConnectionStatus.Disconnected;
        }

        StartCoroutine(EHeartbeat());
    }
}