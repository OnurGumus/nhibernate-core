#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2331
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Nh2331Test : BugTestCase
	{
		[Test]
		public Task DetachedCriteriaCorrelatedQueryExplodesAsync()
		{
			try
			{
				DetachedCriteriaCorrelatedQueryExplodes();
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
