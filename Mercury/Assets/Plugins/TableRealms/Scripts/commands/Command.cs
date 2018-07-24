using UnityEngine;
using System.Collections;

namespace tablerealms.comms.message {

    abstract public class Command<type> : CommandInterface where type : Message {
        public void ProcessMessage(Message message, TableRealmsClientConnection tableRealmsClientConnection) {
            Process((type)message, tableRealmsClientConnection);
        }

        protected abstract void Process(type message, TableRealmsClientConnection tableRealmsClientConnection);
    }

}