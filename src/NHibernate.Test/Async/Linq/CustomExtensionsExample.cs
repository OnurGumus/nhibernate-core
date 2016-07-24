#if NET_4_5
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomExtensionsExampleAsync : LinqTestCaseAsync
	{
		protected override Task ConfigureAsync(NHibernate.Cfg.Configuration configuration)
		{
			try
			{
				configuration.LinqToHqlGeneratorsRegistry<MyLinqToHqlGeneratorsRegistry>();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void CanUseMyCustomExtension()
		{
			var contacts = (
				from c in db.Customers
				where c.ContactName.IsLike("%Thomas%")select c).ToList();
			Assert.That(contacts.Count, Is.GreaterThan(0));
			Assert.That(contacts.All(c => c.ContactName.Contains("Thomas")), Is.True);
		}
	}
}
#endif
