using System;
using UnityEngine;

namespace tablerealms.comms.message
{
	[Serializable]
	public class MP2PM : P2PMessage
	{
        public long g;
        public float t;
        public Vector3 p;
        public Vector3 r;
        public Vector3 v;
        public Vector3 rv;

        public MP2PM() : base()
		{
		}

		public MP2PM(long guid,float time,Vector3 position, Vector3 rotation, Vector3 velocity, Vector3 rotationalVelocity) : base()
		{
            this.g = guid;
            this.t = time;
            this.p = position;
            this.r = rotation;
            this.v = velocity;
            this.rv = rotationalVelocity;
        }

    }

}