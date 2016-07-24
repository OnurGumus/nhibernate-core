#if NET_4_5
using NHibernate.Engine;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.QueryTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryParametersFixtureAsync
	{
		[Test]
		public void ValidateNullParameters()
		{
			QueryParameters qp = new QueryParameters(null, null);
			qp.ValidateParameters();
		}

		[Test]
		public void ValidateOk()
		{
			QueryParameters qp = new QueryParameters(new IType[]{NHibernateUtil.String}, new object[]{"string"});
			qp.ValidateParameters();
		}

		[Test]
		public Task ValidateFailureDifferentLengthsAsync()
		{
			try
			{
				QueryParameters qp = new QueryParameters(new IType[]{NHibernateUtil.String}, new object[]{});
				Assert.Throws<QueryException>(() => qp.ValidateParameters());
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
