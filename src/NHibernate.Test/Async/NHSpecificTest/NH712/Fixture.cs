#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH712
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public Task BugAsync()
		{
			try
			{
				if (!Cfg.Environment.UseReflectionOptimizer)
				{
					Assert.Ignore("Test only works with reflection optimization enabled");
				}
				else
					Assert.Throws<InstantiationException>(() => new Configuration().AddResource(GetType().Namespace + ".Mappings.hbm.xml", GetType().Assembly).BuildSessionFactory());
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
