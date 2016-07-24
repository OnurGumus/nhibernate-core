#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1533
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Person e1 = new Person("Joe", 10, 9);
					Person e2 = new Person("Sally", 10, 8);
					Person e3 = new Person("Tim", 20, 40); //20
					Person e4 = new Person("Fred", 20, 7);
					Person e5 = new Person("Mike", 50, 50);
					await (s.SaveAsync(e1));
					await (s.SaveAsync(e2));
					await (s.SaveAsync(e3));
					await (s.SaveAsync(e4));
					await (s.SaveAsync(e5));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task Can_query_using_two_orderby_and_limit_altogetherAsync()
		{
			using (var sess = OpenSession())
			{
				using (var tran = sess.BeginTransaction())
				{
					var query = sess.CreateQuery("select this.Name,this.ShoeSize,this.IQ from Person as this order by this.IQ asc,this.ShoeSize asc");
					query.SetMaxResults(2);
					query.SetFirstResult(2);
					IList results = await (query.ListAsync());
					Assert.That(results.Count, Is.EqualTo(2));
					Assert.That(((IList)results[0])[0], Is.EqualTo("Fred"));
					Assert.That(((IList)results[1])[0], Is.EqualTo("Tim"));
				}
			}
		}

		[Test]
		public async Task Can_query_using_two_orderby_and_limit_with_maxresult_onlyAsync()
		{
			using (var sess = OpenSession())
			{
				using (var tran = sess.BeginTransaction())
				{
					var query = sess.CreateQuery("select this.Name,this.ShoeSize,this.IQ from Person as this order by this.IQ asc,this.ShoeSize asc");
					query.SetMaxResults(2);
					IList results = await (query.ListAsync());
					Assert.That(results.Count, Is.EqualTo(2));
					Assert.That(((IList)results[0])[0], Is.EqualTo("Sally"));
					Assert.That(((IList)results[1])[0], Is.EqualTo("Joe"));
				}
			}
		}

		[Test]
		public async Task Can_query_using_two_orderby_and_limit_with_firstresult_onlyAsync()
		{
			using (var sess = OpenSession())
			{
				using (var tran = sess.BeginTransaction())
				{
					var query = sess.CreateQuery("select this.Name,this.ShoeSize,this.IQ from Person as this order by this.IQ asc,this.ShoeSize asc");
					query.SetFirstResult(2);
					IList results = await (query.ListAsync());
					Assert.That(results.Count, Is.EqualTo(3));
					Assert.That(((IList)results[0])[0], Is.EqualTo("Fred"));
					Assert.That(((IList)results[1])[0], Is.EqualTo("Tim"));
					Assert.That(((IList)results[2])[0], Is.EqualTo("Mike"));
				}
			}
		}
	}
}
#endif
