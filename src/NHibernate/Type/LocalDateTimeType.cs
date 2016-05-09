using System;

namespace NHibernate.Type
{
	[Serializable]
	public partial class LocalDateTimeType : AbstractDateTimeSpecificKindType
	{
		protected override DateTimeKind DateTimeKind
		{
			get { return DateTimeKind.Local; }
		}

		public override string Name
		{
			get { return "LocalDateTime"; }
		}
	}
}