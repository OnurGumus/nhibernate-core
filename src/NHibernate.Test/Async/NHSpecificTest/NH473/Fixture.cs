#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH473
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ChildItemsGetInOrderWhenUsingFetchJoinAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					var result = await (session.CreateCriteria(typeof (Parent)).SetFetchMode("Children", FetchMode.Join).ListAsync<Parent>());
					Assert.That(result[0].Children[0].Name, Is.EqualTo("Ayende"));
					Assert.That(result[0].Children[1].Name, Is.EqualTo("Dario"));
					Assert.That(result[0].Children[2].Name, Is.EqualTo("Fabio"));
				}
		}

		[Test]
		public async Task ChildItemsGetInOrderWhenUsingFetchJoinUniqueResultAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					var result = await (session.CreateCriteria(typeof (Parent)).SetFetchMode("Children", FetchMode.Join).UniqueResultAsync<Parent>());
					Assert.That(result.Children[0].Name, Is.EqualTo("Ayende"));
					Assert.That(result.Children[1].Name, Is.EqualTo("Dario"));
					Assert.That(result.Children[2].Name, Is.EqualTo("Fabio"));
				}
		}
	}
}
#endif
