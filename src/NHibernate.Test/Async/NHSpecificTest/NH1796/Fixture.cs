#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1796
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task MergeAsync()
		{
			var entity = new Entity{Name = "Vinnie Luther"};
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(entity));
					await (t.CommitAsync());
				}

			entity.DynProps = new Dictionary<string, object>();
			entity.DynProps["StrProp"] = "Modified";
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.Merge(entity);
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.CreateQuery("delete from Entity").ExecuteUpdate();
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task SaveOrUpdateAsync()
		{
			var entity = new Entity{Name = "Vinnie Luther"};
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.SaveOrUpdate(entity);
					await (t.CommitAsync());
				}

			entity.DynProps = new Dictionary<string, object>();
			entity.DynProps["StrProp"] = "Modified";
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.SaveOrUpdate(entity);
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.CreateQuery("delete from Entity").ExecuteUpdate();
					await (t.CommitAsync());
				}
		}
	}
}
#endif
