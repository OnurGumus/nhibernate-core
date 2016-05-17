#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH681
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			Foo parent = new Foo();
			parent.Children.Add(new Foo());
			parent.Children.Add(new Foo());
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(parent));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				Foo parentReloaded = await (s.GetAsync<Foo>(parent.Id));
				parentReloaded.Children.RemoveAt(0);
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync(await (s.GetAsync<Foo>(parent.Id))));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
