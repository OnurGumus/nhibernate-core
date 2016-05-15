#if NET_4_5
using System;
using System.Collections;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SessionCacheTest : TestCase
	{
		[Test]
		public async Task MakeCollectionTransientAsync()
		{
			ISession fixture = OpenSession();
			for (long i = 1L; i < 6L; i++)
			{
				Simple s = new Simple((int)i);
				s.Address = "dummy collection address " + i;
				s.Date = DateTime.Now;
				s.Name = "dummy collection name " + i;
				s.Pay = i * 1279L;
				await (fixture.SaveAsync(s, i));
			}

			await (fixture.FlushAsync());
			IList list = await (fixture.CreateCriteria(typeof (Simple)).ListAsync());
			Assert.IsNotNull(list);
			Assert.IsTrue(list.Count == 5);
			Assert.IsTrue(await (fixture.ContainsAsync(list[2])));
			fixture.Clear();
			Assert.IsTrue(list.Count == 5);
			Assert.IsFalse(await (fixture.ContainsAsync(list[2])));
			await (fixture.FlushAsync());
			Assert.IsTrue(list.Count == 5);
			await (fixture.DeleteAsync("from System.Object o"));
			await (fixture.FlushAsync());
			fixture.Close();
		}

		[Test]
		public async Task LoadAfterNotExistsAsync()
		{
			ISession fixture = OpenSession();
			// First, prime the fixture session to think the entity does not exist
			try
			{
				await (fixture.LoadAsync(typeof (Simple), -1L));
			}
			catch (ObjectNotFoundException)
			{
			// this is expected
			}

			// Next, lets create that entity under the covers
			ISession anotherSession = null;
			try
			{
				anotherSession = OpenSession();
				Simple oneSimple = new Simple(1);
				oneSimple.Name = "hidden entity";
				oneSimple.Address = "SessionCacheTest.LoadAfterNotExists";
				oneSimple.Date = DateTime.Now;
				oneSimple.Pay = 1000000f;
				await (anotherSession.SaveAsync(oneSimple, -1L));
				await (anotherSession.FlushAsync());
			}
			finally
			{
				QuietlyClose(anotherSession);
			}

			// Verify that the original session is still unable to see the new entry...
			try
			{
				await (fixture.LoadAsync(typeof (Simple), -1L));
			}
			catch (ObjectNotFoundException)
			{
			}

			// Now, lets clear the original session at which point it should be able to see the new entity
			fixture.Clear();
			string failedMessage = "Unable to load entity with id = -1.";
			try
			{
				Simple dummy = await (fixture.LoadAsync(typeof (Simple), -1L)) as Simple;
				Assert.IsNotNull(dummy, failedMessage);
				await (fixture.DeleteAsync(dummy));
				await (fixture.FlushAsync());
			}
			catch (ObjectNotFoundException)
			{
				Assert.Fail(failedMessage);
			}
			finally
			{
				QuietlyClose(fixture);
			}
		}
	}
}
#endif
