#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH734
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[TestAttribute]
		public async Task LimitProblemAsync()
		{
			using (ISession session = sessions.OpenSession())
			{
				ICriteria criteria = session.CreateCriteria(typeof (MyClass));
				criteria.SetMaxResults(100);
				criteria.SetFirstResult(0);
				try
				{
					session.BeginTransaction();
					IList result = criteria.List();
					await (session.Transaction.CommitAsync());
				}
				catch
				{
					if (session.Transaction != null)
					{
						session.Transaction.Rollback();
					}

					throw;
				}
			}
		}
	}
}
#endif
