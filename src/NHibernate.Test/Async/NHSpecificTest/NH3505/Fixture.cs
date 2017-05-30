﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH3505
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnTearDown()
		{
			using (ISession s = sessions.OpenSession())
			{
				s.Delete("from Student");
			    s.Delete("from Teacher");
				s.Flush();
			}
		}

		[Test]
		public async Task StatelessSessionLazyUpdateAsync()
		{
		    var s = OpenSession();
		    Guid studentId;
		    Guid teacherId;
			try
			{
			    var teacher = new Teacher {Name = "Wise Man"};
				await (s.SaveAsync(teacher, CancellationToken.None));
			    teacherId = teacher.Id;
			    var student = new Student {Name = "Rebelious Teenager", Teacher = teacher};
			    await (s.SaveAsync(student, CancellationToken.None));
			    studentId = student.Id;
				await (s.FlushAsync(CancellationToken.None));
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
                    var student = await (ss.GetAsync<Student>(studentId, CancellationToken.None));
                    Assert.AreEqual(teacherId, student.Teacher.Id);
                    Assert.AreEqual("Rebelious Teenager", student.Name);
                    student.Name = "Young Protege";
                    await (ss.UpdateAsync(student, CancellationToken.None));
                    await (trans.CommitAsync(CancellationToken.None));
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
			    var student = await (s.GetAsync<Student>(studentId, CancellationToken.None));
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
