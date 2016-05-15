#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2554
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task TestMappedFormulasContainingSqlServerDataTypeKeywordsAsync()
		{
			using (ISession session = Sfi.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var students = await (session.CreateQuery("from Student").ListAsync<Student>());
					Assert.That(students.Count, Is.EqualTo(1));
					Assert.That(students[0].FullName, Is.StringMatching("Julian Maughan"));
					Assert.That(students[0].FullNameAsVarBinary.Length, Is.EqualTo(28));
					Assert.That(students[0].FullNameAsVarBinary512.Length, Is.EqualTo(28));
					// Assert.That(students[0].FullNameAsBinary.Length, Is.EqualTo(28)); 30???
					Assert.That(students[0].FullNameAsBinary256.Length, Is.EqualTo(256));
					Assert.That(students[0].FullNameAsVarChar.Length, Is.EqualTo(14));
					Assert.That(students[0].FullNameAsVarChar125.Length, Is.EqualTo(14));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task TestHqlStatementsContainingSqlServerDataTypeKeywordsAsync()
		{
			using (ISession session = Sfi.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var students = await (session.CreateQuery("from Student where length(convert(varbinary, FullName)) = 28").ListAsync<Student>());
					Assert.That(students.Count, Is.EqualTo(1));
					students = await (session.CreateQuery("from Student where length(convert(varbinary(256), FullName)) = 28").ListAsync<Student>());
					Assert.That(students.Count, Is.EqualTo(1));
					students = await (session.CreateQuery("from Student where convert(int, 1) = 1").ListAsync<Student>());
					Assert.That(students.Count, Is.EqualTo(1));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
