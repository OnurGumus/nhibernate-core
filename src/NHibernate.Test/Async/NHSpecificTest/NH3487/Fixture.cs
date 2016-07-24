#if NET_4_5
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3487
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private Key _key1;
		private Key _key2;
		public override string BugNumber
		{
			get
			{
				return "NH3487";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					_key1 = new Key{Id = 1};
					var entity1 = new Entity{Id = _key1, Name = "Bob1"};
					await (session.SaveAsync(entity1));
					_key2 = new Key{Id = 2};
					var entity2 = new Entity{Id = _key2, Name = "Bob2"};
					await (session.SaveAsync(entity2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
			}
		}

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
