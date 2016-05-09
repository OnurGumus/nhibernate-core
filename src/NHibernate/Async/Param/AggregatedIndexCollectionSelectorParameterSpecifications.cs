using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Param
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AggregatedIndexCollectionSelectorParameterSpecifications : IParameterSpecification
	{
		//public int Bind(IDbCommand statement, QueryParameters qp, ISessionImplementor session, int position)
		//{
		//  int bindCount = 0;
		//  foreach (IParameterSpecification spec in _paramSpecs)
		//  {
		//    bindCount += spec.Bind(statement, qp, session, position + bindCount);
		//  }
		//  return bindCount;
		//}
		public async Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			throw new NotImplementedException();
		}

		public async Task BindAsync(IDbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			throw new NotImplementedException();
		}
	}
}