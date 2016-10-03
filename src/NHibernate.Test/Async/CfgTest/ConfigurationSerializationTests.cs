#if NET_4_5
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Cfg;
using NHibernate.DomainModel;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.CfgTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ConfigurationSerializationTestsAsync
	{
		[Test]
		public async Task Basic_CRUD_should_workAsync()
		{
			Assembly assembly = Assembly.Load("NHibernate.DomainModel");
			var cfg = new Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
			{
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
			}

			cfg.AddResource("NHibernate.DomainModel.ParentChild.hbm.xml", assembly);
			var formatter = new BinaryFormatter();
			var memoryStream = new MemoryStream();
			formatter.Serialize(memoryStream, cfg);
			memoryStream.Position = 0;
			cfg = formatter.Deserialize(memoryStream) as Configuration;
			Assert.That(cfg, Is.Not.Null);
			var export = new SchemaExport(cfg);
			await (export.ExecuteAsync(true, true, false));
			ISessionFactory sf = cfg.BuildSessionFactory();
			using (ISession session = sf.OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					var parent = new Parent();
					var child = new Child();
					parent.Child = child;
					parent.X = 9;
					parent.Count = 5;
					child.Parent = parent;
					child.Count = 3;
					child.X = 4;
					await (session.SaveAsync(parent));
					await (session.SaveAsync(child));
					await (tran.CommitAsync());
				}
			}

			using (ISession session = sf.OpenSession())
			{
				var parent = await (session.GetAsync<Parent>(1L));
				Assert.That(parent.Count, Is.EqualTo(5));
				Assert.That(parent.X, Is.EqualTo(9));
				Assert.That(parent.Child, Is.Not.Null);
				Assert.That(parent.Child.X, Is.EqualTo(4));
				Assert.That(parent.Child.Count, Is.EqualTo(3));
				Assert.That(parent.Child.Parent, Is.EqualTo(parent));
			}

			using (ISession session = sf.OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					var p = await (session.GetAsync<Parent>(1L));
					var c = await (session.GetAsync<Child>(1L));
					await (session.DeleteAsync(c));
					await (session.DeleteAsync(p));
					await (tran.CommitAsync());
				}
			}

			using (ISession session = sf.OpenSession())
			{
				var p = await (session.GetAsync<Parent>(1L));
				Assert.That(p, Is.Null);
			}

			await (export.DropAsync(true, true));
		}
	}
}
#endif
