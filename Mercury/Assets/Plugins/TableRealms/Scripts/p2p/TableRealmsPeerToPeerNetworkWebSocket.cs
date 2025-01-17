﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using tablerealms.comms.message;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
#endif

public class TableRealmsPeerToPeerNetworkWebSocket : TableRealmsPeerToPeerNetwork {
#if UNITY_WEBGL && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern string TableRealmsGetProtocol();

    [DllImport("__Internal")]
    private static extern string TableRealmsAmIHost(string gameObjectName);
#endif

    private string sessionId;
    private bool calledHost=false;

    override public void Start() {
        if (instance == null) {
#if UNITY_WEBGL && !UNITY_EDITOR
            string protocol = TableRealmsGetProtocol();

            // Are we running embedded
            if (protocol != null && protocol.StartsWith("Embedded")) {
                TableRealmsGameNetwork.embeded = true;
            }

            // Are we running in Peer2Peer mode? If not then remove flag us as the host
            if ("EmbeddedP2P".Equals(protocol)) {
                TableRealmsGameNetwork.p2p=true;
                Debug.Log("TableRealms: P2P Enabled.");
            } else {
                TableRealmsGameNetwork.p2p=false;
                TableRealmsGameNetwork.host=true;
                Debug.Log("TableRealms: P2P Not Enabled. Protocol '"+ TableRealmsGetProtocol() + "'");
            }
#else
            TableRealmsGameNetwork.p2p = false;
            TableRealmsGameNetwork.host = true;
#endif
        }
        base.Start();
    }

    public override void Update() {
        if (!calledHost) {
            calledHost = true;
#if UNITY_WEBGL && !UNITY_EDITOR
            TableRealmsAmIHost(gameObject.name);
#endif
        } else {
            base.Update();
        }
    }

    override public string GetSessionId() {
        return sessionId;
    }

    public void YouAreTheHost() {
        iAmTheHost = true;
        TableRealmsGameNetwork.host = true;
    }

    public void SetHostSessionId(string sessionId) {
        this.sessionId = sessionId;
    }

    public override void DeliverMessageToClients(P2PMessage message) {
        if (TableRealmsGameNetwork.host && TableRealmsGameNetwork.p2p) {
            InitTables();

            TableRealmsGameNetwork.instance.SendAllClientsP2PMessage(GetMessageName(message) + "|" + JsonUtility.ToJson(message, false) + "\n");
        }
    }
}
