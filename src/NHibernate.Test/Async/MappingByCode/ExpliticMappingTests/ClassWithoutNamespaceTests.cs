#if NET_4_5
using System;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.MappingByCode.ExpliticMappingTests
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ClassWithoutNamespaceTestsAsync
	{
		[SetUp]
		public void OnSetUp()
		{
			Assert.That(typeof (EntityNH3615).Namespace, Is.Null);
		}

		[Test]
		public Task ShouldBeAbleToMapClassWithoutNamespaceAsync()
		{
			try
			{
				var mapper = new ModelMapper();
				mapper.Class<EntityNH3615>(rc =>
				{
					rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
					rc.Property(x => x.Name);
				}

				);
				Assert.DoesNotThrow(() => mapper.CompileMappingForAllExplicitlyAddedEntities());
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
