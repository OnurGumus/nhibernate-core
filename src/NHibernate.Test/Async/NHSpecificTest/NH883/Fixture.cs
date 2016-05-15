#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH883
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
				Cat c = (Cat)await (sess.GetAsync(typeof (Cat), catId));
				Cat kitten = new Cat();
				c.Children.Add(kitten);
				kitten.Mother = c;
				await (sess.SaveAsync(kitten));
				// Double flush
				await (sess.FlushAsync());
				await (sess.FlushAsync());
				await (sess.DeleteAsync(c));
				await (sess.DeleteAsync(kitten));
				await (sess.FlushAsync());
			}
		}
	}
}
#endif
