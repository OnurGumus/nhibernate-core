#if NET_4_5
using NHibernate.Dialect;
using NHibernate.Id;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.IdTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IdentifierGeneratorFactoryFixtureAsync
	{
		/// <summary>
		/// Testing NH-325 to make sure that an exception is actually
		/// caught with a bad class name passed in.
		/// </summary>
		[Test]
		public Task NonCreatableStrategyAsync()
		{
			try
			{
				Assert.Throws<IdentifierGenerationException>(() => IdentifierGeneratorFactory.Create("Guid", NHibernateUtil.Guid, null, new MsSql2000Dialect()), "Could not interpret id generator strategy: Guid");
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
