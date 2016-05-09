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
	/// <summary>
	/// Responsible for generation of runtime metamodel <see cref = "NHibernate.Tuple.Property"/> representations.
	/// Makes distinction between identifier, version, and other (standard) properties.
	/// </summary>
	/// <remarks>
	/// Author: Steve Ebersole
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PropertyFactory
	{
		public static async Task<VersionProperty> BuildVersionPropertyAsync(Mapping.Property property, bool lazyAvailable)
		{
			String mappedUnsavedValue = ((IKeyValue)property.Value).NullValue;
			VersionValue unsavedValue = await (UnsavedValueFactory.GetUnsavedVersionValueAsync(mappedUnsavedValue, GetGetter(property), (IVersionType)property.Type, GetConstructor(property.PersistentClass)));
			bool lazy = lazyAvailable && property.IsLazy;
			return new VersionProperty(property.Name, property.NodeName, property.Value.Type, lazy, property.IsInsertable, property.IsUpdateable, property.Generation == PropertyGeneration.Insert || property.Generation == PropertyGeneration.Always, property.Generation == PropertyGeneration.Always, property.IsOptional, property.IsUpdateable && !lazy, property.IsOptimisticLocked, property.CascadeStyle, unsavedValue);
		}
	}
}