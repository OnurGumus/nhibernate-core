#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2459
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
			{
				SkillSet skillSet = new SkillSet()
				{Code = "123", Title = "Skill Set"};
				Qualification qualification = new Qualification()
				{Code = "124", Title = "Qualification"};
				await (session.SaveAsync(skillSet));
				await (session.SaveAsync(qualification));
				await (session.FlushAsync());
			}
		}

		[Test]
		public void IsTypeOperator()
		{
			using (ISession session = OpenSession())
			{
				//first query is OK
				IQueryable<TrainingComponent> query = session.Query<TrainingComponent>().Where(c => c is SkillSet);
				Assert.That(!query.ToList().Any(c => !(c is SkillSet)));
				//Second time round the a cached version of the SQL for the query is used BUT the type parameter is not updated... 
				query = session.Query<TrainingComponent>().Where(c => c is Qualification);
				Assert.That(!query.ToList().Any(c => !(c is Qualification)));
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from TrainingComponent"));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
