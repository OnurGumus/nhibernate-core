#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3570
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UniFixture : BugTestCase
	{
		[Test]
		[KnownBug("NH-3570")]
		public async Task ShouldNotSaveRemoveChildAsync()
		{
			var parent = new UniParent();
			parent.Children.Add(new UniChild());
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					id = (Guid)await (s.SaveAsync(parent));
					parent.Children.Clear();
					parent.Children.Add(new UniChild());
					await (tx.CommitAsync());
				}
			}

			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					Assert.That((await (s.GetAsync<UniParent>(id))).Children.Count, Is.EqualTo(1));
					Assert.That(s.CreateCriteria<UniChild>().List().Count, Is.EqualTo(1));
				}
			}
		}
	}
}
#endif
