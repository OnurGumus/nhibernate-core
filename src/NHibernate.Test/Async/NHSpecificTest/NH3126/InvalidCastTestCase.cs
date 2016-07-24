#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3126
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InvalidCastWithGenericDictionaryOnCascadeTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestAsync()
		{
			var property = new Property{Name = "Property 1"};
			using (var session = OpenSession())
			{
				using (var tx = session.BeginTransaction())
				{
					await (session.SaveAsync(property));
					await (tx.CommitAsync());
				}

				var item = new Item();
				using (var tx = session.BeginTransaction())
				{
					item.Name = "Item 1";
					item.PropertyValues = new Dictionary<Guid, PropertyValue>{{property.Id, new PropertyValue{Value = "Value 1"}}};
					await (session.SaveAsync(item));
					await (tx.CommitAsync());
				}

				session.Clear();
				var savedItem = await (session.GetAsync<Item>(item.Id));
				Assert.AreEqual(1, savedItem.PropertyValues.Count);
				Assert.AreEqual("Value 1", savedItem.PropertyValues[property.Id].Value);
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
			{
				await (session.DeleteAsync("from System.Object"));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
