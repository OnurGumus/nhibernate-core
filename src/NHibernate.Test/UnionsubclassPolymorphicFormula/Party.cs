using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Test.UnionsubclassPolymorphicFormula
{
	public abstract partial class Party
	{
		public virtual long Id { get; protected internal set; }
		public abstract string Name { get; }
	}
}
