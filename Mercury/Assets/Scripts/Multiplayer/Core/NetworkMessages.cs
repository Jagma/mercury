using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMessages {

    // Data structures
    public class LobbyListing {
        public string uniqueID = "-1";
        public string name = "noname";
    }
    public class LobbyDetails {
        public string uniqueID = "-1";
        public string name = "noname";
        public List<string> clientList = new List<string>();
    }
    
    public class MessageBase {
    }

    // Server connection
    public class ConnectionEstablished : MessageBase {
    }

    // Hearbeat
    public class RequestHeartbeat : MessageBase {
    }
    public class Heartbeat : MessageBase {
    }

    // Server details
    public class RequestServerDetails : MessageBase {
    }
    public class ReturnServerDetails : MessageBase {
        public List<LobbyListing> lobbyList = new List<LobbyListing>();
    }

    // Lobbies
    public class RequestLobbyDetails : MessageBase {
        public string lobbyUniqueID;
    }
    public class ReturnLobbyDetails : MessageBase {
        public LobbyDetails data;
    }


    public class HostLobby : MessageBase {
        public LobbyDetails data;
    }
    public class LobbyHosted : MessageBase {
        public string lobbyUniqueID;
    }


    public class JoinLobby : MessageBase {
        public string lobbyUniqueID;
    }
    public class LobbyJoined : MessageBase {
        public string lobbyUniqueID;
    }


    public class LeaveLobby : MessageBase {
        public string lobbyUniqueID;
    }
    public class LobbyLeft : MessageBase {
        public string lobbyUniqueID;
    }


    public class ExecuteLobby : MessageBase {
    }
    public class LobbyExecuted : MessageBase {
        public string lobbyUniqueID;
    }



    // Game

    // TODO: Game left message
    public class GameStarted : MessageBase {
        public string thisClientID = "-1";
        public bool isHost = false;
        public string[] clientUniqueIDs;
    }

    public class ObjectCreated : MessageBase {
        public string gameUniqueID = "";
        public Factory.ObjectConstructor objectConstructor;
    }

    public class ObjectDestroyed : MessageBase {
        public string gameUniqueID = "";
        public string type = "";
        public string uniqueID = "";
    }

    public class ModelUpdated : MessageBase {
        public string gameUniqueID = "";
        public string uniqueID = "-1";
        public Dictionary<string, object> model;
    }

    public class StartGame : MessageBase {
        public string gameUniqueID = "";
    }

    public class CreateObject : MessageBase {
        public string gameUniqueID = "";
        public Factory.ObjectConstructor objectConstructor;
    }

    public class DestroyObject : MessageBase {
        public string gameUniqueID = "";
        public string type = "";
        public string uniqueID = "";
    }

    public class UpdateModel : MessageBase {
        public string gameUniqueID = "";
        public string uniqueID = "-1";
        public Dictionary<string, object> model;
    }

    
}


// Aditional helper classes
public class NetworkVector3 {
    public float x = 0;
    public float y = 0;
    public float z = 0;

    public NetworkVector3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 ToVector3() {
        return new Vector3(x, y, z);
    }

    public static NetworkVector3 FromVector3(Vector3 vec3) {
        return new NetworkVector3(vec3.x, vec3.y, vec3.z);
    }

    public override bool Equals(object obj) {
        if (obj.GetType() == typeof(NetworkVector3)) {
            NetworkVector3 v3 = (NetworkVector3)obj;
            if (this.x == v3.x &&
                this.y == v3.y &&
                this.z == v3.z) {
                return true;
            }
        }


        return false;
    }
}