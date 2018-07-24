using System;

namespace tablerealms.comms.message
{
	[Serializable]
	public class CommandMessage : Message
	{
		private const long serialVersionUID = 1L;

		public string command;
		public string[] @params;

		public CommandMessage(string command) : base()
		{
			this.command = command;
		}

		public CommandMessage(string command, string[] @params) : base()
		{
			this.command = command;
			this.@params = @params;
		}

		public virtual string getCommand()
		{
			return command;
		}

		public virtual void setCommand(string command)
		{
			this.command = command;
		}

		public virtual string[] getParams()
		{
			return @params;
		}

		public virtual void setParams(string[] @params)
		{
			this.@params = @params;
		}

	}

}