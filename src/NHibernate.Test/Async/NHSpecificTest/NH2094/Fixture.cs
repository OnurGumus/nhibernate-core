#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2094
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanAccessInitializedPropertiesOutsideOfSessionAsync()
		{
			try
			{
				using (var s = OpenSession())
				{
					var p = new Person{Id = 1, Name = "Person1", LazyField = "Long field"};
					await (s.SaveAsync(p));
					await (s.FlushAsync());
				}

				Person person;
				using (var s = OpenSession())
				{
					person = await (s.GetAsync<Person>(1));
					Assert.AreEqual("Person1", person.Name);
					Assert.AreEqual("Long field", person.LazyField);
				}

				Assert.AreEqual("Person1", person.Name);
				Assert.AreEqual("Long field", person.LazyField);
			}
			finally
			{
				using (var s = OpenSession())
				{
					await (s.DeleteAsync("from Person"));
					await (s.FlushAsync());
				}
			}
		}

		[Test]
		public async Task WhenAccessNoLazyPropertiesOutsideOfSessionThenNotThrowsAsync()
		{
			try
			{
				using (var s = OpenSession())
				{
					var p = new Person{Id = 1, Name = "Person1", LazyField = "Long field"};
					await (s.SaveAsync(p));
					await (s.FlushAsync());
				}

				Person person;
				using (var s = OpenSession())
				{
					person = await (s.GetAsync<Person>(1));
				}

				string personName;
				Assert.That(() => personName = person.Name, Throws.Nothing);
			}
			finally
			{
				using (var s = OpenSession())
				{
					await (s.DeleteAsync("from Person"));
					await (s.FlushAsync());
				}
			}
		}

		[Test]
		public async Task WhenAccessLazyPropertiesOutsideOfSessionThenThrowsAsync()
		{
			try
			{
				using (var s = OpenSession())
				{
					var p = new Person{Id = 1, Name = "Person1", LazyField = "Long field"};
					await (s.SaveAsync(p));
					await (s.FlushAsync());
				}

				Person person;
				using (var s = OpenSession())
				{
					person = await (s.GetAsync<Person>(1));
				}

				string lazyField;
				var lazyException = Assert.Throws<LazyInitializationException>(() => lazyField = person.LazyField);
				Assert.That(lazyException.EntityName, Is.Not.Null);
				Assert.That(lazyException.Message, Is.StringContaining("LazyField"));
			}
			finally
			{
				using (var s = OpenSession())
				{
					await (s.DeleteAsync("from Person"));
					await (s.FlushAsync());
				}
			}
		}
	}
}
#endif
