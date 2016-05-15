#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MultipleCollectionFetchTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultipleBagFetchFixture : AbstractMultipleCollectionFetchFixture
	{
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
