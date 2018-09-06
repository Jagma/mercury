using UnityEngine.SceneManagement;

namespace tablerealms.comms.message
{

    public class SceneP2PCommand : P2PCommand<SceneP2PMessage>
    {
        protected override void Process(SceneP2PMessage message)
        {
            SceneManager.LoadScene(message.name);
        }
    }
}

