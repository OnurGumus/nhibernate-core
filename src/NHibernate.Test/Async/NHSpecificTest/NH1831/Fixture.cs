﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1831
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !(dialect is Oracle8iDialect);
		}

		[Test]
		public async Task CorrectPrecedenceForBitwiseOperatorsAsync()
		{
			using (var s = OpenSession())
			using (s.BeginTransaction())
			{
				const string hql = @"SELECT dt FROM DocumentType dt WHERE dt.systemAction & :sysAct = :sysAct ";

				await (s.CreateQuery(hql).SetParameter("sysAct", SystemAction.Denunciation).ListAsync(CancellationToken.None));
			}
		}
	}
}
