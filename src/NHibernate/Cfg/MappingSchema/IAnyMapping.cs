using System.Collections.Generic;

namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IAnyMapping
	{
		string MetaType { get; }
		ICollection<HbmMetaValue> MetaValues { get; }
	}
}