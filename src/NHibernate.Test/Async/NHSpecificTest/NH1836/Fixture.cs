#if NET_4_5
using System.Collections;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1836
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task AliasToBeanTransformerShouldApplyCorrectlyToMultiQueryAsync()
		{
			try
			{
				AliasToBeanTransformerShouldApplyCorrectlyToMultiQuery();
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
