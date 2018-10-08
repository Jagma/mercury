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
}
