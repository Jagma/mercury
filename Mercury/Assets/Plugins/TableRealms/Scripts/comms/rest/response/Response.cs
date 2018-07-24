using System;

namespace tablerealms.comms.rest.response
{

	[Serializable]
	public class Response
	{
		private const long serialVersionUID = 1L;

		public bool ok = true;
		public Issue issue = Issue.None;

		protected internal Response()
		{
		}

		public Response(Issue issue)
		{
			this.issue = issue;
			ok = issue == Issue.None;
		}

		public virtual bool isOk()
		{
			return ok;
		}

		public virtual void setOk(bool ok)
		{
			this.ok = ok;
		}

		public virtual Issue getIssue()
		{
			return issue;
		}

		public virtual void setIssue(Issue issue)
		{
			this.issue = issue;
		}


	}

}