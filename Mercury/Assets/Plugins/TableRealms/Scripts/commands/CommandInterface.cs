using UnityEngine;
using System.Collections;

namespace tablerealms.comms.message {

    public interface CommandInterface {
        void ProcessMessage(Message message, TableRealmsClientConnection tableRealmsClientConnection);
    }

}