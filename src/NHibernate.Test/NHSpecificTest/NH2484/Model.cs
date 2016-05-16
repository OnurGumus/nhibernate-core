using System;
using System.Runtime.Serialization;

namespace NHibernate.Test.NHSpecificTest.NH2484
{
	public partial class ClassWithImage
	{
		public virtual int Id { get; set; }
		public virtual System.Drawing.Image Image { get; set; }
	}
	
	public partial class ClassWithSerializableType
	{
		public virtual int Id { get; set; }
		public virtual ISerializable Image { get; set; }
	}
}