#if NET_4_5
using System.Collections;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3795
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected Child childAliasField = null;
		protected A aAliasField = null;
		protected override IList Mappings
		{
			get
			{
				return new[]{"ParentChild.hbm.xml", "ABC.hbm.xml"};
			}
		}

		[Test]
		public async Task TestFieldAliasInQueryOverAsync()
		{
			using (var s = sessions.OpenSession())
			{
				A rowalias = null;
				await (s.QueryOver(() => aAliasField).SelectList(list => list.Select(() => aAliasField.Id).WithAlias(() => rowalias.Id)).ListAsync());
			}
		}

		[Test]
		public async Task TestFieldAliasInQueryOverWithConversionAsync()
		{
			using (var s = sessions.OpenSession())
			{
				B rowalias = null;
				await (s.QueryOver(() => aAliasField).SelectList(list => list.Select(() => ((B)aAliasField).Count).WithAlias(() => rowalias.Count)).ListAsync());
			}
		}

		[Test]
		public async Task TestFieldAliasInJoinAliasAsync()
		{
			using (var s = sessions.OpenSession())
			{
				Child rowalias = null;
				await (s.QueryOver<Parent>().JoinAlias(p => p.Child, () => childAliasField).SelectList(list => list.Select(() => childAliasField.Id).WithAlias(() => rowalias.Id)).ListAsync());
			}
		}

		[Test]
		public async Task TestFieldAliasInJoinQueryOverAsync()
		{
			using (var s = sessions.OpenSession())
			{
				Child rowalias = null;
				await (s.QueryOver<Parent>().JoinQueryOver(p => p.Child, () => childAliasField).SelectList(list => list.Select(() => childAliasField.Id).WithAlias(() => rowalias.Id)).ListAsync());
			}
		}

		[Test]
		public async Task TestAliasInQueryOverAsync()
		{
			Child childAlias = null;
			A aAlias = null;
			using (var s = sessions.OpenSession())
			{
				A rowalias = null;
				await (s.QueryOver(() => aAlias).SelectList(list => list.Select(() => aAlias.Id).WithAlias(() => rowalias.Id)).ListAsync());
			}
		}

		[Test]
		public async Task TestAliasInQueryOverWithConversionAsync()
		{
			Child childAlias = null;
			A aAlias = null;
			using (var s = sessions.OpenSession())
			{
				B rowalias = null;
				await (s.QueryOver(() => aAlias).SelectList(list => list.Select(() => ((B)aAlias).Count).WithAlias(() => rowalias.Count)).ListAsync());
			}
		}

		[Test]
		public async Task TestAliasInJoinAliasAsync()
		{
			Child childAlias = null;
			A aAlias = null;
			using (var s = sessions.OpenSession())
			{
				Child rowalias = null;
				await (s.QueryOver<Parent>().JoinAlias(p => p.Child, () => childAlias).SelectList(list => list.Select(() => childAlias.Id).WithAlias(() => rowalias.Id)).ListAsync());
			}
		}

		[Test]
		public async Task TestAliasInJoinQueryOverAsync()
		{
			Child childAlias = null;
			A aAlias = null;
			using (var s = sessions.OpenSession())
			{
				Child rowalias = null;
				await (s.QueryOver<Parent>().JoinQueryOver(p => p.Child, () => childAlias).SelectList(list => list.Select(() => childAlias.Id).WithAlias(() => rowalias.Id)).ListAsync());
			}
		}
	}
}
#endif
