#if NET_4_5
using System;
using System.Collections.Generic;
using log4net;
using log4net.Core;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.IdTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AssignedFixture : IdFixtureBase
	{
		[Test]
		public async Task SaveOrUpdate_SaveAsync()
		{
			using (LogSpy ls = new LogSpy(LogManager.GetLogger("NHibernate"), Level.Warn))
				using (ISession s = OpenSession())
				{
					ITransaction t = s.BeginTransaction();
					Parent parent = new Parent()
					{Id = "parent", Children = new List<Child>(), };
					s.SaveOrUpdate(parent);
					await (t.CommitAsync());
					long actual = s.CreateQuery("select count(p) from Parent p").UniqueResult<long>();
					Assert.That(actual, Is.EqualTo(1));
					string[] warnings = GetAssignedIdentifierWarnings(ls);
					Assert.That(warnings.Length, Is.EqualTo(1));
					Assert.IsTrue(warnings[0].Contains("parent"));
				}
		}

		[Test]
		public async Task SaveNoWarningAsync()
		{
			using (LogSpy ls = new LogSpy(LogManager.GetLogger("NHibernate"), Level.Warn))
				using (ISession s = OpenSession())
				{
					ITransaction t = s.BeginTransaction();
					Parent parent = new Parent()
					{Id = "parent", Children = new List<Child>(), };
					await (s.SaveAsync(parent));
					await (t.CommitAsync());
					long actual = s.CreateQuery("select count(p) from Parent p").UniqueResult<long>();
					Assert.That(actual, Is.EqualTo(1));
					string[] warnings = GetAssignedIdentifierWarnings(ls);
					Assert.That(warnings.Length, Is.EqualTo(0));
				}
		}

		[Test]
		public async Task SaveOrUpdate_UpdateAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();
				await (s.SaveAsync(new Parent()
				{Id = "parent", Name = "before"}));
				await (t.CommitAsync());
			}

			using (LogSpy ls = new LogSpy(LogManager.GetLogger("NHibernate"), Level.Warn))
				using (ISession s = OpenSession())
				{
					ITransaction t = s.BeginTransaction();
					Parent parent = new Parent()
					{Id = "parent", Name = "after", };
					s.SaveOrUpdate(parent);
					await (t.CommitAsync());
					string[] warnings = GetAssignedIdentifierWarnings(ls);
					Assert.That(warnings.Length, Is.EqualTo(1));
					Assert.IsTrue(warnings[0].Contains("parent"));
				}

			using (ISession s = OpenSession())
			{
				Parent parent = s.CreateQuery("from Parent").UniqueResult<Parent>();
				Assert.That(parent.Name, Is.EqualTo("after"));
			}
		}

		[Test]
		public async Task UpdateNoWarningAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();
				await (s.SaveAsync(new Parent()
				{Id = "parent", Name = "before"}));
				await (t.CommitAsync());
			}

			using (LogSpy ls = new LogSpy(LogManager.GetLogger("NHibernate"), Level.Warn))
				using (ISession s = OpenSession())
				{
					ITransaction t = s.BeginTransaction();
					Parent parent = new Parent()
					{Id = "parent", Name = "after", };
					await (s.UpdateAsync(parent));
					await (t.CommitAsync());
					string[] warnings = GetAssignedIdentifierWarnings(ls);
					Assert.That(warnings.Length, Is.EqualTo(0));
				}

			using (ISession s = OpenSession())
			{
				Parent parent = s.CreateQuery("from Parent").UniqueResult<Parent>();
				Assert.That(parent.Name, Is.EqualTo("after"));
			}
		}

		[Test]
		public async Task InsertCascadeAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();
				await (s.SaveAsync(new Child()
				{Id = "detachedChild"}));
				await (t.CommitAsync());
			}

			using (LogSpy ls = new LogSpy(LogManager.GetLogger("NHibernate"), Level.Warn))
				using (ISession s = OpenSession())
				{
					ITransaction t = s.BeginTransaction();
					Parent parent = new Parent()
					{Id = "parent", Children = new List<Child>(), };
					parent.Children.Add(new Child()
					{Id = "detachedChild", Parent = parent});
					parent.Children.Add(new Child()
					{Id = "transientChild", Parent = parent});
					await (s.SaveAsync(parent));
					await (t.CommitAsync());
					long actual = s.CreateQuery("select count(c) from Child c").UniqueResult<long>();
					Assert.That(actual, Is.EqualTo(2));
					string[] warnings = GetAssignedIdentifierWarnings(ls);
					Assert.That(warnings.Length, Is.EqualTo(2));
					Assert.IsTrue(warnings[0].Contains("detachedChild"));
					Assert.IsTrue(warnings[1].Contains("transientChild"));
				}
		}

		[Test]
		public async Task InsertCascadeNoWarningAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();
				await (s.SaveAsync(new Child()
				{Id = "persistedChild"}));
				await (t.CommitAsync());
			}

			using (LogSpy ls = new LogSpy(LogManager.GetLogger("NHibernate"), Level.Warn))
				using (ISession s = OpenSession())
				{
					ITransaction t = s.BeginTransaction();
					Parent parent = new Parent()
					{Id = "parent", Children = new List<Child>(), };
					await (s.SaveAsync(parent));
					Child child1 = s.Load<Child>("persistedChild");
					child1.Parent = parent;
					parent.Children.Add(child1);
					Child child2 = new Child()
					{Id = "transientChild", Parent = parent};
					await (s.SaveAsync(child2));
					parent.Children.Add(child2);
					await (t.CommitAsync());
					long actual = s.CreateQuery("select count(c) from Child c").UniqueResult<long>();
					Assert.That(actual, Is.EqualTo(2));
					string[] warnings = GetAssignedIdentifierWarnings(ls);
					Assert.That(warnings.Length, Is.EqualTo(0));
				}
		}
	}
}
#endif
