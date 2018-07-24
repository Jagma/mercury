using System;
using System.Net.Sockets;
using System.Text;

public class ClientConnectionSocket : IClientConnection {
#if (!UNITY_WEBGL || UNITY_EDITOR)
    private TcpClient tcpClient;
    private Func<string, bool> incommingMessageCallBack;
    private SocketRecieveState socketRecieveState = new SocketRecieveState();

    public ClientConnectionSocket(TcpClient tcpClient) {
        this.tcpClient = tcpClient;
    }
#endif

    public string GetRemoteEndPoint() {
#if (!UNITY_WEBGL || UNITY_EDITOR)
        if (tcpClient != null && tcpClient.Client != null && tcpClient.Client.RemoteEndPoint != null)
        {
            return tcpClient.Client.RemoteEndPoint.ToString();
        }
        else
        {
            return "[Unknown]";
        }
#else
        return null;
#endif
    }

    public bool IsConnected() {
#if (!UNITY_WEBGL || UNITY_EDITOR)
        return tcpClient.Connected;
#else
        return true;
#endif
    }

    public void Close() {
#if (!UNITY_WEBGL || UNITY_EDITOR)
        tcpClient.Close();
#endif
    }

    public void Write(string message) {
#if (!UNITY_WEBGL || UNITY_EDITOR)
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        tcpClient.GetStream().BeginWrite(buffer,0,buffer.Length,null,null);
#endif
    }

#if (!UNITY_WEBGL || UNITY_EDITOR)
    private void BeginRead() {
        NetworkStream ns = tcpClient.GetStream();
        ns.BeginRead(socketRecieveState.buffer, 0, socketRecieveState.buffer.Length, EndRead, socketRecieveState);
    }

    private void EndRead(IAsyncResult result) {
        SocketRecieveState socketRecieveState = (SocketRecieveState)result.AsyncState;
        NetworkStream ns = tcpClient.GetStream();
        int bytesAvailable = ns.EndRead(result);

        if (incommingMessageCallBack(Encoding.ASCII.GetString(socketRecieveState.buffer, 0, bytesAvailable))) {
            BeginRead();
        }
    }
#endif

    public void ReadAndDeliverTo(Func<string, bool> incommingMessageCallBack) {
#if (!UNITY_WEBGL || UNITY_EDITOR)
        this.incommingMessageCallBack = incommingMessageCallBack;
        BeginRead();
#endif

    }
}
