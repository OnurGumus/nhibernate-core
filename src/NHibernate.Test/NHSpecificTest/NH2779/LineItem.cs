﻿
namespace NHibernate.Test.NHSpecificTest.NH2779
{
	public partial class LineItem
	{
		public virtual int LineItemId { get; set; }
		public virtual int InternalOrderId { get; set; }
		public virtual int SortOrder { get; set; }
	}
}
