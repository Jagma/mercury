using System;
using UnityEngine;

namespace tablerealms.comms.message
{
	[Serializable]
	public class IP2PM : P2PMessage
	{
		private const long serialVersionUID = 1L;

        public string n;
        public string p;
        public long g;
        public Vector3 l;
        public Vector3 o;
        public bool m;

        public IP2PM() : base()
		{
		}

		public IP2PM(string player, string name, long guid, Vector3 position, Vector3 orientation,bool networkMovement) : base()
		{
            this.p = player;
            this.n = name;
            this.g = guid;
            this.l = position;
            this.o = orientation;
            this.m = networkMovement;
        }

    }

}