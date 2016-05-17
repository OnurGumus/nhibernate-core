#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Criterion;
using NHibernate.Proxy;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ReadOnly
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReadOnlyCriteriaQueryTest : AbstractReadOnlyTest
	{
		[Test]
		public async Task ModifiableSessionDefaultCriteriaAsync()
		{
			Sfi.Statistics.Clear();
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					Course coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					Enrolment enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}

			Assert.That(sessions.Statistics.EntityInsertCount, Is.EqualTo(4));
			Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(0));
			sessions.Statistics.Clear();
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					ICriteria criteria = s.CreateCriteria<Student>();
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.False);
					Assert.That(criteria.IsReadOnly, Is.False);
					Student gavin = (Student)await (criteria.UniqueResultAsync());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.False);
					Assert.That(criteria.IsReadOnly, Is.False);
					Assert.That(s.IsReadOnly(gavin), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, gavin.PreferredCourse, false);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					NHibernateUtil.Initialize(gavin.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, gavin.PreferredCourse, false);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}

			Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(1));
			Assert.That(sessions.Statistics.EntityDeleteCount, Is.EqualTo(4));
			sessions.Statistics.Clear();
		}

		[Test]
		public async Task ModifiableSessionReadOnlyCriteriaAsync()
		{
			await (DefaultTestSetupAsync());
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					ICriteria criteria = s.CreateCriteria<Student>().SetReadOnly(true);
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Assert.That(s.IsReadOnly(gavin), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, gavin.PreferredCourse, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					NHibernateUtil.Initialize(gavin.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, gavin.PreferredCourse, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ModifiableSessionModifiableCriteriaAsync()
		{
			await (DefaultTestSetupAsync());
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					ICriteria criteria = s.CreateCriteria<Student>();
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.False);
					Assert.That(criteria.IsReadOnly, Is.False);
					criteria.SetReadOnly(false);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Assert.That(s.IsReadOnly(gavin), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, gavin.PreferredCourse, false);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					NHibernateUtil.Initialize(gavin.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, gavin.PreferredCourse, false);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ReadOnlySessionDefaultCriteriaAsync()
		{
			await (DefaultTestSetupAsync());
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.DefaultReadOnly = true;
					ICriteria criteria = s.CreateCriteria<Student>();
					Assert.That(s.DefaultReadOnly, Is.True);
					Assert.That(criteria.IsReadOnlyInitialized, Is.False);
					Assert.That(criteria.IsReadOnly, Is.True);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.True);
					Assert.That(criteria.IsReadOnlyInitialized, Is.False);
					Assert.That(criteria.IsReadOnly, Is.True);
					Assert.That(s.IsReadOnly(gavin), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, gavin.PreferredCourse, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					NHibernateUtil.Initialize(gavin.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, gavin.PreferredCourse, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, true);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, true);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ReadOnlySessionReadOnlyCriteriaAsync()
		{
			await (DefaultTestSetupAsync());
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.DefaultReadOnly = true;
					ICriteria criteria = s.CreateCriteria<Student>();
					Assert.That(s.DefaultReadOnly, Is.True);
					Assert.That(criteria.IsReadOnlyInitialized, Is.False);
					Assert.That(criteria.IsReadOnly, Is.True);
					criteria.SetReadOnly(true);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.True);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Assert.That(s.IsReadOnly(gavin), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, gavin.PreferredCourse, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					NHibernateUtil.Initialize(gavin.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, gavin.PreferredCourse, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, true);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, true);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ReadOnlySessionModifiableCriteriaAsync()
		{
			await (DefaultTestSetupAsync());
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.DefaultReadOnly = true;
					ICriteria criteria = s.CreateCriteria<Student>();
					Assert.That(s.DefaultReadOnly, Is.True);
					Assert.That(criteria.IsReadOnlyInitialized, Is.False);
					Assert.That(criteria.IsReadOnly, Is.True);
					criteria.SetReadOnly(false);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.True);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Assert.That(s.IsReadOnly(gavin), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, gavin.PreferredCourse, false);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					NHibernateUtil.Initialize(gavin.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, gavin.PreferredCourse, false);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, true);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, true);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ReadOnlyCriteriaReturnsModifiableExistingEntityAsync()
		{
			Course coursePreferred = null;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					Enrolment enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Assert.That(s.DefaultReadOnly, Is.False);
					coursePreferred = await (s.GetAsync<Course>(coursePreferred.CourseCode));
					Assert.That(s.IsReadOnly(coursePreferred), Is.False);
					ICriteria criteria = s.CreateCriteria<Student>().SetReadOnly(true);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Assert.That(s.IsReadOnly(gavin), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ReadOnlyCriteriaReturnsExistingModifiableProxyNotInitAsync()
		{
			Course coursePreferred = null;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					Enrolment enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Assert.That(s.DefaultReadOnly, Is.False);
					coursePreferred = s.Load<Course>(coursePreferred.CourseCode);
					Assert.That(NHibernateUtil.IsInitialized(coursePreferred), Is.False);
					CheckProxyReadOnly(s, coursePreferred, false);
					ICriteria criteria = s.CreateCriteria<Student>().SetReadOnly(true);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Assert.That(s.IsReadOnly(gavin), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(coursePreferred), Is.False);
					CheckProxyReadOnly(s, coursePreferred, false);
					NHibernateUtil.Initialize(coursePreferred);
					CheckProxyReadOnly(s, coursePreferred, false);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ReadOnlyCriteriaReturnsExistingModifiableProxyInitAsync()
		{
			Course coursePreferred = null;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					Enrolment enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Assert.That(s.DefaultReadOnly, Is.False);
					coursePreferred = s.Load<Course>(coursePreferred.CourseCode);
					Assert.That(NHibernateUtil.IsInitialized(coursePreferred), Is.False);
					CheckProxyReadOnly(s, coursePreferred, false);
					NHibernateUtil.Initialize(coursePreferred);
					CheckProxyReadOnly(s, coursePreferred, false);
					ICriteria criteria = s.CreateCriteria<Student>().SetReadOnly(true);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.True);
					Assert.That(s.IsReadOnly(gavin), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(coursePreferred), Is.True);
					CheckProxyReadOnly(s, coursePreferred, false);
					NHibernateUtil.Initialize(coursePreferred);
					CheckProxyReadOnly(s, coursePreferred, false);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ModifiableCriteriaReturnsExistingReadOnlyEntityAsync()
		{
			Course coursePreferred = null;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					Enrolment enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Assert.That(s.DefaultReadOnly, Is.False);
					coursePreferred = await (s.GetAsync<Course>(coursePreferred.CourseCode));
					Assert.That(s.IsReadOnly(coursePreferred), Is.False);
					s.SetReadOnly(coursePreferred, true);
					ICriteria criteria = s.CreateCriteria<Student>().SetReadOnly(false);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Assert.That(s.IsReadOnly(gavin), Is.False);
					Assert.That(s.IsReadOnly(coursePreferred), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ModifiableCriteriaReturnsExistingReadOnlyProxyNotInitAsync()
		{
			Course coursePreferred = null;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					Enrolment enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Assert.That(s.DefaultReadOnly, Is.False);
					coursePreferred = s.Load<Course>(coursePreferred.CourseCode);
					Assert.That(NHibernateUtil.IsInitialized(coursePreferred), Is.False);
					CheckProxyReadOnly(s, coursePreferred, false);
					s.SetReadOnly(coursePreferred, true);
					CheckProxyReadOnly(s, coursePreferred, true);
					ICriteria criteria = s.CreateCriteria<Student>().SetReadOnly(false);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Assert.That(s.IsReadOnly(gavin), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(coursePreferred), Is.False);
					CheckProxyReadOnly(s, coursePreferred, true);
					NHibernateUtil.Initialize(coursePreferred);
					CheckProxyReadOnly(s, coursePreferred, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task ModifiableCriteriaReturnsExistingReadOnlyProxyInitAsync()
		{
			Course coursePreferred = null;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					Enrolment enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Assert.That(s.DefaultReadOnly, Is.False);
					coursePreferred = s.Load<Course>(coursePreferred.CourseCode);
					Assert.That(NHibernateUtil.IsInitialized(coursePreferred), Is.False);
					CheckProxyReadOnly(s, coursePreferred, false);
					NHibernateUtil.Initialize(coursePreferred);
					CheckProxyReadOnly(s, coursePreferred, false);
					s.SetReadOnly(coursePreferred, true);
					CheckProxyReadOnly(s, coursePreferred, true);
					ICriteria criteria = s.CreateCriteria<Student>().SetReadOnly(false);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Student gavin = await (criteria.UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(criteria.IsReadOnlyInitialized, Is.True);
					Assert.That(criteria.IsReadOnly, Is.False);
					Assert.That(s.IsReadOnly(gavin), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(coursePreferred), Is.True);
					CheckProxyReadOnly(s, coursePreferred, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					Enrolment enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(enrolment.Course));
					await (s.DeleteAsync(enrolment));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task SubselectAsync()
		{
			Student gavin = null;
			Enrolment enrolment = null;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					Course coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					DetachedCriteria dc = NHibernate.Criterion.DetachedCriteria.For<Student>().Add(Property.ForName("StudentNumber").Eq(232L)).SetProjection(Property.ForName("Name"));
					gavin = await (s.CreateCriteria<Student>().Add(Subqueries.Exists(dc)).SetReadOnly(true).UniqueResultAsync<Student>());
					Assert.That(s.DefaultReadOnly, Is.False);
					Assert.That(s.IsReadOnly(gavin), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, gavin.PreferredCourse, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.False);
					NHibernateUtil.Initialize(gavin.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(gavin.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, gavin.PreferredCourse, true);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.False);
					NHibernateUtil.Initialize(gavin.Enrolments);
					Assert.That(NHibernateUtil.IsInitialized(gavin.Enrolments), Is.True);
					Assert.That(gavin.Enrolments.Count, Is.EqualTo(1));
					IEnumerator<Enrolment> enrolments = gavin.Enrolments.GetEnumerator();
					enrolments.MoveNext();
					enrolment = enrolments.Current;
					Assert.That(s.IsReadOnly(enrolment), Is.False);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, false);
					NHibernateUtil.Initialize(enrolment.Course);
					CheckProxyReadOnly(s, enrolment.Course, false);
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					DetachedCriteria dc = NHibernate.Criterion.DetachedCriteria.For<Student>("st").Add(Property.ForName("st.StudentNumber").EqProperty("e.StudentNumber")).SetProjection(Property.ForName("Name"));
					enrolment = await (s.CreateCriteria<Enrolment>("e").Add(Subqueries.Eq("Gavin King", dc)).SetReadOnly(true).UniqueResultAsync<Enrolment>());
					Assert.That(s.IsReadOnly(enrolment), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, true);
					NHibernateUtil.Initialize(enrolment.Course);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.True);
					CheckProxyReadOnly(s, enrolment.Course, true);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Student), Is.False);
					CheckProxyReadOnly(s, enrolment.Student, true);
					NHibernateUtil.Initialize(enrolment.Student);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Student), Is.True);
					CheckProxyReadOnly(s, enrolment.Student, true);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Student.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, enrolment.Student.PreferredCourse, false);
					NHibernateUtil.Initialize(enrolment.Student.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Student.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, enrolment.Student.PreferredCourse, false);
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					DetachedCriteria dc = NHibernate.Criterion.DetachedCriteria.For<Student>("st").CreateCriteria("Enrolments").CreateCriteria("Course").Add(Property.ForName("Description").Eq("Hibernate Training")).SetProjection(Property.ForName("st.Name"));
					enrolment = await (s.CreateCriteria<Enrolment>("e").Add(Subqueries.Eq("Gavin King", dc)).SetReadOnly(true).UniqueResultAsync<Enrolment>());
					Assert.That(s.IsReadOnly(enrolment), Is.True);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.False);
					CheckProxyReadOnly(s, enrolment.Course, true);
					NHibernateUtil.Initialize(enrolment.Course);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Course), Is.True);
					CheckProxyReadOnly(s, enrolment.Course, true);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Student), Is.False);
					CheckProxyReadOnly(s, enrolment.Student, true);
					NHibernateUtil.Initialize(enrolment.Student);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Student), Is.True);
					CheckProxyReadOnly(s, enrolment.Student, true);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Student.PreferredCourse), Is.False);
					CheckProxyReadOnly(s, enrolment.Student.PreferredCourse, false);
					NHibernateUtil.Initialize(enrolment.Student.PreferredCourse);
					Assert.That(NHibernateUtil.IsInitialized(enrolment.Student.PreferredCourse), Is.True);
					CheckProxyReadOnly(s, enrolment.Student.PreferredCourse, false);
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync(gavin.PreferredCourse));
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(gavin.Enrolments.First().Course));
					await (s.DeleteAsync(gavin.Enrolments.First()));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task DetachedCriteriaAsync()
		{
			DetachedCriteria dc = NHibernate.Criterion.DetachedCriteria.For<Student>().Add(Property.ForName("Name").Eq("Gavin King")).AddOrder(Order.Asc("StudentNumber"));
			byte[] bytes = SerializationHelper.Serialize(dc);
			dc = (DetachedCriteria)SerializationHelper.Deserialize(bytes);
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					Student bizarroGavin = new Student();
					bizarroGavin.Name = "Gavin King";
					bizarroGavin.StudentNumber = 666;
					await (s.PersistAsync(bizarroGavin));
					await (s.PersistAsync(gavin));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					IList result = dc.GetExecutableCriteria(s).SetMaxResults(3).SetReadOnly(true).List();
					Assert.That(result.Count, Is.EqualTo(2));
					Student gavin = (Student)result[0];
					Student bizarroGavin = (Student)result[1];
					Assert.That(gavin.StudentNumber, Is.EqualTo(232));
					Assert.That(bizarroGavin.StudentNumber, Is.EqualTo(666));
					Assert.That(s.IsReadOnly(gavin), Is.True);
					Assert.That(s.IsReadOnly(bizarroGavin), Is.True);
					await (s.DeleteAsync(gavin));
					await (s.DeleteAsync(bizarroGavin));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task TwoAliasesCacheAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Course course = new Course();
			course.CourseCode = "HIB";
			course.Description = "Hibernate Training";
			await (s.SaveAsync(course));
			Student gavin = new Student();
			gavin.Name = "Gavin King";
			gavin.StudentNumber = 666;
			await (s.SaveAsync(gavin));
			Student xam = new Student();
			xam.Name = "Max Rydahl Andersen";
			xam.StudentNumber = 101;
			await (s.SaveAsync(xam));
			Enrolment enrolment1 = new Enrolment();
			enrolment1.Course = course;
			enrolment1.CourseCode = course.CourseCode;
			enrolment1.Semester = 1;
			enrolment1.Year = 1999;
			enrolment1.Student = xam;
			enrolment1.StudentNumber = xam.StudentNumber;
			xam.Enrolments.Add(enrolment1);
			await (s.SaveAsync(enrolment1));
			Enrolment enrolment2 = new Enrolment();
			enrolment2.Course = course;
			enrolment2.CourseCode = course.CourseCode;
			enrolment2.Semester = 3;
			enrolment2.Year = 1998;
			enrolment2.Student = gavin;
			enrolment2.StudentNumber = gavin.StudentNumber;
			gavin.Enrolments.Add(enrolment2);
			await (s.SaveAsync(enrolment2));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			IList list = s.CreateCriteria<Enrolment>().CreateAlias("Student", "s").CreateAlias("Course", "c").Add(Restrictions.IsNotEmpty("s.Enrolments")).SetCacheable(true).SetReadOnly(true).List();
			Assert.That(list.Count, Is.EqualTo(2));
			Enrolment e = (Enrolment)list[0];
			if (e.Student.StudentNumber == xam.StudentNumber)
			{
				enrolment1 = e;
				enrolment2 = (Enrolment)list[1];
			}
			else if (e.Student.StudentNumber == gavin.StudentNumber)
			{
				enrolment2 = e;
				enrolment1 = (Enrolment)list[1];
			}
			else
			{
				Assert.Fail("Enrolment has unknown student number: " + e.Student.StudentNumber);
			}

			Assert.That(s.IsReadOnly(enrolment1), Is.True);
			Assert.That(s.IsReadOnly(enrolment2), Is.True);
			Assert.That(s.IsReadOnly(enrolment1.Course), Is.True);
			Assert.That(s.IsReadOnly(enrolment2.Course), Is.True);
			Assert.That(enrolment1.Course, Is.SameAs(enrolment2.Course));
			Assert.That(s.IsReadOnly(enrolment1.Student), Is.True);
			Assert.That(s.IsReadOnly(enrolment2.Student), Is.True);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			list = s.CreateCriteria<Enrolment>().CreateAlias("Student", "s").CreateAlias("Course", "c").SetReadOnly(true).Add(Restrictions.IsNotEmpty("s.Enrolments")).SetCacheable(true).SetReadOnly(true).List();
			Assert.That(list.Count, Is.EqualTo(2));
			e = (Enrolment)list[0];
			if (e.Student.StudentNumber == xam.StudentNumber)
			{
				enrolment1 = e;
				enrolment2 = (Enrolment)list[1];
			}
			else if (e.Student.StudentNumber == gavin.StudentNumber)
			{
				enrolment2 = e;
				enrolment1 = (Enrolment)list[1];
			}
			else
			{
				Assert.Fail("Enrolment has unknown student number: " + e.Student.StudentNumber);
			}

			Assert.That(s.IsReadOnly(enrolment1), Is.True);
			Assert.That(s.IsReadOnly(enrolment2), Is.True);
			Assert.That(s.IsReadOnly(enrolment1.Course), Is.True);
			Assert.That(s.IsReadOnly(enrolment2.Course), Is.True);
			Assert.That(enrolment1.Course, Is.SameAs(enrolment2.Course));
			Assert.That(s.IsReadOnly(enrolment1.Student), Is.True);
			Assert.That(s.IsReadOnly(enrolment2.Student), Is.True);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			list = s.CreateCriteria<Enrolment>().SetReadOnly(true).CreateAlias("Student", "s").CreateAlias("Course", "c").Add(Restrictions.IsNotEmpty("s.Enrolments")).SetCacheable(true).List();
			Assert.That(list.Count, Is.EqualTo(2));
			e = (Enrolment)list[0];
			if (e.Student.StudentNumber == xam.StudentNumber)
			{
				enrolment1 = e;
				enrolment2 = (Enrolment)list[1];
			}
			else if (e.Student.StudentNumber == gavin.StudentNumber)
			{
				enrolment2 = e;
				enrolment1 = (Enrolment)list[1];
			}
			else
			{
				Assert.Fail("Enrolment has unknown student number: " + e.Student.StudentNumber);
			}

			Assert.That(s.IsReadOnly(enrolment1), Is.True);
			Assert.That(s.IsReadOnly(enrolment2), Is.True);
			Assert.That(s.IsReadOnly(enrolment1.Course), Is.True);
			Assert.That(s.IsReadOnly(enrolment2.Course), Is.True);
			Assert.That(enrolment1.Course, Is.SameAs(enrolment2.Course));
			Assert.That(s.IsReadOnly(enrolment1.Student), Is.True);
			Assert.That(s.IsReadOnly(enrolment2.Student), Is.True);
			await (s.DeleteAsync(enrolment1));
			await (s.DeleteAsync(enrolment2));
			await (s.DeleteAsync(enrolment1.Course));
			await (s.DeleteAsync(enrolment1.Student));
			await (s.DeleteAsync(enrolment2.Student));
			await (t.CommitAsync());
			s.Close();
		}

		private async Task DefaultTestSetupAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Course course = new Course();
					course.CourseCode = "HIB";
					course.Description = "Hibernate Training";
					await (s.PersistAsync(course));
					Course coursePreferred = new Course();
					coursePreferred.CourseCode = "JBOSS";
					coursePreferred.Description = "JBoss";
					await (s.PersistAsync(coursePreferred));
					Student gavin = new Student();
					gavin.Name = "Gavin King";
					gavin.StudentNumber = 232;
					gavin.PreferredCourse = coursePreferred;
					await (s.PersistAsync(gavin));
					Enrolment enrolment = new Enrolment();
					enrolment.Course = course;
					enrolment.CourseCode = course.CourseCode;
					enrolment.Semester = 3;
					enrolment.Year = 1998;
					enrolment.Student = gavin;
					enrolment.StudentNumber = gavin.StudentNumber;
					gavin.Enrolments.Add(enrolment);
					await (s.PersistAsync(enrolment));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
