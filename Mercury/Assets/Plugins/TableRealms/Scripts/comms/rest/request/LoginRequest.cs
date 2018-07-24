using System;

namespace tablerealms.comms.rest.request
{
	[Serializable]
	public class LoginRequest : Request
	{
		private const long serialVersionUID = 1L;

		public string account;
		public string secret;

		public LoginRequest(string account, string secret) : base()
		{
			this.account = account;
			this.secret = secret;
		}

		public virtual string getAccount()
		{
			return account;
		}

		public virtual void setAccount(string account)
		{
			this.account = account;
		}

		public virtual string getSecret()
		{
			return secret;
		}

		public virtual void setSecret(string secret)
		{
			this.secret = secret;
		}
	}

}