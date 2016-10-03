#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1388
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task BagTestAsync()
		{
			int studentId = 1;
			// Set major.
			using (ISession session = OpenSession())
			{
				ITransaction t = session.BeginTransaction();
				var student = new Student();
				var subject1 = new Subject{Id = 1};
				var subject2 = new Subject{Id = 2};
				// Create major objects.
				var major1 = new Major{Note = ""};
				var major2 = new Major{Note = ""};
				// Set major objects.
				student.Majors[subject1] = major1;
				student.Majors[subject2] = major2;
				await (session.SaveAsync(subject1));
				await (session.SaveAsync(subject2));
				await (session.SaveAsync(student));
				await (session.FlushAsync());
				await (t.CommitAsync());
			}

			// Remove major for subject 2.
			using (ISession session = OpenSession())
			{
				ITransaction t = session.BeginTransaction();
				var student = await (session.GetAsync<Student>(studentId));
				var subject2 = await (session.GetAsync<Subject>(2));
				// Remove major.
				student.Majors.Remove(subject2);
				await (session.FlushAsync());
				await (t.CommitAsync());
			}

			// Get major for subject 2.
			using (ISession session = OpenSession())
			{
				ITransaction t = session.BeginTransaction();
				var student = await (session.GetAsync<Student>(studentId));
				var subject2 = await (session.GetAsync<Subject>(2));
				Assert.IsNotNull(subject2);
				// Major for subject 2 should have been removed.
				Assert.IsFalse(student.Majors.ContainsKey(subject2));
				await (t.CommitAsync());
			}

			// Remove all - NHibernate will now succeed in removing all.
			using (ISession session = OpenSession())
			{
				ITransaction t = session.BeginTransaction();
				var student = await (session.GetAsync<Student>(studentId));
				student.Majors.Clear();
				await (session.FlushAsync());
				await (t.CommitAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			// clean up the database
			using (ISession session = OpenSession())
			{
				session.BeginTransaction();
				foreach (var student in await (session.CreateCriteria(typeof (Student)).ListAsync<Student>()))
				{
					await (session.DeleteAsync(student));
				}

				foreach (var subject in await (session.CreateCriteria(typeof (Subject)).ListAsync<Subject>()))
				{
					await (session.DeleteAsync(subject));
				}

				await (session.Transaction.CommitAsync());
			}
		}

		protected override string CacheConcurrencyStrategy
		{
			get
			{
				return null;
			}
		}
	}
}
#endif
