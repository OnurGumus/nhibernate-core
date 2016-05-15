#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2374
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH2374Fixture : BugTestCase
	{
		[Test]
		public async Task OneToOne_with_EntityMode_MapAsync()
		{
			int id;
			using (ISession sroot = OpenSession())
			{
				using (ISession s = sroot.GetSession(EntityMode.Map))
				{
					using (ITransaction t = s.BeginTransaction())
					{
						var parent = new Hashtable();
						var child = new Hashtable{{"Parent", parent}};
						parent["Child"] = child;
						id = (int)await (s.SaveAsync("Parent", parent));
						await (s.FlushAsync());
						await (t.CommitAsync());
					}
				}
			}

			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					var p = await (s.GetAsync("Parent", id)) as IDictionary;
					Assert.That(p["Child"], Is.Not.Null);
					await (s.DeleteAsync("Parent", p));
					await (t.CommitAsync());
				}
			}
		}

		[Test]
		public async Task OneToOne_with_EntityMode_PocoAsync()
		{
			int id;
			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					var parent = new Hashtable();
					var child = new Hashtable{{"Parent", parent}};
					parent["Child"] = child;
					id = (int)await (s.SaveAsync("Parent", parent));
					await (s.FlushAsync());
					await (t.CommitAsync());
				}
			}

			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					var p = await (s.GetAsync("Parent", id)) as IDictionary;
					Assert.That(p["Child"], Is.Not.Null);
					await (s.DeleteAsync("Parent", p));
					await (t.CommitAsync());
				}
			}
		}
	}
}
#endif
