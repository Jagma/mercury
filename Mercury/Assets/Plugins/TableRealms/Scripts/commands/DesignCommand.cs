using UnityEngine;
using System;

namespace tablerealms.comms.message {

    public class DesignCommand : Command<DesignMessage> {
        const int DATA_PACK_SIZE = 64 * 1024;


        static byte[] designBytes = null;

        protected override void Process(DesignMessage message, TableRealmsClientConnection tableRealmsClientConnection) {
            if (designBytes == null) {
                designBytes = TableRealmsGameNetwork.instance.designBytes;
            }
            if (message.startPos + message.dataLength < designBytes.Length) {

                DesignMessage designMessage = new DesignMessage();
                designMessage.startPos = message.startPos + message.dataLength;
                designMessage.dataLength = Mathf.Min(DATA_PACK_SIZE, (int)(designBytes.Length - designMessage.startPos));
                designMessage.totalLength = designBytes.Length;
                designMessage.data = new sbyte[designMessage.dataLength];
                Buffer.BlockCopy(designBytes, designMessage.startPos, designMessage.data, 0, designMessage.dataLength);

                tableRealmsClientConnection.SendClientMessage(designMessage);
                tableRealmsClientConnection.SetModelData("loaded", (float)(designMessage.startPos + designMessage.dataLength) / (float)designBytes.Length);
            } else {
                tableRealmsClientConnection.SetModelData("loaded", 1f);
            }
        }
    }

}