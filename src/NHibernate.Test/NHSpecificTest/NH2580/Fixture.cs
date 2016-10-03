using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2580
{
	public partial class Fixture: BugTestCase
	{
		private partial class MyClass
		{
			
		}

		[Test]
		public void WhenPersisterNotFoundShouldThrowAMoreExplicitException()
		{
			using (var s = OpenSession())
			{
				var exeption = Assert.Throws<HibernateException>(() => s.Get<MyClass>(1));
				Assert.That(exeption.Message.ToLowerInvariant(), Is.StringContaining("possible cause"));
			}
		}
	}
}