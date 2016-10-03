#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MultipleCollectionFetchTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultipleBagFetchFixtureAsync : AbstractMultipleCollectionFetchFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"MultipleCollectionFetchTest.PersonBag.hbm.xml"};
			}
		}

		protected override void AddToCollection(ICollection<Person> collection, Person person)
		{
			((List<Person>)collection).Add(person);
		}

		protected override ICollection<Person> CreateCollection()
		{
			return new List<Person>();
		}

		protected override async Task RunLinearJoinFetchTestAsync(Person parent)
		{
			try
			{
				await (base.RunLinearJoinFetchTestAsync(parent));
				Assert.Fail("Should have failed");
			}
			catch (QueryException e)
			{
				Assert.IsTrue(e.Message.IndexOf("Cannot simultaneously fetch multiple bags") >= 0);
			}
		}

		protected override async Task RunNonLinearJoinFetchTestAsync(Person person)
		{
			try
			{
				await (base.RunNonLinearJoinFetchTestAsync(person));
				Assert.Fail("Should have failed");
			}
			catch (QueryException e)
			{
				Assert.IsTrue(e.Message.IndexOf("Cannot simultaneously fetch multiple bags") >= 0);
			}
		}
	}
}
#endif
