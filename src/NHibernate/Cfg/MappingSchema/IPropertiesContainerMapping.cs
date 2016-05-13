using System.Collections.Generic;
namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IPropertiesContainerMapping
	{
		IEnumerable<IEntityPropertyMapping> Properties { get; }
	}
}