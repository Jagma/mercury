using UnityEngine;
using System.Collections;

namespace tablerealms.comms.message {

    public class HeartbeatCommand : Command<HeartbeatMessage> {

        protected override void Process(HeartbeatMessage message, TableRealmsClientConnection tableRealmsClientConnection) {
            tableRealmsClientConnection.SetHeartbeat(message);
        }
    }

}