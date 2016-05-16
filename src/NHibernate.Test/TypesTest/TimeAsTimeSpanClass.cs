using System;

namespace NHibernate.Test.TypesTest
{
	public partial class TimeAsTimeSpanClass
	{
		public int Id { get; set; }
		public TimeSpan TimeSpanValue { get; set; }
	}
}