#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1054
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH1054FixtureAsync
	{
		[Test]
		public void SettingsTransactionFactoryReturnsConfiguredTransactionFactory()
		{
			Configuration configuration = new Configuration();
			configuration.Properties[Cfg.Environment.TransactionStrategy] = "NHibernate.Test.NHSpecificTest.NH1054.DummyTransactionFactory, " + this.GetType().Assembly.FullName;
			ISessionFactoryImplementor sessionFactory = (ISessionFactoryImplementor)configuration.BuildSessionFactory();
			Assert.That(sessionFactory.Settings.TransactionFactory, Is.InstanceOf<DummyTransactionFactory>());
		}

		[Test]
		public void AdoNetWithDistributedTransactionFactoryIsDefaultTransactionFactory()
		{
			Configuration configuration = new Configuration();
			ISessionFactoryImplementor sessionFactory = (ISessionFactoryImplementor)configuration.BuildSessionFactory();
			Assert.That(sessionFactory.Settings.TransactionFactory, Is.InstanceOf<NHibernate.Transaction.AdoNetWithDistributedTransactionFactory>());
		}
	}
}
#endif
