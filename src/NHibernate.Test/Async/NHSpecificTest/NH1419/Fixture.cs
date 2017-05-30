﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1419
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class TestsAsync : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			using (ISession session = OpenSession())
			{
				ITransaction transaction = session.BeginTransaction();

				Blog blog = new Blog();
				blog.Name = "Test Blog 1";

				Entry entry = new Entry();
				entry.Subject = "Test Entry 1";

				blog.AddEntry(entry);

				await (session.SaveOrUpdateAsync(blog, CancellationToken.None));

				await (transaction.CommitAsync(CancellationToken.None));
			}
			using (ISession session = OpenSession())
			{
				ITransaction transaction = session.BeginTransaction();
				await (session.DeleteAsync("from Blog", CancellationToken.None));
				await (transaction.CommitAsync(CancellationToken.None));
			}
		}

		[Test]
		public async Task WithEmptyCollectionAsync()
		{
			using (ISession session = OpenSession())
			{
				ITransaction transaction = session.BeginTransaction();

				Blog blog = new Blog();
				blog.Name = "Test Blog 1";

				await (session.SaveOrUpdateAsync(blog, CancellationToken.None));

				await (transaction.CommitAsync(CancellationToken.None));
			}
			using (ISession session = OpenSession())
			{
				ITransaction transaction = session.BeginTransaction();
				await (session.DeleteAsync("from Blog", CancellationToken.None));
				await (transaction.CommitAsync(CancellationToken.None));
			}
		}
	}
}
