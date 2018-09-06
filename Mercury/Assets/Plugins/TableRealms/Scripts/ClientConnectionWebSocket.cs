using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if UNITY_WEBGL
public class ClientConnectionWebSocket : IClientConnection {
 #if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void TableRealmsConnectToClient(string gameObjectName,string url);

    [DllImport("__Internal")]
    private static extern void TableRealmsSendToClient(string url,string message);
#endif

    private string url;
    private Func<string, bool> incommingMessageCallBack;
    private SocketRecieveState socketRecieveState = new SocketRecieveState();
    private Boolean closed = false;
    private string gameObjectName;
    private List<string> storedMessages = new List<string>();

    public ClientConnectionWebSocket(string url, string gameObjectName) {
        this.url = url;
        this.gameObjectName = gameObjectName;
    }

    public string GetRemoteEndPoint() {
        return url;
    }

    public bool IsConnected() {
        return !closed;
    }

    public void Close() {
        closed = true;
    }

    public void Write(string message) {
#if UNITY_WEBGL && !UNITY_EDITOR
        TableRealmsSendToClient(url, message);
#endif
    }

    public void BeginRead() {
#if UNITY_WEBGL && !UNITY_EDITOR
        TableRealmsConnectToClient(gameObjectName,url);
#endif
    }

    public void RecieveMessage(string message) {
        if (incommingMessageCallBack == null) {
            storedMessages.Add(message);
        } else if (!incommingMessageCallBack(message)) {
            Close();
        }
    }

    public void ReadAndDeliverTo(Func<string, bool> incommingMessageCallBack) {
        this.incommingMessageCallBack = incommingMessageCallBack;
        List<string> oldStoredMessages = storedMessages;
        storedMessages = null;
        foreach (string message in oldStoredMessages) {
            if (!incommingMessageCallBack(message)) {
                Close();
            }
        }
    }
}
#endif