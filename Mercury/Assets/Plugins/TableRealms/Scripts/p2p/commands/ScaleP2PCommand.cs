using UnityEngine.SceneManagement;

namespace tablerealms.comms.message
{

    public class ScaleP2PCommand : P2PCommand<ScP2PM>
    {
        protected override void Process(ScP2PM message)
        {
            TableRealmsPeerToPeerNetwork.instance.P2PScale(message);
        }
    }
}

