#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH392
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task UnsavedMinusOneNoNullReferenceExceptionAsync()
		{
			UnsavedValueMinusOne uvmo = new UnsavedValueMinusOne();
			uvmo.Name = "TEST";
			uvmo.UpdateTimestamp = DateTime.Now;
			Assert.AreEqual(-1, uvmo.Id);
			using (ISession s = OpenSession())
			{
				ITransaction tran = s.BeginTransaction();
				try
				{
					await (s.SaveOrUpdateAsync(uvmo));
					await (tran.CommitAsync());
				}
				catch
				{
					tran.Rollback();
				}
			}
		}
	}
}
#endif
