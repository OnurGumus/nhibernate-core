﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Text;
using NHibernate.Test.NHSpecificTest;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.HqlOnMapWithForumula
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		public override string BugNumber
		{
			get { return "HqlOnMapWithForumula"; }
		}


		[Test]
		public async Task TestBugAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				await (s.CreateQuery("from A a where 1 in elements(a.MyMaps)")
					.ListAsync());
			}
		}
	}
}