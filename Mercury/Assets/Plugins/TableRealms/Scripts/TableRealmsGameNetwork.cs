using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using ZXing;
using ZXing.QrCode;
using tablerealms.comms.message;
using UniWebServer;
using ICSharpCode.SharpZipLib.Zip;

public class TableRealmsGameNetwork : MonoBehaviour {

    static private bool initialized = false;
    static public TableRealmsGameNetwork instance = null;

    static private ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

    static private List<TableRealmsClientConnection> connections = new List<TableRealmsClientConnection>();
    static private HashSet<string> connectionIds = new HashSet<string>();

    public string gameName = "No Name";
    public TextAsset tableRealmsDesign;
    public byte[] designBytes;
    public Texture2D joinQRCodeTexture;

    public string playerCommandScriptName;
    public Type playerCommandScript;

    private string serverUrl;

    private IClientConnectionListener clientConnectionListener;
    private Queue<IClientConnection> pendingClientConnections = new Queue<IClientConnection>();
    private static CultureInfo cultureInfo = new CultureInfo("en-US");

    void Start() {
        if (tableRealmsDesign == null) {
            Debug.LogError("TableRealms: No project file was set for tableRealmsDesign. System will not work.");
        } else {
            Debug.Log("TableRealms: Project file = " + tableRealmsDesign .name + ".");
        }

        if (!initialized) {
            instance = this;
            InitServer();
            GameObject.DontDestroyOnLoad(gameObject);

            if (playerCommandScriptName != null && playerCommandScriptName.Length > 0)
            {
                playerCommandScript = Type.GetType(playerCommandScriptName);
                if (playerCommandScript == null)
                {
                    playerCommandScript = Type.GetType(playerCommandScriptName + ",Assembly-CSharp");
                }
                if (playerCommandScript == null)
                {
                    Debug.LogError("TableRealms: Unable to find type for '" + playerCommandScriptName + "'");
                }
            }
            else
            {
                Debug.LogWarning("TableRealms: No player specific script configured.");
            }

            designBytes = tableRealmsDesign.bytes;

        }
        else {
            Debug.LogWarning("TableRealmsGameNetwork should only be instatiated once. There is one on '" + gameObject.scene.name + ":" + gameObject.name+"' when one was aslready instantiated.");
            Destroy(gameObject);
        }
    }

    void Update() {
        if (serverUrl==null) {
            serverUrl = clientConnectionListener.GetConnectionString();
            if (serverUrl != null) {
                joinQRCodeTexture = GenerateQR(serverUrl);
            }
        }

        while (pendingClientConnections.Count > 0) {
            AddClient(pendingClientConnections.Dequeue());
        }
    }

    private Texture2D GenerateQR(string text) {
        Texture2D encoded = new Texture2D(256,256);
        encoded.filterMode = FilterMode.Point;
        Color32[] color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    private static Color32[] Encode(string textForEncoding, int width, int height) {
        var writer = new BarcodeWriter {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public List<TableRealmsClientConnection> GetAllConnections() {
        return connections;
    }

    private bool AddPendingClient(IClientConnection clientConnection) {
        pendingClientConnections.Enqueue(clientConnection);
        return true;
    }

    public bool IsClientConnected(string name) {
        return connectionIds.Contains(name);
    }

    public void AddClientId(string name) {
        if (!connectionIds.Contains(name)) {
            connectionIds.Add(name);
        }
    }
    
    private void AddClient(IClientConnection clientConnection) {

        GameObject newClientGameObject = new GameObject();
        newClientGameObject.transform.parent = gameObject.transform;
        TableRealmsClientConnection tableRealmsClientConnection = newClientGameObject.AddComponent<TableRealmsClientConnection>() as TableRealmsClientConnection;
        tableRealmsClientConnection.SetClientConnection(clientConnection);
        connections.Add(tableRealmsClientConnection);
        newClientGameObject.name = "Connecting";// tableRealmsClientConnection.id;

        tableRealmsClientConnection.SetPlayerCommandScript(playerCommandScript);

        // Send client global data.
        HashSet<string> globalKeys=TableRealmsModel.instance.GetGlobalDataKeys();
        foreach (string key in globalKeys) {
            UpdateModelMessage updateModelMessage = null;

            Type valueType = TableRealmsModel.instance.GetDataType(key);
            if (valueType == typeof(string)) {
                updateModelMessage = new UpdateModelMessage(true,key,ModelType.TypeString.ToString(), TableRealmsModel.instance.GetData<string>(key));
            } else if (valueType == typeof(long)) {
                updateModelMessage = new UpdateModelMessage(true, key, ModelType.TypeDouble.ToString(), TableRealmsModel.instance.GetData<long>(key).ToString());
            } else if (valueType == typeof(float)) {
                updateModelMessage = new UpdateModelMessage(true, key, ModelType.TypeDouble.ToString(), TableRealmsModel.instance.GetData<float>(key).ToString());
            } else if (valueType == typeof(double)) {
                updateModelMessage = new UpdateModelMessage(true, key, ModelType.TypeDouble.ToString(), TableRealmsModel.instance.GetData<double>(key).ToString());
            } else if (valueType == typeof(bool)) {
                updateModelMessage = new UpdateModelMessage(true, key, ModelType.TypeBoolean.ToString(), TableRealmsModel.instance.GetData<bool>(key).ToString());
            } else {
                Debug.LogError("TableRealms: Global data of type '"+ valueType + "' is not suported, only string, bool, long, and float are.");
            }
            if (updateModelMessage != null) {
                //Debug.LogError("Sending global "+key+"="+updateModelMessage.value);
                tableRealmsClientConnection.SendClientMessage(updateModelMessage);
            //}else{
                //Debug.LogError("Not Sending global "+key+" of type "+valueType.Name);                
            }
        }
    }

    public void RemoveConnection(TableRealmsClientConnection tableRealmsClientConnection) {
        connections.Remove(tableRealmsClientConnection);
        if (connectionIds.Contains(tableRealmsClientConnection.gameObject.name)) {
            connectionIds.Remove(tableRealmsClientConnection.gameObject.name);
        }
    }

    private void InitServer() {
        if (clientConnectionListener == null) {
#if (!UNITY_WEBGL || UNITY_EDITOR)
            clientConnectionListener = gameObject.AddComponent<ClientConnectionListenerSocket>();
            clientConnectionListener.Initialize(gameName);
            clientConnectionListener.AcceptClientConnections(AddPendingClient);
#else
            clientConnectionListener = gameObject.AddComponent<ClientConnectionListenerWebSocket>();
            clientConnectionListener.Initialize(gameName);
            clientConnectionListener.AcceptClientConnections(AddPendingClient);
#endif
        }
    }

    /*
    private void InjectResorucesIntoWebServer() {
        EmbeddedWebServerComponent embeddedWebServerComponent = GetComponent<EmbeddedWebServerComponent>();

        Dictionary<string, string> mimeTypes = new Dictionary<string, string>();
        mimeTypes.Add(".gif", "image/gif");
        mimeTypes.Add(".jpeg", "image/jpeg");
        mimeTypes.Add(".jpg", "image/jpeg");
        mimeTypes.Add(".png", "image/png");
        mimeTypes.Add(".json", "application/json");
        mimeTypes.Add(".otf", "font/otf");
        mimeTypes.Add(".ttf", "font/ttf");
        mimeTypes.Add(".wav", "audio/wav");
        mimeTypes.Add(".lua", "text/plain");
        HashSet<string> binaryTypes = new HashSet<string>();
        binaryTypes.Add("image/gif");
        binaryTypes.Add("image/jpeg");
        binaryTypes.Add("image/png");
        binaryTypes.Add("font/otf");
        binaryTypes.Add("font/ttf");
        binaryTypes.Add("audio/wav");


        if (embeddedWebServerComponent != null) {
            byte[] data = new byte[2048];
            using (ZipInputStream s = new ZipInputStream(new MemoryStream(tableRealmsDesign.bytes))) {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null) {

                    Console.WriteLine(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // File.Create(theEntry.Name)
                    if (fileName != String.Empty) {
                        MemoryStream streamWriter = new MemoryStream();
                        while (true) {
                            int size = s.Read(data, 0, data.Length);
                            if (size > 0) {
                                streamWriter.Write(data, 0, size);
                            } else {
                                break;
                            }
                        }
                        streamWriter.Close();

                        string mimeType = "text/plain";
                        if (fileName.LastIndexOf(".") != -1) { 
                            string extension = fileName.Substring(fileName.LastIndexOf("."));
                            if (mimeTypes.ContainsKey(extension)) {
                                mimeType = mimeTypes[extension];
                            } else {
                                Debug.LogError("No mime type extension for extension '" + extension + "' setting default mime type.");
                                mimeTypes.Add(extension, "text/plain");
                            }
                        } else {
                            Debug.LogError("No file extension for '"+ fileName + "' setting default mime type.");
                        }                    
                        embeddedWebServerComponent.AddResource(directoryName + "/" + fileName,new TableRealmsHTTPResponse(mimeType,streamWriter.ToArray(), binaryTypes.Contains(mimeType)));
                    }
                }
            }
        } else {
            Debug.LogError("TableRealms: Can not find <EmbeddedWebServerComponent> on table realms object. HTTP Server not started.");
        }

    }*/

    public void SetAllPlayersToPage(string page)
    {
        foreach (TableRealmsClientConnection connection in connections){
            TableRealmsModel.instance.SetData(connection.gameObject.name+".page", page);
        }
    }

    public void SendData(string key, object value) {
        if (value!=null){
            String firstPart = key.IndexOf(".")==-1?null:key.Substring(0, key.IndexOf("."));

            String type = ModelType.TypeString.ToString();
            String valueString = value.ToString();
            if (value is string) {
                type = ModelType.TypeString.ToString();
            } else if (value is long || value is int) {
                type = ModelType.TypeDouble.ToString();
            } else if (value is float) {
                type = ModelType.TypeDouble.ToString();
                valueString = ((float)value).ToString(cultureInfo);
            } else if (value is double) {
                type = ModelType.TypeDouble.ToString();
                valueString = ((double)value).ToString(cultureInfo);
            } else if (value is bool) {
                type = ModelType.TypeBoolean.ToString();
            }

            UpdateModelMessage updateModelMessage = null;
            // Test if this is for a client
            if (firstPart != null) {
                foreach (TableRealmsClientConnection connection in connections) {
                    if (connection.id == firstPart) {
                        updateModelMessage = new UpdateModelMessage(false, key.Substring(firstPart.Length + 1), type, valueString);
                        connection.SendClientMessage(updateModelMessage);
                    }
                }
            }
            // Send to all
            if (updateModelMessage == null) {
                updateModelMessage = new UpdateModelMessage(false, key, type, value.ToString());
                foreach (TableRealmsClientConnection connection in connections) {
                    connection.SendClientMessage(updateModelMessage);
                }
            }
        }
    }
}
