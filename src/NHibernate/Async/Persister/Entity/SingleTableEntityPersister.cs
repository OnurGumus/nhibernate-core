using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Persister.Entity
{
	/// <summary>
	/// Default implementation of the <c>ClassPersister</c> interface. Implements the
	/// "table-per-class hierarchy" mapping strategy for an entity class.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SingleTableEntityPersister : AbstractEntityPersister, IQueryable
	{
		public override async Task PostInstantiateAsync()
		{
			await (base.PostInstantiateAsync());
			if (hasSequentialSelects)
			{
				string[] entityNames = SubclassClosure;
				for (int i = 1; i < entityNames.Length; i++)
				{
					ILoadable loadable = (ILoadable)Factory.GetEntityPersister(entityNames[i]);
					if (!loadable.IsAbstract)
					{
						//perhaps not really necessary...
						SqlString sequentialSelect = GenerateSequentialSelect(loadable);
						sequentialSelectStringsByEntityName[entityNames[i]] = sequentialSelect;
					}
				}
			}
		}
	}
}