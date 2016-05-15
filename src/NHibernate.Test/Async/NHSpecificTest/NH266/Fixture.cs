#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH266
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task BaseClassLoadAsync()
		{
			// just do a straight load
			ISession s = OpenSession();
			A a = await (s.LoadAsync(typeof (A), aId)) as A;
			Assert.AreEqual("the a", a.Name);
			s.Close();
			// load instance through hql
			s = OpenSession();
			IQuery q = s.CreateQuery("from A as a where a.id = :id ");
			q.SetParameter("id", aId);
			a = await (q.UniqueResultAsync()) as A;
			Assert.AreEqual("the a", a.Name);
			s.Close();
			// load instance through Criteria
			s = OpenSession();
			ICriteria c = s.CreateCriteria(typeof (A));
			c.Add(Expression.Eq("Id", aId));
			a = await (c.UniqueResultAsync()) as A;
			Assert.AreEqual("the a", a.Name);
			s.Close();
		}

		/// <summary>
		/// This is testing problems that existed in 0.8.0-2 with extra "AND"
		/// being added to the sql when there was a discriminator-value.
		/// </summary>
		[Test]
		public async Task SpecificSubclassAsync()
		{
			ISession s = OpenSession();
			B b = await (s.LoadAsync(typeof (B), bId)) as B;
			Assert.AreEqual("the b", b.Name);
			s.Close();
			// load a instance of B through hql
			s = OpenSession();
			IQuery q = s.CreateQuery("from B as b where b.id = :id");
			q.SetParameter("id", bId);
			b = await (q.UniqueResultAsync()) as B;
			Assert.AreEqual("the b", b.Name);
			s.Close();
			// load a instance of B through Criteria
			s = OpenSession();
			ICriteria c = s.CreateCriteria(typeof (B));
			c.Add(Expression.Eq("Id", bId));
			b = await (c.UniqueResultAsync()) as B;
			Assert.AreEqual("the b", b.Name);
			s.Close();
		}
	}
}
#endif
