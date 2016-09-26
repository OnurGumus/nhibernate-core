#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3182
{
	[TestFixture, Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			var mother = new Lizard{BodyWeight = 48, Description = "Mother", Children = new List<Animal>()};
			var father = new Lizard{BodyWeight = 48, Description = "Father", Children = new List<Animal>()};
			var child = new Lizard{Mother = mother, Father = father, BodyWeight = 48, Description = "Child", };
			mother.Children.Add(child);
			father.Children.Add(child);
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(mother));
					await (session.SaveAsync(father));
					await (session.SaveAsync(child));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
