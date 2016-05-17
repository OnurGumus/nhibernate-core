#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1792
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		/// <summary>
		/// Deletes all the product entities from the persistence medium
		/// </summary>
		private async Task DeleteAllAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Product"));
					await (trans.CommitAsync());
				}
			}
		}
	}
}
#endif
