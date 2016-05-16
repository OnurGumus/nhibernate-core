using System.Collections.Generic;

namespace NHibernate.Test.GenericTest.Overall
{
	/// <summary>
	/// This class is used in <see cref="Fixture" /> with
	/// <c>int</c> substituted for <typeparamref name="T" />.
	/// </summary>
	public partial class A<T>
	{
		public virtual int? Id { get; set; }

		public virtual T Property { get; set; }

		public virtual IList<T> Collection { get; set; }
	}

	public partial class B
	{
		public virtual int? Id { get; set; }

		public virtual int Prop { get; set; }
	}
}
