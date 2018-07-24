using System;

namespace tablerealms.comms.rest.response
{
	[Serializable]
	public class CreateGameResponse : Response
	{
		private const long serialVersionUID = 1L;

		public string gameId;

		protected internal CreateGameResponse(string gameId) : base()
		{
			this.gameId = gameId;
		}

		public virtual string getGameId()
		{
			return gameId;
		}

		public virtual void setGameId(string gameId)
		{
			this.gameId = gameId;
		}

	}

}