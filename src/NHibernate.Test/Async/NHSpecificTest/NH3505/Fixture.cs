#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3505
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task StatelessSessionLazyUpdateAsync()
		{
			var s = OpenSession();
			Guid studentId;
			Guid teacherId;
			try
			{
				var teacher = new Teacher{Name = "Wise Man"};
				await (s.SaveAsync(teacher));
				teacherId = teacher.Id;
				var student = new Student{Name = "Rebelious Teenager", Teacher = teacher};
				await (s.SaveAsync(student));
				studentId = student.Id;
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}

			var ss = Sfi.OpenStatelessSession();
			try
			{
				var trans = ss.BeginTransaction();
				try
				{
					var student = ss.Get<Student>(studentId);
					Assert.AreEqual(teacherId, student.Teacher.Id);
					Assert.AreEqual("Rebelious Teenager", student.Name);
					student.Name = "Young Protege";
					ss.Update(student);
					await (trans.CommitAsync());
				}
				catch
				{
					trans.Rollback();
					throw;
				}
			}
			finally
			{
				ss.Close();
			}

			s = OpenSession();
			try
			{
				var student = await (s.GetAsync<Student>(studentId));
				Assert.AreEqual(teacherId, student.Teacher.Id);
				Assert.AreEqual("Young Protege", student.Name);
			}
			finally
			{
				s.Close();
			}
		}
	}
}
#endif
