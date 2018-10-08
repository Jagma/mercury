using System;

namespace tablerealms.comms.message
{
	[Serializable]
	public class DP2PM : P2PMessage
	{
		private const long serialVersionUID = 1L;

		public long g;

		public DP2PM() : base()
		{
		}

		public DP2PM(long guid) : base()
		{
			this.g = guid;
		}

	}

}