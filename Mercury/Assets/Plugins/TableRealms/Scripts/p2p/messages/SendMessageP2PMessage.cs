using System;
using UnityEngine;

namespace tablerealms.comms.message
{
	[Serializable]
	public class SMP2P : P2PMessage
	{
        public enum ParamType {
            N,
            S,
            I,
            L,
            F,
            D,
            V2,
            V3
        }

        public long g;
        public string m;
        public string s;
        public int i;
        public long l;
        public double d;
        public float f;
        public Vector3 v;
        public ParamType t = ParamType.N;

        public SMP2P() : base()
		{
		}

        public SMP2P(long guid, string message) : base() {
            this.g = guid;
            this.m = message;
        }

        public SMP2P(long guid, string message, string param) : base() {
            this.g = guid;
            this.m = message;
            this.s = param;
            t = ParamType.S;
        }

        public SMP2P(long guid, string message, int param) : base() {
            this.g = guid;
            this.m = message;
            this.i = param;
            t = ParamType.I;
        }

        public SMP2P(long guid, string message, long param) : base() {
            this.g = guid;
            this.m = message;
            this.l = param;
            t = ParamType.L;
        }

        public SMP2P(long guid, string message, float param) : base() {
            this.g = guid;
            this.m = message;
            this.f = param;
            t = ParamType.F;
        }

        public SMP2P(long guid, string message, double param) : base() {
            this.g = guid;
            this.m = message;
            this.d = param;
            t = ParamType.D;
        }

        public SMP2P(long guid, string message, Vector2 param) : base() {
            this.g = guid;
            this.m = message;
            this.v = param;
            t = ParamType.V2;
        }

        public SMP2P(long guid, string message, Vector3 param) : base() {
            this.g = guid;
            this.m = message;
            this.v = param;
            t = ParamType.V3;
        }

    }

}