using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRealmsP2PMonoBehavior : MonoBehaviour {

    private TableRealmsPlayerId _tableRealmsPlayerId;

    public TableRealmsPlayerId tableRealmsPlayerId
    {
        get
        {
            if (_tableRealmsPlayerId == null) {
                _tableRealmsPlayerId = GetComponent<TableRealmsPlayerId>();
            }
            return _tableRealmsPlayerId;
        }
    }

    public string playerId
    {
        get
        {
            return tableRealmsPlayerId.player;
        }
    }

    public long guid
    {
        get
        {
            return tableRealmsPlayerId.guid;
        }
    }

    public void P2PSendMessage(string message) {
        TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(guid, message);
    }

    public void P2PSendMessage(string message, string param) {
        TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(guid, message, param);
    }

    public void P2PSendMessage(string message, long param) {
        TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(guid, message, param);
    }

    public void P2PSendMessage(string message, int param) {
        TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(guid, message, param);
    }

    public void P2PSendMessage(string message, double param) {
        TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(guid, message, param);
    }

    public void P2PSendMessage(string message, float param) {
        TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(guid, message, param);
    }

    public void P2PSendMessage(string message, Vector3 param) {
        TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(guid, message, param);
    }

    public void P2PSendMessage(string message, Vector2 param) {
        TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(guid, message, param);
    }
}
