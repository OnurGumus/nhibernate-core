#if NET_4_5
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2880
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
	}
}
#endif
