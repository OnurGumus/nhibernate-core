#if NET_4_5
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3731
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixture : TestCaseMappingByCode
	{
		[Test]
		public async Task Serializing_Session_After_Reordering_ChildrenList_Should_WorkAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var p = session.Query<Parent>().Single();
					var c = p.ChildrenList.Last();
					p.ChildrenList.Remove(c);
					p.ChildrenList.Insert(p.ChildrenList.Count - 1, c);
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}

				using (MemoryStream stream = new MemoryStream())
				{
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(stream, session);
					Assert.AreNotEqual(0, stream.Length);
				}
			}
		}

		[Test]
		public async Task Serializing_Session_After_Deleting_First_Child_In_List_Should_WorkAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var p = session.Query<Parent>().Single();
					p.ChildrenList.RemoveAt(0);
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}

				using (MemoryStream stream = new MemoryStream())
				{
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(stream, session);
					Assert.AreNotEqual(0, stream.Length);
				}
			}
		}

		[Test]
		public async Task Serializing_Session_After_Changing_Key_ChildrenMap_Should_WorkAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var p = session.Query<Parent>().Single();
					var firstChild = p.ChildrenMap["first"];
					var secondChild = p.ChildrenMap["second"];
					p.ChildrenMap.Remove("first");
					p.ChildrenMap.Remove("second");
					p.ChildrenMap.Add("first", secondChild);
					p.ChildrenMap.Add("second", firstChild);
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}

				using (MemoryStream stream = new MemoryStream())
				{
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(stream, session);
					Assert.AreNotEqual(0, stream.Length);
				}
			}
		}
	}
}
#endif
