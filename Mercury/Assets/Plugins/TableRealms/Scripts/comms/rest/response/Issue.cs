using System.Collections.Generic;

namespace tablerealms.comms.rest.response
{
	public sealed class Issue
	{
		public static readonly Issue None = new Issue("None", InnerEnum.None, 0,null);

		// Login issues
		public static readonly Issue AccountUnknown = new Issue("AccountUnknown", InnerEnum.AccountUnknown, 1000,"account.unknown");

		private static readonly IList<Issue> valueList = new List<Issue>();

		static Issue()
		{
			valueList.Add(None);
			valueList.Add(AccountUnknown);
		}

		public enum InnerEnum
		{
			None,
			AccountUnknown
		}

		public readonly InnerEnum innerEnumValue;
		private readonly string nameValue;
		private readonly int ordinalValue;
		private static int nextOrdinal = 0;

		private long id;
		private string description;

		private Issue(string name, InnerEnum innerEnum, long id, string description)
		{
			this.id = id;
			this.description = description;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public long getId()
		{
			return id;
		}

		public void setId(long id)
		{
			this.id = id;
		}

		public string getDescription()
		{
			return description;
		}

		public void setDescription(string description)
		{
			this.description = description;
		}


		public static IList<Issue> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static Issue valueOf(string name)
		{
			foreach (Issue enumInstance in Issue.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}