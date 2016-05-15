#if NET_4_5
using System;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2386
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Test : BugTestCase
	{
		[Test]
		public async Task TheTestAsync()
		{
			using (ISession session = OpenSession())
			{
				var organisation = new Organisation();
				await (session.SaveOrUpdateAsync(organisation));
				await (session.FlushAsync());
				organisation.TradingNames.Add(new TradingName(organisation)
				{Name = "Trading Name", StartDate = DateTime.Today});
				await (session.SaveOrUpdateAsync(organisation));
				//this line below fails 
				//AbstractBatcher:0 - Could not execute command: UPDATE tblTrnOrganisation SET  WHERE OrganisationId = @p0 AND RVersion = @p1
				//System.Data.SqlClient.SqlException: Incorrect syntax near the keyword 'WHERE'.
				await (session.FlushAsync());
				await (session.DeleteAsync(organisation));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
