#if NET_4_5
using System;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Tuple
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PropertyFactory
	{
		/// <summary>
		/// Generates a VersionProperty representation for an entity mapping given its
		/// version mapping Property.
		/// </summary>
		/// <param name = "property">The version mapping Property.</param>
		/// <param name = "lazyAvailable">Is property lazy loading currently available.</param>
		/// <returns>The appropriate VersionProperty definition.</returns>
		public static async Task<VersionProperty> BuildVersionPropertyAsync(Mapping.Property property, bool lazyAvailable)
		{
			String mappedUnsavedValue = ((IKeyValue)property.Value).NullValue;
			VersionValue unsavedValue = await (UnsavedValueFactory.GetUnsavedVersionValueAsync(mappedUnsavedValue, GetGetter(property), (IVersionType)property.Type, GetConstructor(property.PersistentClass)));
			bool lazy = lazyAvailable && property.IsLazy;
			return new VersionProperty(property.Name, property.NodeName, property.Value.Type, lazy, property.IsInsertable, property.IsUpdateable, property.Generation == PropertyGeneration.Insert || property.Generation == PropertyGeneration.Always, property.Generation == PropertyGeneration.Always, property.IsOptional, property.IsUpdateable && !lazy, property.IsOptimisticLocked, property.CascadeStyle, unsavedValue);
		}
	}
}
#endif
