using System;
using UnityEngine;

namespace tablerealms.comms.message
{
	[Serializable]
	public class ScP2PM : P2PMessage
	{
        public long g;
        public float t;
        public Vector3 s;

        public ScP2PM() : base()
		{
		}

		public ScP2PM(long guid,float time,Vector3 scale) : base()
		{
            this.g = guid;
            this.t = time;
            this.s = scale;
        }

    }

}