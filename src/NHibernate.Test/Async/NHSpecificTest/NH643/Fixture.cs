#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH643
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH643";
			}
		}

		private object parentId;
		[Test]
		public async Task CacheAndLazyCollectionsAsync()
		{
			await (PrepareDataAsync());
			try
			{
				await (AddChildAsync());
				await (CheckChildrenCountAsync(1));
				await (AddChildAsync());
				await (CheckChildrenCountAsync(2));
			}
			finally
			{
				await (CleanUpAsync());
			}
		}

		private async Task PrepareDataAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					parentId = await (session.SaveAsync(new Parent()));
					await (tx.CommitAsync());
				}
		}

		private async Task CleanUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync(await (session.GetAsync(typeof (Parent), parentId))));
					await (tx.CommitAsync());
				}
		}

		private async Task AddChildAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					Parent parent = (Parent)await (session.GetAsync(typeof (Parent), 1));
					Child child = new Child();
					parent.AddChild(child);
					await (NHibernateUtil.InitializeAsync(parent.Children));
					await (tx.CommitAsync());
				}
		}

		private async Task CheckChildrenCountAsync(int count)
		{
			using (ISession session = OpenSession())
			{
				Parent parent = (Parent)await (session.GetAsync(typeof (Parent), 1));
				Assert.AreEqual(count, parent.Children.Count);
			}
		}
	}
}
#endif
