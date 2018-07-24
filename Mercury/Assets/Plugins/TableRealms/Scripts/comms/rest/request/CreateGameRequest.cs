using System;

namespace tablerealms.comms.rest.request
{
	[Serializable]
	public class CreateGameRequest : Request
	{
		private const long serialVersionUID = 1L;

		public string serverId;

		public CreateGameRequest(string serverId) : base()
		{
			this.serverId = serverId;
		}

		public virtual string getServerId()
		{
			return serverId;
		}

		public virtual void setServerId(string serverId)
		{
			this.serverId = serverId;
		}

	}

}