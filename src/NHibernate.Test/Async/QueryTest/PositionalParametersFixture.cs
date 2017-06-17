﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NUnit.Framework;

namespace NHibernate.Test.QueryTest
{
	using System.Threading.Tasks;
	/// <summary>
	/// Summary description for PositionalParametersFixture.
	/// </summary>
	[TestFixture]
	public class PositionalParametersFixtureAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] {"Simple.hbm.xml"}; }
		}

		[Test]
		public void TestMissingHQLParametersAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();

			try
			{
				IQuery q = s.CreateQuery("from s in class Simple where s.Name=? and s.Count=?");
				// Set the first property, but not the second
				q.SetParameter(0, "Fred");

				// Try to execute it
				Assert.ThrowsAsync<QueryException>(() => q.ListAsync());
			}
			finally
			{
				t.Rollback();
				s.Close();
			}
		}

		[Test]
		public void TestMissingHQLParameters2Async()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();

			try
			{
				IQuery q = s.CreateQuery("from s in class Simple where s.Name=? and s.Count=?");
				// Set the second property, but not the first - should give a nice not found at position xxx error
				q.SetParameter(1, "Fred");

				// Try to execute it
				Assert.ThrowsAsync<QueryException>(() => q.ListAsync());
			}
			finally
			{
				t.Rollback();
				s.Close();
			}
		}
	}
}