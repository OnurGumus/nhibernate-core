#if NET_4_5
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2773
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task DeserializedSession_ProxyType_ShouldBeEqualToOriginalProxyTypeAsync()
		{
			System.Type originalProxyType = null;
			System.Type deserializedProxyType = null;
			ISession deserializedSession = null;
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var entity = await (session.GetAsync<MyEntity>(_entityGuid));
					originalProxyType = entity.OtherEntity.GetType();
					await (tx.CommitAsync());
				}

				using (MemoryStream sessionMemoryStream = new MemoryStream())
				{
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(sessionMemoryStream, session);
					sessionMemoryStream.Seek(0, SeekOrigin.Begin);
					deserializedSession = (ISession)formatter.Deserialize(sessionMemoryStream);
				}
			}

			using (ITransaction tx = deserializedSession.BeginTransaction())
			{
				var entity = await (deserializedSession.GetAsync<MyEntity>(_entityGuid));
				deserializedProxyType = entity.OtherEntity.GetType();
				await (tx.CommitAsync());
			}

			deserializedSession.Dispose();
			Assert.AreEqual(originalProxyType, deserializedProxyType);
		}
	}
}
#endif
