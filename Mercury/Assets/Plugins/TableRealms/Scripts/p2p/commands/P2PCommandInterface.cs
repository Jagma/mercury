using UnityEngine;
using System.Collections;

namespace tablerealms.comms.message {

    public interface P2PCommandInterface {
        void ProcessMessage(P2PMessage message);
    }

}