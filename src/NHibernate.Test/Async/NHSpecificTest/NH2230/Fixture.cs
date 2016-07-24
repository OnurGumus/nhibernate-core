#if NET_4_5
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2230
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task CanCreacteRetrieveDeleteComponentsWithPrivateReferenceSetterToParentAsync()
		{
			var entity = new MyEntity();
			var component = new MyComponentWithParent(entity)
			{Something = "A"};
			entity.Component = component;
			entity.Children = new List<MyComponentWithParent>{new MyComponentWithParent(entity)
			{Something = "B"}, new MyComponentWithParent(entity)
			{Something = "C"}};
			object poid;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					poid = await (s.SaveAsync(entity));
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var savedEntity = await (s.GetAsync<MyEntity>(poid));
					var myComponentWithParent = savedEntity.Component;
					Assert.That(myComponentWithParent, Is.Not.Null);
					Assert.That(myComponentWithParent.Parent, Is.SameAs(savedEntity));
					Assert.That(myComponentWithParent.Something, Is.EqualTo("A"));
					Assert.That(savedEntity.Children.Select(c => c.Something), Is.EquivalentTo(new[]{"B", "C"}));
					Assert.That(savedEntity.Children.All(c => ReferenceEquals(c.Parent, savedEntity)), Is.True);
					await (s.DeleteAsync(savedEntity));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
