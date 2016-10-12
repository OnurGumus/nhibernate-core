#if NET_4_5
using System.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Exceptions;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Linq.ByMethod
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CastTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task CastCountAsync()
		{
			Assert.That(await (session.Query<Cat>().Cast<Animal>().CountAsync()), Is.EqualTo(1));
		}

		[Test]
		public async Task CastWithWhereAsync()
		{
			var pregnatMammal = await ((
				from a in session.Query<Animal>().Cast<Cat>()where a.Pregnant
				select a).FirstOrDefaultAsync());
			Assert.That(pregnatMammal, Is.Not.Null);
		}

		[Test]
		public async Task CastDowncastAsync()
		{
			var query = session.Query<Mammal>().Cast<Dog>();
			Assert.ThrowsAsync<GenericADOException>(async () =>
			{
				// the list contains at least one Cat then should Throws
				await (query.ToListAsync());
			}

			);
		}

		[Test, Ignore("Not fixed yet. The method OfType does not work as expected.")]
		public async Task CastDowncastUsingOfTypeAsync()
		{
			var query = session.Query<Animal>().OfType<Mammal>().Cast<Dog>();
			// the list contains at least one Cat then should Throws
			Assert.That(async () => await (query.ToListAsync()), Throws.Exception);
		}
	}
}
#endif
