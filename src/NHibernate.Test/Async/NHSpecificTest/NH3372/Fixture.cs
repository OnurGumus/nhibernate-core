#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3372
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanGeneratePropertyOnInsertOfEntityWithCustomLoaderAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var entity = new Entity{Content = "Some text"};
					await (session.SaveAsync(entity));
					await (session.FlushAsync());
					Assert.That(entity.ShardId, Is.Not.Null & Has.Length.GreaterThan(0));
				}
		}

		[Test]
		public async Task CanGeneratePropertyOnUpdateOfEntityWithCustomLoaderAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var entity = new Entity{Content = "Some text"};
					await (session.SaveAsync(entity));
					await (session.FlushAsync());
					entity.ShardId = null;
					entity.Content = "Some other text";
					await (session.UpdateAsync(entity));
					await (session.FlushAsync());
					Assert.That(entity.ShardId, Is.Not.Null & Has.Length.GreaterThan(0));
				}
		}
	}
}
#endif
