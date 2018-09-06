using UnityEngine;
using System.Collections;

namespace tablerealms.comms.message {

    abstract public class P2PCommand<type> : P2PCommandInterface where type : P2PMessage {
        public void ProcessMessage(P2PMessage message) {
            Process((type)message);
        }

        protected abstract void Process(type message);
    }

}