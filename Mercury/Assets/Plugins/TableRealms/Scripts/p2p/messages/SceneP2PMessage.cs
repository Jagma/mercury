using System;

namespace tablerealms.comms.message
{
	[Serializable]
	public class SceneP2PMessage : P2PMessage
	{
		private const long serialVersionUID = 1L;

		public string name;

		public SceneP2PMessage() : base()
		{
		}

		public SceneP2PMessage(string name) : base()
		{
			this.name = name;
		}

	}

}