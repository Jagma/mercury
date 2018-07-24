using UnityEngine;
using System;
using System.Collections;
using System.Globalization;

namespace tablerealms.comms.message {
    
    public class UpdateModelCommand : Command<UpdateModelMessage> {

        private static CultureInfo cultureInfo = new CultureInfo("en-US");

        protected override void Process(UpdateModelMessage message, TableRealmsClientConnection tableRealmsClientConnection) {
            if (message.isGlobal()) {
                TableRealmsModel.instance.AddGlobalKey(message.name);
                TableRealmsModel.instance.SetData(message.name,GetFieldValue(message));
            } else {
                if ("id" == message.name) {
                    tableRealmsClientConnection.gameObject.name = message.value;
                    tableRealmsClientConnection.id = message.value;
                    TableRealmsGameNetwork.instance.AddClientId(message.value);
                } else {
                    TableRealmsModel.instance.SetDataLocalOnly(tableRealmsClientConnection.id + "." + message.name, GetFieldValue(message));
                    if ("state" == message.name) {
                        tableRealmsClientConnection.SetState((TableRealmsClientConnection.State)Enum.Parse(typeof(TableRealmsClientConnection.State), message.getValue()));
                    }
                }
            }
        }

        private object GetFieldValue(UpdateModelMessage message) {
            object value = null;
            ModelType modelType = (ModelType)Enum.Parse(typeof(ModelType), message.getType());
            switch (modelType) {
                case ModelType.TypeBoolean:
                    value = bool.Parse(message.value);
                    break;
                case ModelType.TypeString:
                    value = message.value;
                    break;
                case ModelType.TypeDouble:
                    value = double.Parse(message.value, cultureInfo);
                    break;
            }
            return value;
        }
    }

}