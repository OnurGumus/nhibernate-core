#if NET_4_5
using System.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EnumTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task CanQueryOnEnumStoredAsInt32_High_1Async()
		{
			await (CanQueryOnEnumStoredAsInt32Async(EnumStoredAsInt32.High, 1));
		}

		[Test]
		public async Task CanQueryOnEnumStoredAsInt32_Unspecified_2Async()
		{
			await (CanQueryOnEnumStoredAsInt32Async(EnumStoredAsInt32.Unspecified, 2));
		}

		public async Task CanQueryOnEnumStoredAsInt32Async(EnumStoredAsInt32 type, int expectedCount)
		{
			var query = await ((
				from user in db.Users
				where user.Enum2 == type
				select user).ToListAsync());
			Assert.AreEqual(expectedCount, query.Count);
		}

		[Test]
		public async Task CanQueryOnEnumStoredAsString_Meduim_2Async()
		{
			await (CanQueryOnEnumStoredAsStringAsync(EnumStoredAsString.Medium, 2));
		}

		[Test]
		public async Task CanQueryOnEnumStoredAsString_Small_1Async()
		{
			await (CanQueryOnEnumStoredAsStringAsync(EnumStoredAsString.Small, 1));
		}

		public async Task CanQueryOnEnumStoredAsStringAsync(EnumStoredAsString type, int expectedCount)
		{
			var query = await ((
				from user in db.Users
				where user.Enum1 == type
				select user).ToListAsync());
			Assert.AreEqual(expectedCount, query.Count);
		}
	}
}
#endif
