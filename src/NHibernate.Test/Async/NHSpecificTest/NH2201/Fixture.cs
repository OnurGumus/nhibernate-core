#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2201
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return factory.ConnectionProvider.Driver.SupportsMultipleQueries;
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from System.Object"));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new Parent()));
					await (s.SaveAsync(new SubClass()
					{Name = "test"}));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanUseMutliCriteriaAndFetchSelectAsync()
		{
			using (var s = OpenSession())
			{
				Console.WriteLine("*** start");
				var results = await (s.CreateMultiCriteria().Add<Parent>(s.CreateCriteria<Parent>()).Add<Parent>(s.CreateCriteria<Parent>()).ListAsync());
				var result1 = (IList<Parent>)results[0];
				var result2 = (IList<Parent>)results[1];
				Assert.That(result1.Count, Is.EqualTo(2));
				Assert.That(result2.Count, Is.EqualTo(2));
				Console.WriteLine("*** end");
			}
		}
	}
}
#endif
