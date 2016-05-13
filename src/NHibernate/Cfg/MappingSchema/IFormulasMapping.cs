using System.Collections.Generic;

namespace NHibernate.Cfg.MappingSchema
{
	public partial interface IFormulasMapping
	{
		IEnumerable<HbmFormula> Formulas { get; }
	}
}