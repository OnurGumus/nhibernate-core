#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleSubqueryExpression : SubqueryExpression
	{
		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			TypedValue[] superTv = await (base.GetTypedValuesAsync(criteria, criteriaQuery));
			TypedValue[] result = new TypedValue[superTv.Length + 1];
			superTv.CopyTo(result, 1);
			result[0] = FirstTypedValue();
			return result;
		}
	}
}
#endif
