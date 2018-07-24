using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SocketRecieveState {
    public IClientConnection clientConnection = null;
    public const int BufferSize = 256;
    public byte[] buffer = new byte[BufferSize];
}
