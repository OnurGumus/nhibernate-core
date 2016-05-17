#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH276
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		/// <summary>
		/// Testing that the syntax of "manytoone.Id" works inside
		/// of an ICriteria.  This was broken in the upgrade to 0.8
		/// </summary>
		[Test]
		public async Task ManyToOneIdAsync()
		{
			Building madison = new Building();
			madison.Id = 1;
			madison.Number = "4800";
			Building college = new Building();
			college.Id = 2;
			college.Number = "6363";
			Office acctg = new Office();
			acctg.Id = 3;
			acctg.Worker = "Bean Counter";
			acctg.Location = college;
			Office hr = new Office();
			hr.Id = 4;
			hr.Worker = "benefits";
			hr.Location = madison;
			Office it = new Office();
			hr.Id = 5;
			it.Worker = "servers";
			it.Location = madison;
			ISession s = OpenSession();
			await (s.SaveAsync(madison));
			await (s.SaveAsync(college));
			await (s.SaveAsync(acctg));
			await (s.SaveAsync(hr));
			await (s.SaveAsync(it));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ICriteria c = s.CreateCriteria(typeof (Office));
			c.Add(Expression.Eq("Location.Id", madison.Id));
			IList results = c.List();
			Assert.AreEqual(2, results.Count, "2 objects");
			foreach (Office office in results)
			{
				Assert.AreEqual(madison.Id, office.Location.Id, "same location as criteria specified");
			}

			c = s.CreateCriteria(typeof (Office));
			c.Add(Expression.Eq("Location.Id", college.Id));
			results = c.List();
			Assert.AreEqual(1, results.Count, "1 objects");
			foreach (Office office in results)
			{
				Assert.AreEqual(college.Id, office.Location.Id, "same location as criteria specified");
			}

			await (s.DeleteAsync("from Office "));
			await (s.DeleteAsync("from Building"));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
