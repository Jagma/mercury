using System;

namespace tablerealms.comms.rest.response
{
	[Serializable]
	public class LoginResponse : Response
	{
		private const long serialVersionUID = 1L;

		public string sessionId;

		protected internal LoginResponse(string sessionId) : base()
		{
			this.sessionId = sessionId;
		}

		public virtual string getSessionId()
		{
			return sessionId;
		}

		public virtual void setSessionId(string sessionId)
		{
			this.sessionId = sessionId;
		}

	}

}