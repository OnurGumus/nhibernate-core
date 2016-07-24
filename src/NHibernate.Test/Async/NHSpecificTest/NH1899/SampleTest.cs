#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1899
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				Parent entity = new Parent();
				entity.Id = 1;
				entity.Relations = new Dictionary<Key, Value>();
				entity.Relations.Add(Key.One, Value.ValOne);
				entity.Relations.Add(Key.Two, Value.ValTwo);
				await (session.SaveAsync(entity));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				string hql = "from System.Object";
				await (session.DeleteAsync(hql));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task ShouldNotThrowOnMergeAsync()
		{
			Parent entity;
			using (ISession session = OpenSession())
			{
				entity = await (session.GetAsync<Parent>(1));
				session.Close();
				session.Dispose();
			}

			using (ISession session2 = OpenSession())
			{
				entity = session2.Merge(entity);
			}
		}
	}
}
#endif
