#if NET_4_5
using System.Collections;
using NHibernate.Classic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Classic
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ValidatableFixture : TestCase
	{
		[Test]
		public async Task SaveAsync()
		{
			try
			{
				using (ISession s = OpenSession())
				{
					await (s.SaveAsync(new Video()));
					await (s.FlushAsync());
				}

				Assert.Fail("Saved an invalid entity");
			}
			catch (ValidationFailure)
			{
			// Ok
			}

			Video v = new Video("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.DeleteAsync(v));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task UpdateAsync()
		{
			Video v = new Video("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
			}

			int savedId = v.Id;
			// update detached
			v.Heigth = 0;
			try
			{
				using (ISession s = OpenSession())
				{
					await (s.UpdateAsync(v));
					await (s.FlushAsync());
				}

				Assert.Fail("Updated an invalid entity");
			}
			catch (ValidationFailure)
			{
			// Ok
			}

			// update in the same session
			using (ISession s = OpenSession())
			{
				Video vu = (Video)s.Get(typeof (Video), savedId);
				vu.Width = 0;
				try
				{
					await (s.UpdateAsync(vu));
					await (s.FlushAsync());
					Assert.Fail("Updated an invalid entity");
				}
				catch (ValidationFailure)
				{
				//Ok
				}
			}

			// cleanup
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync(v));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task MergeAsync()
		{
			Video v = new Video("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
			}

			v.Heigth = 0;
			try
			{
				using (ISession s = OpenSession())
				{
					s.Merge(v);
					await (s.FlushAsync());
				}

				Assert.Fail("Updated an invalid entity");
			}
			catch (ValidationFailure)
			{
			// Ok
			}

			Video v1 = new Video("Shinobi", 0, 10);
			try
			{
				using (ISession s = OpenSession())
				{
					s.Merge(v1);
					await (s.FlushAsync());
				}

				Assert.Fail("saved an invalid entity");
			}
			catch (ValidationFailure)
			{
			// Ok
			}

			// cleanup
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Video"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task DeleteAsync()
		{
			Video v = new Video("Shinobi", 10, 10);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(v));
				await (s.FlushAsync());
				// Validatable not called in deletation
				await (s.DeleteAsync(v));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
