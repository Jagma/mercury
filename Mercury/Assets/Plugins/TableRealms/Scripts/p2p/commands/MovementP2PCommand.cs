using UnityEngine.SceneManagement;

namespace tablerealms.comms.message
{

    public class MovementP2PCommand : P2PCommand<MP2PM>
    {
        protected override void Process(MP2PM message)
        {
            TableRealmsPeerToPeerNetwork.instance.P2PMovement(message);
        }
    }
}

