#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3570
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BiFixture : BugTestCase
	{
		[Test]
		[KnownBug("NH-3570")]
		public async Task ShouldNotSaveRemoveChildAsync()
		{
			var parent = new BiParent();
			parent.AddChild(new BiChild());
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					id = (Guid)await (s.SaveAsync(parent));
					parent.Children.Clear();
					parent.AddChild(new BiChild());
					await (tx.CommitAsync());
				}
			}

			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					Assert.That((await (s.GetAsync<BiParent>(id))).Children.Count, Is.EqualTo(1));
					Assert.That(s.CreateCriteria<BiChild>().List().Count, Is.EqualTo(1));
				}
			}
		}
	}
}
#endif
