#if NET_4_5
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH317
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest.NH317.Node.hbm.xml"};
			}
		}

		[Test]
		public async Task ProxySerializationAsync()
		{
			Node node = new Node();
			node.Id = 1;
			node.Name = "Node 1";
			ISession s = sessions.OpenSession();
			await (s.SaveAsync(node));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			Node nodeProxy = (Node)await (s.LoadAsync(typeof (Node), 1));
			// Test if it is really a proxy
			Assert.IsTrue(nodeProxy is INHibernateProxy);
			s.Close();
			// Serialize
			IFormatter formatter = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			formatter.Serialize(ms, nodeProxy);
			// Deserialize
			ms.Seek(0, SeekOrigin.Begin);
			Node deserializedNodeProxy = (Node)formatter.Deserialize(ms);
			ms.Close();
			// Deserialized proxy should implement the INHibernateProxy interface.
			Assert.IsTrue(deserializedNodeProxy is INHibernateProxy);
			s = OpenSession();
			await (s.DeleteAsync("from Node"));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
