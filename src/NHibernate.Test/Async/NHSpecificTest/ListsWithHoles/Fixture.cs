#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.ListsWithHoles
{
	using System.Collections;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task CanHandleHolesInListAsync()
		{
			int parentId, firstChildId;
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					Employee e = new Employee();
					e.Children.Add(new Employee());
					e.Children.Add(new Employee());
					await (sess.SaveAsync(e));
					await (tx.CommitAsync());
					parentId = e.Id;
					firstChildId = e.Children[0].Id;
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					await (sess.DeleteAsync(await (sess.GetAsync<Employee>(firstChildId))));
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					Employee employee = await (sess.GetAsync<Employee>(parentId));
					employee.Children.Add(new Employee());
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					await (sess.DeleteAsync("from Employee"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
