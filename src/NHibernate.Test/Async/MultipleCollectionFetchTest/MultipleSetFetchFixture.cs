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
	public partial class MultipleSetFetchFixtureAsync : AbstractMultipleCollectionFetchFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"MultipleCollectionFetchTest.PersonSet.hbm.xml"};
			}
		}

		protected override void AddToCollection(ICollection<Person> persons, Person person)
		{
			((ISet<Person>)persons).Add(person);
		}

		protected override ICollection<Person> CreateCollection()
		{
			return new HashSet<Person>();
		}
	}
}
#endif
