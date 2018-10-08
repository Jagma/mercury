using System;

namespace tablerealms.comms.message
{
	[Serializable]
	public class SP2PM : P2PMessage
	{
		private const long serialVersionUID = 1L;

		public string n;
        public bool m = false;

        public SP2PM() : base()
		{
		}

        public SP2PM(string name, bool merge) : base() {
            this.n = name;
            this.m = merge;
        }

    }

}