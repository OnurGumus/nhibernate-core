#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1891
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task FormulaEscapingAsync()
		{
			string name = "Test";
			B b = new B();
			b.Name = name;
			A a = new A();
			a.FormulaConstraint = name;
			ISession s = OpenSession();
			await (s.SaveAsync(b));
			await (s.SaveAsync(a));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			a = await (s.GetAsync<A>(a.Id));
			Assert.AreEqual(1, a.FormulaCount);
			s.Close();
		}
	}
}
#endif
