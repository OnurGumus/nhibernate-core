#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3590
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ShouldUpdateAsync()
		{
			_entity.Dates.Add(DateTime.Now);
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					await (s.UpdateAsync(_entity));
					await (tx.CommitAsync());
				}
			}

			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					Assert.That((await (s.GetAsync<Entity>(_entity.Id))).Dates.Count, Is.EqualTo(1));
				}
			}
		}

		[Test]
		public async Task ShouldMergeAsync()
		{
			_entity.Dates.Add(DateTime.Now);
			using (var s = OpenSession())
			{
				using (var tx = s.BeginTransaction())
				{
					s.Merge(_entity);
					await (tx.CommitAsync());
				}
			}

			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					Assert.That((await (s.GetAsync<Entity>(_entity.Id))).Dates.Count, Is.EqualTo(1));
				}
			}
		}
	}
}
#endif
