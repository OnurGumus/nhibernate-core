#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Docs.ExampleParentChild
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UpdateFixture : TestCase
	{
		[Test]
		public async Task UpdateAsync()
		{
			ISession session1 = OpenSession();
			Parent parent1 = new Parent();
			Child child1 = new Child();
			parent1.AddChild(child1);
			long pId = (long)await (session1.SaveAsync(parent1));
			long cId = (long)await (session1.SaveAsync(child1));
			await (session1.FlushAsync());
			session1.Close();
			ISession session2 = OpenSession();
			Parent parent = await (session2.LoadAsync(typeof (Parent), pId)) as Parent;
			Child child = await (session2.LoadAsync(typeof (Child), cId)) as Child;
			session2.Close();
			parent.AddChild(child);
			Child newChild = new Child();
			parent.AddChild(newChild);
			ISession session = OpenSession();
			await (session.UpdateAsync(parent));
			await (session.FlushAsync());
			session.Close();
			// Clean up
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Parent"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
