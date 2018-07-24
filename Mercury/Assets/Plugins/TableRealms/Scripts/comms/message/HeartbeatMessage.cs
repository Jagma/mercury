using System;

namespace tablerealms.comms.message
{
	[Serializable]
	public class HeartbeatMessage : Message
	{
		private const long serialVersionUID = 1L;

		public long tick;

		public HeartbeatMessage() : base()
		{
		}

		public HeartbeatMessage(long tick) : base()
		{
			this.tick = tick;
		}

		public virtual long getTick()
		{
			return tick;
		}

		public virtual void setTick(long tick)
		{
			this.tick = tick;
		}

	}

}