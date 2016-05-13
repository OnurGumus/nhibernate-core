using System;

namespace NHibernate.Mapping.ByCode
{
	public partial interface IColumnsMapper
	{
		void Column(Action<IColumnMapper> columnMapper);
		void Columns(params Action<IColumnMapper>[] columnMapper);
		void Column(string name);
	}
}