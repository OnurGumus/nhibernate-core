#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Id.Insert
{
	/// <summary> 
	/// <see cref = "IInsertGeneratedIdentifierDelegate"/> implementation where the
	/// underlying strategy causes the generated identitifer to be returned, as an
	/// effect of performing the insert statement, in a Output parameter.
	/// Thus, there is no need for an additional sql statement to determine the generated identitifer. 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OutputParamReturningDelegate : AbstractReturningDelegate
	{
		public override Task<object> ExecuteAndExtractAsync(DbCommand insert, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(ExecuteAndExtract(insert, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
