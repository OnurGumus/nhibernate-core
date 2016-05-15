#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2491
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task InheritanceSameColumnNameAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var subClass = new SubClass();
					var referencing = new ReferencingClass()
					{SubClass = subClass};
					await (session.SaveAsync(subClass));
					await (session.SaveAsync(referencing));
					await (transaction.CommitAsync());
				}

			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var referencing = await (session.CreateQuery("from ReferencingClass").UniqueResultAsync<ReferencingClass>());
					// accessing a property of the base class to activate lazy loading
					// this line crashes because it tries to find the base class by
					// the wrong column name.
					BaseClass another;
					Assert.That(() => another = referencing.SubClass.Another, Throws.Nothing);
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
