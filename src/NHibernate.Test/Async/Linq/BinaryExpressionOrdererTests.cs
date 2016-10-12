#if NET_4_5
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BinaryExpressionOrdererTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task ValuePropertySwapsToPropertyValueAsync()
		{
			var query = await ((
				from user in db.Users
				where ("ayende" == user.Name)select user).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}

		[Test]
		public async Task PropertyValueDoesntSwapsAsync()
		{
			var query = await ((
				from user in db.Users
				where (user.Name == "ayende")select user).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}

		[Test]
		public async Task PropertyPropertyDoesntSwapAsync()
		{
			var query = await ((
				from user in db.Users
				where (user.Name == user.Name)select user).ToListAsync());
			Assert.AreEqual(3, query.Count);
		}

		[Test]
		public async Task EqualsSwapsToEqualsAsync()
		{
			var query = await ((
				from user in db.Users
				where ("ayende" == user.Name)select user).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}

		[Test]
		public async Task NotEqualsSwapsToNotEqualsAsync()
		{
			var query = await ((
				from user in db.Users
				where ("ayende" != user.Name)select user).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task GreaterThanSwapsToLessThanAsync()
		{
			var query = await ((
				from user in db.Users
				where (3 > user.Id)select user).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task GreaterThanOrEqualToSwapsToLessThanOrEqualToAsync()
		{
			var query = await ((
				from user in db.Users
				where (2 >= user.Id)select user).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task LessThanSwapsToGreaterThanAsync()
		{
			var query = await ((
				from user in db.Users
				where (1 < user.Id)select user).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task LessThanOrEqualToSwapsToGreaterThanOrEqualToAsync()
		{
			var query = await ((
				from user in db.Users
				where (2 <= user.Id)select user).ToListAsync());
			Assert.AreEqual(2, query.Count);
		}

		[Test]
		public async Task ValuePropertySwapsToPropertyValueUsingEqualsFromConstantAsync()
		{
			// check NH-2440
			var query = await ((
				from user in db.Users
				where ("ayende".Equals(user.Name))select user).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}

		[Test]
		public async Task ValuePropertySwapsToPropertyValueUsingEqualsToConstantAsync()
		{
			// check NH-2440
			var query = await ((
				from user in db.Users
				where (user.Name.Equals("ayende"))select user).ToListAsync());
			Assert.AreEqual(1, query.Count);
		}
	}
}
#endif
