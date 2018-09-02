using System;
using System.Runtime.InteropServices;

#if UNITY_WEBGL
public class ClientConnectionWebSocket : IClientConnection {
    [DllImport("__Internal")]
    private static extern void TableRealmsConnectToClient(string gameObjectName,string url);

    [DllImport("__Internal")]
    private static extern void TableRealmsSendToClient(string url,string message);

    private string url;
    private Func<string, bool> incommingMessageCallBack;
    private SocketRecieveState socketRecieveState = new SocketRecieveState();
    private Boolean closed = false;
    private string gameObjectName;

    public ClientConnectionWebSocket(string url, string gameObjectName) {
        this.url = url;
        this.gameObjectName = gameObjectName;
        BeginRead();
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
        TableRealmsSendToClient(url, message);
    }

    private void BeginRead() {
        TableRealmsConnectToClient(gameObjectName,url);
    }

    public void RecieveMessage(string message) {
        if (!incommingMessageCallBack(message)) {
            Close();
        }
    }

    public void ReadAndDeliverTo(Func<string, bool> incommingMessageCallBack) {
        this.incommingMessageCallBack = incommingMessageCallBack;
    }
}
#endif