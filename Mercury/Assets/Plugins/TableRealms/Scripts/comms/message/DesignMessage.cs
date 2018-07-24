using System;

namespace tablerealms.comms.message
{
	[Serializable]
	public class DesignMessage : Message
	{
		private const long serialVersionUID = 1L;

		public int startPos;
		public int dataLength;
		public int totalLength;
		public sbyte[] data;

		public DesignMessage() : base()
		{
		}

		public virtual long getStartPos()
		{
			return startPos;
		}

		public virtual void setStartPos(int startPos)
		{
			this.startPos = startPos;
		}

		public virtual int getTotalLength()
		{
			return totalLength;
		}

		public virtual void setTotalLength(int totalLength)
		{
			this.totalLength = totalLength;
		}

		public virtual int getDataLength()
		{
			return dataLength;
		}

		public virtual void setDataLength(int dataLength)
		{
			this.dataLength = dataLength;
		}

		public virtual sbyte[] getData()
		{
			return data;
		}

		public virtual void setData(sbyte[] data)
		{
			this.data = data;
		}

	}

}