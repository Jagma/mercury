using UnityEngine;
using System;

namespace tablerealms.comms.message {

    public class DesignCommand : Command<DesignMessage> {
        const int DATA_PACK_SIZE = 64 * 1024;

        static byte[] designBytes = null;
        static string hashString = "";

        protected override void Process(DesignMessage message, TableRealmsClientConnection tableRealmsClientConnection) {
            if (designBytes == null) {
                designBytes = TableRealmsGameNetwork.instance.designBytes;

                System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hashBytes = md5.ComputeHash(designBytes);

                for (int i = 0; i < hashBytes.Length; i++) {
                    hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
                }
            }
            string designMd5 = tableRealmsClientConnection.GetModelData<string>("designMd5");
            if (message.startPos + message.dataLength == 0 && designMd5 != null && designMd5 == hashString) {
                DesignMessage designMessage = new DesignMessage();
                designMessage.startPos = 0;
                designMessage.dataLength = designBytes.Length;
                designMessage.totalLength = designBytes.Length;

                tableRealmsClientConnection.SendClientMessage(designMessage);
                tableRealmsClientConnection.SetModelData("loaded", 1f);
            } else {
                if (message.startPos + message.dataLength < designBytes.Length) {
                    tableRealmsClientConnection.SetModelData("designMd5", hashString);

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

}