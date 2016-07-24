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
		public void WhenParameterListIsEmptyArrayUsingQueryThenDoesNotTrowsNullReferenceException()
		{
			using (var s = OpenSession())
			{
				var query = s.CreateQuery("from AnEntity a where a.id in (:myList)");
				Assert.That(() => query.SetParameterList("myList", new long[0]), Throws.Exception.Not.InstanceOf<NullReferenceException>());
			}
		}

		[Test]
		public void WhenParameterListIsEmptyGenericCollectionUsingQueryThenDoesNotTrowsNullReferenceException()
		{
			using (var s = OpenSession())
			{
				var query = s.CreateQuery("from AnEntity a where a.id in (:myList)");
				Assert.That(() => query.SetParameterList("myList", new Collection<long>()), Throws.Exception.Not.InstanceOf<NullReferenceException>());
			}
		}

		[Test]
		public Task WhenParameterListIsEmptyCollectionUsingQueryThenTrowsArgumentExceptionAsync()
		{
			try
			{
				using (var s = OpenSession())
				{
					var query = s.CreateQuery("from AnEntity a where a.id in (:myList)");
					var ex = Assert.Throws<QueryException>(() => query.SetParameterList("myList", new List<object>()));
					Assert.That(ex.Message, Is.EqualTo("An empty parameter-list generates wrong SQL; parameter name 'myList'"));
				}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void WhenParameterListIsNullUsingQueryThenTrowsArgumentException()
		{
			using (var s = OpenSession())
			{
				var query = s.CreateQuery("from AnEntity a where a.id in (:myList)");
				Assert.That(() => query.SetParameterList("myList", null), Throws.Exception.InstanceOf<ArgumentNullException>());
			}
		}

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
