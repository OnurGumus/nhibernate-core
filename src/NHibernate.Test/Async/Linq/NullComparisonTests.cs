#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NullComparisonTests : LinqTestCase
	{
		[Test]
		public async Task NullEqualityAsync()
		{
			string nullVariable = null;
			string nullVariable2 = null;
			string notNullVariable = "input";
			Assert.AreEqual(5, (await (session.CreateCriteria<AnotherEntity>().ListAsync<AnotherEntity>())).Count);
			IQueryable<AnotherEntity> q;
			// Null literal against itself
			q =
				from x in session.Query<AnotherEntity>()where null == null
				select x;
			ExpectAll(q);
			// Null against constants
			q =
				from x in session.Query<AnotherEntity>()where null == "value"
				select x;
			ExpectNone(q);
			q =
				from x in session.Query<AnotherEntity>()where "value" == null
				select x;
			ExpectNone(q);
			// Null against variables
			q =
				from x in session.Query<AnotherEntity>()where null == nullVariable
				select x;
			ExpectAll(q);
			q =
				from x in session.Query<AnotherEntity>()where null == notNullVariable
				select x;
			ExpectNone(q);
			q =
				from x in session.Query<AnotherEntity>()where nullVariable == null
				select x;
			ExpectAll(q);
			q =
				from x in session.Query<AnotherEntity>()where notNullVariable == null
				select x;
			ExpectNone(q);
			// Null against columns
			q =
				from x in session.Query<AnotherEntity>()where x.Input == null
				select x;
			ExpectInputIsNull(q);
			q =
				from x in session.Query<AnotherEntity>()where null == x.Input
				select x;
			ExpectInputIsNull(q);
			// All null pairings with two columns.
			q =
				from x in session.Query<AnotherEntity>()where x.Input == null && x.Output == null
				select x;
			Expect(q, BothNull);
			q =
				from x in session.Query<AnotherEntity>()where x.Input != null && x.Output == null
				select x;
			Expect(q, InputSet);
			q =
				from x in session.Query<AnotherEntity>()where x.Input == null && x.Output != null
				select x;
			Expect(q, OutputSet);
			q =
				from x in session.Query<AnotherEntity>()where x.Input != null && x.Output != null
				select x;
			Expect(q, BothSame, BothDifferent);
			// Variables against variables
			q =
				from x in session.Query<AnotherEntity>()where nullVariable == nullVariable2
				select x;
			ExpectAll(q);
			q =
				from x in session.Query<AnotherEntity>()where nullVariable == notNullVariable
				select x;
			ExpectNone(q);
			q =
				from x in session.Query<AnotherEntity>()where notNullVariable == nullVariable
				select x;
			ExpectNone(q);
			//// Variables against columns
			q =
				from x in session.Query<AnotherEntity>()where nullVariable == x.Input
				select x;
			ExpectInputIsNull(q);
			q =
				from x in session.Query<AnotherEntity>()where notNullVariable == x.Input
				select x;
			Expect(q, InputSet, BothDifferent);
			q =
				from x in session.Query<AnotherEntity>()where x.Input == nullVariable
				select x;
			ExpectInputIsNull(q);
			q =
				from x in session.Query<AnotherEntity>()where x.Input == notNullVariable
				select x;
			Expect(q, InputSet, BothDifferent);
			// Columns against columns
			q =
				from x in session.Query<AnotherEntity>()where x.Input == x.Output
				select x;
			Expect(q, BothSame);
		}
	}
}
#endif
