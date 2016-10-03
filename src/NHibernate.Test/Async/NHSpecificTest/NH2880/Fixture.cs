#if NET_4_5
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2880
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private Guid _id;
		protected override async Task OnSetUpAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					Entity1 e1 = new Entity1();
					Entity2 e2 = new Entity2();
					e1.Entity2 = e2;
					e2.Text = "Text";
					await (s.SaveAsync(e1));
					await (s.SaveAsync(e2));
					_id = e1.Id;
					await (t.CommitAsync());
				}
			}
		}

		[Test]
		public async Task ProxiesFromDeserializedSessionsCanBeLoadedAsync()
		{
			MemoryStream sessionMemoryStream;
			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					Entity1 e = await (s.GetAsync<Entity1>(_id));
					await (t.CommitAsync());
				}

				sessionMemoryStream = new MemoryStream();
				BinaryFormatter writer = new BinaryFormatter();
				writer.Serialize(sessionMemoryStream, s);
			}

			sessionMemoryStream.Seek(0, SeekOrigin.Begin);
			BinaryFormatter reader = new BinaryFormatter();
			ISession restoredSession = (ISession)reader.Deserialize(sessionMemoryStream);
			Entity1 e1 = await (restoredSession.GetAsync<Entity1>(_id));
			Entity2 e2 = e1.Entity2;
			Assert.IsNotNull(e2);
			Assert.AreEqual("Text", e2.Text);
			restoredSession.Dispose();
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Entity1"));
					await (s.DeleteAsync("from Entity2"));
					await (t.CommitAsync());
				}
			}
		}
	}
}
#endif
