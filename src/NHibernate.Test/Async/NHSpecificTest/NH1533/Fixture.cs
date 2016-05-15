#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1533
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
