using System;
#if (!UNITY_WEBGL || UNITY_EDITOR)
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
#endif
using UnityEngine;
#if UNITY_WSA
//using Windows.Networking.Connectivity;
#endif

public class ClientConnectionListenerSocket : MonoBehaviour, IClientConnectionListener{
#if (!UNITY_WEBGL || UNITY_EDITOR)

    static private TcpListener server = null;
    static private bool initialized = false;

    private int port = -1;
#endif
    private Func<IClientConnection, bool> callback;
    private string serverUrl;

    public void Initialize(string gameName) {
#if (!UNITY_WEBGL || UNITY_EDITOR)
        if (!initialized) {

#if (UNITY_WSA)
            port = 15000;
#endif
            initialized = true;

            //loop till you find a port which is available
            System.Random random = new System.Random();
            while (port == -1) {
                port = random.Next(1000, 65536);
                //check if the port is not in use
                if (!IsPortAvailable(port)) {
                    port = -1;
                }
            }

            server = new TcpListener(IPAddress.Any, port);

            server.Start();
            server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), server);

            serverUrl = "";

#if (UNITY_WSA)
            /*foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()) {
                if (item.OperationalStatus == OperationalStatus.Up) {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses) {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork) {
                            if (serverUrl.Length > 0) {
                                serverUrl = serverUrl + ",";
                            }
                            serverUrl = serverUrl + ip.Address.ToString();
                        }
                    }
                }
            }*/
#endif
            //serverUrl="192.168.1.176";
            serverUrl = "http://tbrm.me?i=" + WWW.EscapeURL(GetIP4Address()) + ":" + port + "&n=" + WWW.EscapeURL(gameName) + "&p=S";
            Debug.Log("TableRealms: listening for clients on " + serverUrl);
        }
#endif
    }
    
    public static string GetIP4Address(){
        string IP4Address = String.Empty;
        foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
        {
            if (IPA.AddressFamily == AddressFamily.InterNetwork)
            {
                IP4Address = IPA.ToString();
                break;
            }
        }
        return IP4Address;
    }

#if (!UNITY_WEBGL || UNITY_EDITOR)
    private bool IsPortAvailable(int port) {
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
        System.Collections.IEnumerator myEnum = tcpConnInfoArray.GetEnumerator();
        while (myEnum.MoveNext()) {
            TcpConnectionInformation tcpi = (TcpConnectionInformation)myEnum.Current;

            if (tcpi.LocalEndPoint.Port == port) {
                return false;
            }
        }

        return true;
    }
#endif

    public void DoAcceptTcpClientCallback(IAsyncResult ar) {
#if (!UNITY_WEBGL || UNITY_EDITOR)
        // Get the listener that handles the client request.
        TcpListener listener = (TcpListener)ar.AsyncState;

        // End the operation and display the received data on 
        // the console.
        TcpClient tcpClient = listener.EndAcceptTcpClient(ar);

        // If client is not accepted then close it
        if (!callback(new ClientConnectionSocket(tcpClient))) {
            tcpClient.Close();
        }

        server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), server);
#endif
    }

    public string GetConnectionString() {
        return serverUrl;
    }

    public void AcceptClientConnections(Func<IClientConnection, bool> callback) {
        this.callback = callback;
    }

}
