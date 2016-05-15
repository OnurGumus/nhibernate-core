#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH704
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ReAttachCatTestAsync()
		{
			try
			{
				using (ISession sess = OpenSession())
				{
					Cat c = new Cat();
					await (sess.SaveAsync(c));
					await (sess.FlushAsync());
					sess.Clear();
					await (sess.LockAsync(c, LockMode.None)); //Exception throw here
				}
			}
			finally
			{
				using (ISession s = OpenSession())
				{
					await (s.DeleteAsync("from Cat"));
					await (s.FlushAsync());
				}
			}
		}
	}
}
#endif
