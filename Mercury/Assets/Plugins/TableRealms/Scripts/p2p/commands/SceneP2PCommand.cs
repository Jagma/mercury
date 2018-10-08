using UnityEngine.SceneManagement;

namespace tablerealms.comms.message
{

    public class SceneP2PCommand : P2PCommand<SP2PM>
    {
        protected override void Process(SP2PM message)
        {
            TableRealmsPeerToPeerNetwork.instance.LoadScene(message.n);
        }
    }
}

