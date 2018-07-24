using System;

namespace tablerealms.comms.message
{
	[Serializable]
	public class UpdateModelMessage : Message
	{
		private const long serialVersionUID = 1L;

		public bool global = false;
		public string name;
		public string type;
		public string value;

		public UpdateModelMessage() : base()
		{
		}

		public UpdateModelMessage(bool global, string name, string type, string value) : base()
		{
			this.global = global;
			this.name = name;
			this.type = type;
			this.value = value;
		}

		public virtual bool isGlobal()
		{
			return global;
		}

		public virtual void setGlobal(bool global)
		{
			this.global = global;
		}

		public virtual string getName()
		{
			return name;
		}

		public virtual void setName(string name)
		{
			this.name = name;
		}

		public virtual string getType()
		{
			return type;
		}

		public virtual void setType(string type)
		{
			this.type = type;
		}

		public virtual string getValue()
		{
			return value;
		}

		public virtual void setValue(string value)
		{
			this.value = value;
		}

	}

}