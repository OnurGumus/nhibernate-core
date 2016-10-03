#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH962
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH962FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task BugAsync()
		{
			Parent parent = new Parent();
			parent.Name = "Test Parent";
			Child child = new Child();
			child.Name = "Test Child";
			child.Parent = parent;
			parent.Children = new HashSet<Child>();
			parent.Children.Add(child);
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(child));
					Assert.IsTrue(await (session.ContainsAsync(parent)));
					Assert.AreNotEqual(Guid.Empty, parent.Id);
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync(child));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
