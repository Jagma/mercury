using UnityEngine.SceneManagement;

namespace tablerealms.comms.message
{

    public class DestroyP2PCommand : P2PCommand<DP2PM>
    {
        protected override void Process(DP2PM message)
        {
            TableRealmsPeerToPeerNetwork.instance.RemoveNetworkedGameObject(message.g);
        }
    }
}

