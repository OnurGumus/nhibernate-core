#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate.Test
{
	using NUnit.Framework;

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NHibernateUtilTestAsync
	{
		[Test]
		public void CanGuessTypeOfInt32ByValue()
		{
			Assert.AreEqual(NHibernateUtil.Int32, NHibernateUtil.GuessType(15));
		}

		[Test]
		public void CanGuessTypeOfInt32ByType()
		{
			Assert.AreEqual(NHibernateUtil.Int32, NHibernateUtil.GuessType(typeof (int)));
		}

		[Test]
		public void CanGuessTypeOfNullableInt32ByType()
		{
			Assert.AreEqual(NHibernateUtil.Int32, NHibernateUtil.GuessType(typeof (int ? )));
		}

		[Test]
		public void CanGuessTypeOfNullableInt32ByValue()
		{
			int ? val = 15;
			Assert.AreEqual(NHibernateUtil.Int32, NHibernateUtil.GuessType(val));
		}
	}
}
#endif
