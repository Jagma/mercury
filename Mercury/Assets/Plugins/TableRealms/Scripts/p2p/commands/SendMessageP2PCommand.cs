using UnityEngine;
using UnityEngine.SceneManagement;

namespace tablerealms.comms.message
{

    public class SendMessageP2PCommand : P2PCommand<SMP2P>
    {
        protected override void Process(SMP2P message)
        {
            switch (message.t) {
                case SMP2P.ParamType.N:
                    TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(message.g, message.m);
                    break;
                case SMP2P.ParamType.S:
                    TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(message.g, message.m, message.s);
                    break;
                case SMP2P.ParamType.I:
                    TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(message.g, message.m, message.i);
                    break;
                case SMP2P.ParamType.L:
                    TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(message.g, message.m, message.l);
                    break;
                case SMP2P.ParamType.F:
                    TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(message.g, message.m, message.f);
                    break;
                case SMP2P.ParamType.D:
                    TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(message.g, message.m, message.d);
                    break;
                case SMP2P.ParamType.V2:
                    TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(message.g, message.m, (Vector2)message.v);
                    break;
                case SMP2P.ParamType.V3:
                    TableRealmsPeerToPeerNetwork.instance.P2PSendMessage(message.g, message.m, message.v);
                    break;
            }
        }
    }
}

