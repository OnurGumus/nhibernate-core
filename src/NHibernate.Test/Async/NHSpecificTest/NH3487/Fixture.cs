#if NET_4_5
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3487
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanDeserializeSessionWithEntityHashCollisionAsync()
		{
			IFormatter formatter = new BinaryFormatter();
			byte[] serializedSessionArray;
			using (ISession session = OpenSession())
			{
				using (session.BeginTransaction())
				{
					await (session.GetAsync<Entity>(_key1));
					await (session.GetAsync<Entity>(_key2));
				}

				session.Disconnect();
				using (var serializationStream = new MemoryStream())
				{
					formatter.Serialize(serializationStream, session);
					serializationStream.Close();
					serializedSessionArray = serializationStream.ToArray();
				}
			}

			using (var serializationStream = new MemoryStream(serializedSessionArray))
			{
				formatter.Deserialize(serializationStream);
			}
		}
	}
}
#endif
