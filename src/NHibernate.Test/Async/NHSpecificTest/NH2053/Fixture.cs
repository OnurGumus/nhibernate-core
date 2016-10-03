#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2053
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect;
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					Dog snoopy = new Dog()
					{Name = "Snoopy", Talkable = false};
					snoopy.Name = "Snoopy";
					Dog Jake = new Dog()
					{Name = "Jake the dog", Talkable = true};
					await (session.SaveAsync(snoopy));
					await (session.SaveAsync(Jake));
					Cat kitty = new Cat()
					{Name = "Kitty"};
					await (session.SaveAsync(kitty));
					await (tran.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Dog"));
					await (session.DeleteAsync("from Animal"));
					await (tran.CommitAsync());
				}
			}
		}

		[Test]
		public async Task JoinedSubClass_FilterAsync()
		{
			using (var session = this.OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					session.EnableFilter("talkableFilter").SetParameter("talkable", true);
					var snoopy = await (session.QueryOver<Dog>().Where(x => x.Name == "Snoopy").SingleOrDefaultAsync());
					Assert.AreEqual(null, snoopy); // there are no talking dog named Snoopy.
					var jake = await (session.QueryOver<Dog>().Where(x => x.Name == "Jake the dog").SingleOrDefaultAsync());
					Assert.AreNotEqual(null, jake);
					Assert.AreEqual("Jake the dog", jake.Name);
					var kitty = await (session.QueryOver<Cat>().Where(x => x.Name == "Kitty").SingleOrDefaultAsync());
					Assert.AreNotEqual(null, kitty);
					Assert.AreEqual("Kitty", kitty.Name);
				}
			}
		}
	}
}
#endif
