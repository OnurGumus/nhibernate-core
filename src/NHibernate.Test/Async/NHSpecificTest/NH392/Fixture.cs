#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH392
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH392";
			}
		}

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

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				await (s.DeleteAsync("from UnsavedValueMinusOne"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
