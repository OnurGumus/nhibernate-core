#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1421
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task WhenParameterListIsEmptyUsingQueryThenDoesNotTrowsNullReferenceExceptionAsync()
		{
			using (var s = OpenSession())
			{
				var query = s.CreateQuery("from AnEntity a where a.id in (:myList)");
				Assert.That(async () => await (query.SetParameterList("myList", new long[0]).ListAsync()), Throws.Exception.Not.InstanceOf<NullReferenceException>());
			}
		}
	}
}
#endif
