using System;

namespace NHibernate.Test.EntityModeTest.Xml.Many2One
{
	[Serializable]
	public partial class CarPart
	{
		public virtual long Id { get; set; }
		public virtual string PartName { get; set; }
	}
}