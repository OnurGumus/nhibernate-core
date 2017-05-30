﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NUnit.Framework;
using System;

namespace NHibernate.Test.NHSpecificTest.NH3985
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// The test verifies that subsequent child sessions are not issued in already-disposed state.
	/// </summary>
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public void GetChildSession_ShouldReturnNonDisposedInstanceAsync()
		{
			using (var rootSession = OpenSession())
			{
				using (var childSession1 = rootSession.GetChildSession())
				{
				}

				using (var childSession2 = rootSession.GetChildSession())
				{
					Assert.DoesNotThrowAsync(() => { return childSession2.GetAsync<Process>(Guid.NewGuid(), CancellationToken.None); });
				}
			}
		}

		[Test]
		public void GetChildSession_ShouldReturnNonClosedInstanceAsync()
		{
			using (var rootSession = OpenSession())
			{
				var childSession1 = rootSession.GetChildSession();
				childSession1.Close();

				using (var childSession2 = rootSession.GetChildSession())
				{
					Assert.DoesNotThrowAsync(() => { return childSession2.GetAsync<Process>(Guid.NewGuid(), CancellationToken.None); });
				}
			}
		}
	}
}
