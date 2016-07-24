#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.MappingExceptions
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MissingDefCtorFixtureAsync
	{
		[Test]
		public Task ClassMissingDefaultCtorAsync()
		{
			try
			{
				// add a resource that doesn't exist
				string resource = "NHibernate.Test.MappingExceptions.MissingDefCtor.hbm.xml";
				Configuration cfg = new Configuration();
				cfg.AddResource(resource, this.GetType().Assembly);
				Assert.Throws<InstantiationException>(() => cfg.BuildSessionFactory());
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
