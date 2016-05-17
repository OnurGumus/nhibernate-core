#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Insertordering
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InsertOrderingFixture : TestCase
	{
		[Test]
		public async Task BatchOrderingAsync()
		{
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					for (int i = 0; i < instancesPerEach; i++)
					{
						var user = new User{UserName = "user-" + i};
						var group = new Group{Name = "group-" + i};
						await (s.SaveAsync(user));
						await (s.SaveAsync(group));
						user.AddMembership(group);
					}

					StatsBatcher.Reset();
					await (s.Transaction.CommitAsync());
				}

			int expectedBatchesPerEntity = (instancesPerEach / batchSize) + ((instancesPerEach % batchSize) == 0 ? 0 : 1);
			Assert.That(StatsBatcher.BatchSizes.Count, Is.EqualTo(expectedBatchesPerEntity * typesOfEntities));
			using (ISession s = OpenSession())
			{
				s.BeginTransaction();
				IList users = s.CreateQuery("from User u left join fetch u.Memberships m left join fetch m.Group").List();
				foreach (object user in users)
				{
					await (s.DeleteAsync(user));
				}

				await (s.Transaction.CommitAsync());
			}
		}
	}
}
#endif
