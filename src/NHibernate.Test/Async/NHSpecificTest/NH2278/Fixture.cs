#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2278
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				await (s.DeleteAsync("from CustomA"));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task CustomIdBagAsync()
		{
			CustomA a = new CustomA();
			a.Name = "first generic type";
			a.Items = new CustomList<string>();
			a.Items.Add("first string");
			a.Items.Add("second string");
			ISession s = OpenSession();
			await (s.SaveOrUpdateAsync(a));
			await (s.FlushAsync());
			s.Close();
			Assert.That(a.Id, Is.Not.Null);
			Assert.That(a.Items[0], Is.StringMatching("first string"));
			s = OpenSession();
			a = await (s.LoadAsync<CustomA>(a.Id));
			Assert.That(a.Items, Is.InstanceOf<CustomPersistentIdentifierBag<string>>());
			Assert.That(a.Items[0], Is.StringMatching("first string"), "first item should be 'first string'");
			Assert.That(a.Items[1], Is.StringMatching("second string"), "second item should be 'second string'");
			// ensuring the correct generic type was constructed
			a.Items.Add("third string");
			Assert.That(a.Items.Count, Is.EqualTo(3), "3 items in the list now");
			a.Items[1] = "new second string";
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
