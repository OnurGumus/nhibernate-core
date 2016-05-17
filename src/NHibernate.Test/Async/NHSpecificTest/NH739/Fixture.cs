#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH739
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			int catId;
			using (ISession sess = OpenSession())
			{
				Cat c = new Cat();
				await (sess.SaveAsync(c));
				catId = c.Id;
				await (sess.FlushAsync());
			}

			using (ISession sess = OpenSession())
			{
				Cat c = (Cat)sess.Get(typeof (Cat), catId);
				Cat kitten = new Cat();
				c.Children.Add(kitten);
				kitten.Mother = c;
				await (sess.SaveAsync(kitten));
				Assert.AreEqual(1, c.Children.Count); //Test will fail here, the c.Children.Count is 2 here
				await (sess.DeleteAsync(c));
				await (sess.DeleteAsync(kitten));
				await (sess.FlushAsync());
			}
		}
	}
}
#endif
