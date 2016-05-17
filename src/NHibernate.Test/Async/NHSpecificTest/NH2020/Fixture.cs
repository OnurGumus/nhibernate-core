#if NET_4_5
using NUnit.Framework;
using NHibernate.Dialect;
using NHibernate.Exceptions;
using NHibernate.Test.ExceptionsTest;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2020
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ISQLExceptionConverter_gets_called_if_batch_size_enabledAsync()
		{
			long oneId;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var one = new One();
					await (s.SaveAsync(one));
					var many = new Many{One = one};
					await (s.SaveAsync(many));
					await (tx.CommitAsync());
					oneId = one.Id;
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var one = s.Load<One>(oneId);
					await (s.DeleteAsync(one));
					Assert.That(async () => await (tx.CommitAsync()), Throws.TypeOf<ConstraintViolationException>());
				}
		}
	}
}
#endif
