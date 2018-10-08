using UnityEngine.SceneManagement;

namespace tablerealms.comms.message
{

    public class InstantiateP2PCommand : P2PCommand<IP2PM>
    {
        protected override void Process(IP2PM message)
        {
            TableRealmsPeerToPeerNetwork.instance.P2PInstantiateClient(message);
        }
    }
}

