using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tablerealms.comms.message;

public class TableRealmsPlayerId : MonoBehaviour {

    private static long nextguid = 0;

    private string _playerId = null;
    private long _guid = nextguid++;
    private System.Random _random=null;

    public System.Random random{
        get {
            if (_random == null) {
                _random = new System.Random((int)_guid);
            }
            return _random;
        }
    }

    public long guid
    {
        get
        {
            return _guid;
        }
    }

    public string player
    {
        get
        {
            return _playerId;
        }
        set 
        {
            if (_playerId == null) {
                _playerId = value;
            }
        }
    }

    public void Start() {
    }

    public void SetClientData(IP2PM instantiateP2PMessage) {
        _guid = instantiateP2PMessage.g;
        player = instantiateP2PMessage.p;
        _random = new System.Random((int)_guid);
    }

    public void SetPage(string page) {
        TableRealmsModel.instance.SetData(_playerId + ".page", page);
    }

    public oftype GetData<oftype>(string key) {
        return TableRealmsModel.instance.GetData<oftype>(_playerId + "." + key);
    }

    public void SetData(string key,object value) {
        TableRealmsModel.instance.SetData(_playerId + "." + key,value);
    }

    private void OnDestroy() {
        if (TableRealmsGameNetwork.host) {
            TableRealmsPeerToPeerNetwork.instance.RemoveNetworkedGameObject(guid);
        }
    }

}
