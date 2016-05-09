using System;

namespace NHibernate.Linq
{
	public partial class LinqExtensionMethodAttribute: Attribute
	{
		public LinqExtensionMethodAttribute()
		{
		}

		public LinqExtensionMethodAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}