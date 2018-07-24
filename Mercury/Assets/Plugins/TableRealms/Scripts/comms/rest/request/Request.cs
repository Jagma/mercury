using System;

namespace tablerealms.comms.rest.request
{

	[Serializable]
	public class Request
	{
		private const long serialVersionUID = 1L;

		public string sessionId;
		public long? requestId;

		public virtual string getSessionId()
		{
			return sessionId;
		}

		public virtual void setSessionId(string sessionId)
		{
			this.sessionId = sessionId;
		}

		public virtual long? getRequestId()
		{
			return requestId;
		}

		public virtual void setRequestId(long? requestId)
		{
			this.requestId = requestId;
		}

	}

}