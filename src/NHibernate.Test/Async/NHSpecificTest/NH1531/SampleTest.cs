#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1531
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		private async Task FillDbAsync()
		{
			using (ISession session = OpenSession())
			{
				var entity = new Parent{Id = 1};
				entity.AddNewChild();
				await (session.SaveAsync(entity));
				var entity2 = new Parent{Id = 2};
				await (session.SaveAsync(entity2));
				await (session.FlushAsync());
			}
		}

		private async Task CleanDbAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from Parent"));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task ReparentingShouldNotFailAsync()
		{
			await (FillDbAsync());
			using (ISession session = OpenSession())
			{
				var parent1 = await (session.GetAsync<Parent>(1));
				var parent2 = await (session.GetAsync<Parent>(2));
				Assert.AreEqual(1, parent1.Children.Count);
				Assert.AreEqual(0, parent2.Children.Count);
				Child p1Child = parent1.Children[0];
				Assert.IsNotNull(p1Child);
				parent1.DetachAllChildren();
				parent2.AttachNewChild(p1Child);
				session.SaveOrUpdate(parent1);
				session.SaveOrUpdate(parent2);
				// NHibernate.ObjectDeletedException : 
				// deleted object would be re-saved by cascade (remove deleted object from associations)[NHibernate.Test.NHSpecificTest.NH1531.Child#0]
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				// should exist only one child
				var l = session.CreateQuery("from Child").List();
				Assert.That(l.Count, Is.EqualTo(1));
			}

			await (CleanDbAsync());
		}

		[Test]
		public async Task DeleteParentDeleteChildInCascadeAsync()
		{
			await (FillDbAsync());
			await (CleanDbAsync());
		// The TestCase is checking the empty db
		}
	}
}
#endif
