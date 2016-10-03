#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH898
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH898FixtureAsync : BugTestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest.NH898.ClassA.hbm.xml", "NHSpecificTest.NH898.ClassBParent.hbm.xml", "NHSpecificTest.NH898.ClassB.hbm.xml", "NHSpecificTest.NH898.ClassC.hbm.xml", };
			}
		}

		[Test]
		public async Task BugAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					ClassA a = new ClassA();
					await (s.SaveAsync(a));
					await (t.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction t = session.BeginTransaction())
				{
					IList l = await (session.CreateQuery("from ClassA a left join fetch a.B b").ListAsync());
					Console.Write(l.ToString());
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from ClassA"));
					await (s.FlushAsync());
					await (t.CommitAsync());
				}
		}
	}
}
#endif
