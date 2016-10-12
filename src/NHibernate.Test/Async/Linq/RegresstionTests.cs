#if NET_4_5
using System.Linq;
using NUnit.Framework;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class RegresstionTestsAsync : LinqTestCaseAsync
	{
		/// <summary>
		/// http://aspzone.com/tech/nhibernate-linq-troubles/
		/// </summary>
		[Test]
		public async Task HierarchicalQueries_InlineConstantAsync()
		{
			var children =
				from s in db.Role
				where s.ParentRole != null
				select s;
			Assert.AreEqual(0, await (children.CountAsync()));
			var roots =
				from s in db.Role
				where s.ParentRole == null
				select s;
			Assert.AreEqual(2, await (roots.CountAsync()));
		}

		[Test]
		public async Task HierarchicalQueries_VariableAsync()
		{
			Role testRole = null;
			var children =
				from s in db.Role
				where s.ParentRole != testRole
				select s;
			Assert.AreEqual(0, await (children.CountAsync()));
			var roots =
				from s in db.Role
				where s.ParentRole == testRole
				select s;
			Assert.AreEqual(2, await (roots.CountAsync()));
		}

		[Test]
		public async Task CanUseNullConstantAndRestrictionAsync()
		{
			var roots =
				from s in db.Role
				where s.ParentRole == null && s.Name == "Admin"
				select s;
			Assert.AreEqual(1, await (roots.CountAsync()));
		}
	}
}
#endif
