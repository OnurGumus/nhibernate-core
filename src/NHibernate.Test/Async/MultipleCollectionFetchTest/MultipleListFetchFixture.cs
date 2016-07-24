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
	public partial class MultipleListFetchFixtureAsync : AbstractMultipleCollectionFetchFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"MultipleCollectionFetchTest.PersonList.hbm.xml"};
			}
		}

		protected override void AddToCollection(ICollection<Person> persons, Person person)
		{
			((List<Person>)persons).Add(person);
		}

		protected override ICollection<Person> CreateCollection()
		{
			return new List<Person>();
		}
	}
}
#endif
