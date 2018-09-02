using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_WEBGL
public class ClientConnectionListenerWebSocket : MonoBehaviour, IClientConnectionListener {

    [DllImport("__Internal")]
    private static extern void TableRealmsProxyList(string gameObjectName,string sessionId,string serviceUrl);

    [DllImport("__Internal")]
    private static extern void TableRealmsStarted();

    static private bool initialized = false;

    private Func<IClientConnection, bool> callback;
    private string serviceUrl = "dev.red-oxygen.com";
    private string serverUrl = null;
    private string sessionId = null;
    private string gameName = null;

    private float lastConnectOrDisconected;
    private float nextTestTime = 0;

    private Dictionary<string, ClientConnectionWebSocket> clientConnections=new Dictionary<string, ClientConnectionWebSocket>();

    public void Initialize(string gameName) {
        if (!initialized) {
            initialized = true;
            this.gameName = gameName;
            lastConnectOrDisconected = Time.realtimeSinceStartup;
            TableRealmsStarted();
        }
    }
  
    public void Update() {
        if (nextTestTime < Time.realtimeSinceStartup && initialized) {
            PollForConnections();
            float timeGap = Math.Min(10,((Time.realtimeSinceStartup - lastConnectOrDisconected) /60)+2);
            nextTestTime = Time.realtimeSinceStartup + timeGap;
        }
    }

    private void PollForConnections() {
        TableRealmsProxyList(gameObject.name, sessionId, "http://"+serviceUrl);
    }

    public void ClientUrlOpen(string url) {
        ClientConnectionWebSocket clientConnectionWebSocket = clientConnections[url];
        if (!callback(clientConnectionWebSocket)) {
            clientConnectionWebSocket.Close();
            clientConnections.Remove(url);
        }
    }

    public void AddUrl(string url) {
        lastConnectOrDisconected = Time.realtimeSinceStartup;
        ClientConnectionWebSocket clientConnectionWebSocket = new ClientConnectionWebSocket(url,gameObject.name);
        if (clientConnections.ContainsKey(url)) {
            clientConnections.Remove(url);
        }
        clientConnections.Add(url,clientConnectionWebSocket);
    }

    public void SetSessionId(string sessionId) {
        this.sessionId = sessionId;
    }

    public void CloseClientConnection(string url) {
        lastConnectOrDisconected = Time.realtimeSinceStartup;
        if (clientConnections.ContainsKey(url)) {
            ClientConnectionWebSocket clientConnectionWebSocket = clientConnections[url];
            if (clientConnectionWebSocket.IsConnected()) {
                clientConnectionWebSocket.Close();
            }
            clientConnections.Remove(url);
        }
    }

    public void RecieveMessage(string message) {
        int index = message.IndexOf("|");
        string url = message.Substring(0, index);
        message = message.Substring(index + 1);
        if (clientConnections.ContainsKey(url)){
            ClientConnectionWebSocket clientConnectionWebSocket = clientConnections[url];
            if (clientConnectionWebSocket.IsConnected()) {
                clientConnectionWebSocket.RecieveMessage(message);
            } else {
                clientConnections.Remove(url);
            }
        }
    }

    public string GetConnectionString() {
        if (sessionId != null) {
            serverUrl = "http://tbrm.me?i=" + WWW.EscapeURL(serviceUrl) + "&n=" + WWW.EscapeURL(gameName) + "&g=" + WWW.EscapeURL(sessionId) + "&p=NWS";
        }
        return serverUrl;
    }

    public void AcceptClientConnections(Func<IClientConnection, bool> callback) {
        this.callback = callback;
    }

}
#endif