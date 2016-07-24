#if NET_4_5
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BooleanMethodExtensionExampleAsync : LinqTestCaseAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class MyLinqToHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
		{
			public MyLinqToHqlGeneratorsRegistry()
			{
				RegisterGenerator(ReflectionHelper.GetMethodDefinition(() => BooleanLinqExtensions.FreeText(null, null)), new FreetextGenerator());
			}
		}

		protected override Task ConfigureAsync(Configuration configuration)
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

		[Test, Ignore("It work only with full-text indexes enabled.")]
		public void CanUseMyCustomExtension()
		{
			List<Customer> contacts = (
				from c in db.Customers
				where c.ContactName.FreeText("Thomas")select c).ToList();
			Assert.That(contacts.Count, Is.GreaterThan(0));
			Assert.That(contacts.Select(c => c.ContactName).All(c => c.Contains("Thomas")), Is.True);
		}
	}
}
#endif
