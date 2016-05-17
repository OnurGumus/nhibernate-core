#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH643
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CacheAndLazyCollectionsAsync()
		{
			await (PrepareDataAsync());
			try
			{
				await (AddChildAsync());
				CheckChildrenCount(1);
				await (AddChildAsync());
				CheckChildrenCount(2);
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
					await (session.DeleteAsync(session.Get(typeof (Parent), parentId)));
					await (tx.CommitAsync());
				}
		}

		private async Task AddChildAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					Parent parent = (Parent)session.Get(typeof (Parent), 1);
					Child child = new Child();
					parent.AddChild(child);
					NHibernateUtil.Initialize(parent.Children);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
