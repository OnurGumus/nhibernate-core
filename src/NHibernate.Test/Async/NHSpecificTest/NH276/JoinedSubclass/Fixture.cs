﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH276.JoinedSubclass
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// Got another report in NH276 that they are still
	/// getting the error.  
	/// </summary>
	[TestFixture]
	public class FixtureAsync : TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override IList Mappings
		{
			get { return new string[] {"NHSpecificTest.NH276.JoinedSubclass.Mappings.hbm.xml"}; }
		}

		[Test]
		public async Task ManyToOneIdPropertiesAsync()
		{
			Organization org = new Organization();
			org.OrganizationId = 5;
			org.Name = "the org";

			Status stat = new Status();
			stat.StatusId = 4;
			stat.Name = "the stat";

			Request r = new Request();
			r.Extra = "extra";
			r.Office = org;
			r.Status = stat;

			ISession s = OpenSession();
			await (s.SaveAsync(org, CancellationToken.None));
			await (s.SaveAsync(stat, CancellationToken.None));
			await (s.SaveAsync(r, CancellationToken.None));

			await (s.FlushAsync(CancellationToken.None));
			s.Close();

			s = OpenSession();
			ICriteria c = s.CreateCriteria(typeof(Request));
			c.Add(Expression.Eq("Status.StatusId", 1));
			c.Add(Expression.Eq("Office.OrganizationId", 1));
			IList list = await (c.ListAsync(CancellationToken.None));

			Assert.AreEqual(0, list.Count, "should contain no results");

			c = s.CreateCriteria(typeof(Request));
			c.Add(Expression.Eq("Status.StatusId", 4));
			c.Add(Expression.Eq("Office.OrganizationId", 5));
			list = await (c.ListAsync(CancellationToken.None));

			Assert.AreEqual(1, list.Count, "one matching result");

			r = list[0] as Request;
			await (s.DeleteAsync(r, CancellationToken.None));
			await (s.DeleteAsync(r.Status, CancellationToken.None));
			await (s.DeleteAsync(r.Office, CancellationToken.None));
			await (s.FlushAsync(CancellationToken.None));
			s.Close();
		}
	}
}