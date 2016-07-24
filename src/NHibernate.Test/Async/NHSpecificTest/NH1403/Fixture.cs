#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1403
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task BugAsync()
		{
			Hobby h = new Hobby("Develop software");
			Person p = new Male("Diego");
			h.Person = p;
			Hobby h1 = new Hobby("Drive Car");
			Person p1 = new Female("Luciana");
			h1.Person = p1;
			object savedIdMale;
			object saveIdFemale;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					savedIdMale = await (s.SaveAsync(h));
					saveIdFemale = await (s.SaveAsync(h1));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					h = await (s.GetAsync<Hobby>(savedIdMale));
					h1 = await (s.GetAsync<Hobby>(saveIdFemale));
					Assert.IsTrue(h.Person is Male);
					Assert.IsTrue(h1.Person is Female);
					await (s.DeleteAsync(h));
					await (s.DeleteAsync(h1));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
