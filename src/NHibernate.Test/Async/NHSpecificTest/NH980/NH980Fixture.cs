﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;

using NHibernate.Cfg;

using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH980
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class NH980FixtureAsync : BugTestCase
	{
		[Test]
		public async Task IdGeneratorShouldUseQuotedTableNameAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				IdOnly obj = new IdOnly();
				await (s.SaveAsync(obj, CancellationToken.None));
				await (s.FlushAsync(CancellationToken.None));
				await (s.DeleteAsync(obj, CancellationToken.None));
				await (t.CommitAsync(CancellationToken.None));
			}
		}
	}
}
