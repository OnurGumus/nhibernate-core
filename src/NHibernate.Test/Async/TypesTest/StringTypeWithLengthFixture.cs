﻿#if NET_4_5
using System;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Exceptions;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public class StringTypeWithLengthFixtureAsync : TestCaseMappingByCodeAsync
	{
		private int GetLongStringMappedLength()
		{
			// This is a bit ugly...
			//
			// Return a value that should be the largest possible length of a string column
			// in the corresponding database. Note that the actual column type selected by the dialect
			// depends on this value, so it must be the largest possible value for the type
			// that the dialect will pick. Doesn't matter if the dialect can pick another
			// type for an even larger size.
			if (Dialect is Oracle8iDialect)
				return 2000;
			if (Dialect is MySQLDialect)
				return 65535;
			return 4000;
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<StringClass>(ca =>
			{
				ca.Lazy(false);
				ca.Id(x => x.Id, map => map.Generator(Generators.Assigned));
				ca.Property(x => x.StringValue, map => map.Length(10));
				ca.Property(x => x.LongStringValue, map => map.Length(GetLongStringMappedLength()));
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		[Test]
		[Description("Values longer than the maximum possible string length " + "should raise an exception if they would otherwise be truncated.")]
		public async Task ShouldPreventInsertionOfVeryLongStringThatWouldBeTruncatedAsync()
		{
			// This test case is for when the current driver will use a parameter size
			// that is significantly larger than the mapped column size (e.g. SqlClientDriver currently).
			// Note: This test could possible be written as
			//   "database must raise an error OR it must store and return the full value"
			// to avoid this dialect specific exception.
			if (Dialect is SQLiteDialect)
				Assert.Ignore("SQLite does not enforce specified string lengths.");
			int maxStringLength = GetLongStringMappedLength();
			var ex = Assert.CatchAsync<Exception>(async () =>
			{
				using (ISession s = OpenSession())
				{
					StringClass b = new StringClass{LongStringValue = new string ('x', maxStringLength + 1)};
					await (s.SaveAsync(b));
					await (s.FlushAsync());
				}
			}

			);
			await (AssertFailedInsertExceptionDetailsAndEmptyTableAsync(ex));
		}

		[Test]
		[Description("Values longer than the mapped string length " + "should raise an exception if they would otherwise be truncated.")]
		public async Task ShouldPreventInsertionOfTooLongStringThatWouldBeTruncatedAsync()
		{
			// Note: This test could possible be written as
			//   "database must raise an error OR it must store and return the full value"
			// to avoid this dialect specific exception.
			if (Dialect is SQLiteDialect)
				Assert.Ignore("SQLite does not enforce specified string lengths.");
			var ex = Assert.CatchAsync<Exception>(async () =>
			{
				using (ISession s = OpenSession())
				{
					StringClass b = new StringClass{StringValue = "0123456789a"};
					await (s.SaveAsync(b));
					await (s.FlushAsync());
				}
			}

			, "An exception was expected when trying to put too large a value into a column.");
			await (AssertFailedInsertExceptionDetailsAndEmptyTableAsync(ex));
		}

		private async Task AssertFailedInsertExceptionDetailsAndEmptyTableAsync(Exception ex)
		{
			// We can get different sort of exceptions.
			if (ex is PropertyValueException)
			{
				// Some drivers/dialects set explicit parameter sizes, in which case we expect NH to
				// raise a PropertyValueException (to avoid ADO.NET from silently truncating).
				Assert.That(ex.Message, Is.StringStarting("Error dehydrating property value for NHibernate.Test.TypesTest.StringClass."));
				Assert.That(ex.InnerException, Is.TypeOf<HibernateException>());
				Assert.That(ex.InnerException.Message, Is.EqualTo("The length of the string value exceeds the length configured in the mapping/parameter."));
			}
			else if (Dialect is MsSqlCeDialect && ex is InvalidOperationException)
			{
				Assert.That(ex.Message, Is.StringContaining("max=4000, len=4001"));
			}
			else
			{
				// In other cases, we expect the database itself to raise an error. This case
				// will also happen if the driver does set an explicit parameter size, but that
				// size is larger than the mapped column size.
				Assert.That(ex, Is.TypeOf<GenericADOException>());
			}

			// In any case, nothing should have been inserted.
			using (ISession s = OpenSession())
			{
				Assert.That(await (s.Query<StringClass>().ToListAsync()), Is.Empty);
			}
		}

		/// <summary>
		/// Some test cases doesn't work during some scenarios for well-known reasons. If the test
		/// fails under these circumstances, mark it as IGNORED. If it _stops_ failing, mark it
		/// as a FAILURE so that it can be investigated.
		/// </summary>
		private void AssertExpectedFailureOrNoException(Exception exception, bool requireExceptionAndIgnoreTest)
		{
			if (requireExceptionAndIgnoreTest)
			{
				Assert.NotNull(exception, "Test was expected to have a well-known, but ignored, failure for the current configuration. If " + "that expected failure no longer occurs, it may now be possible to remove this exception.");
				Assert.Ignore("This test is known to fail for the current configuration.");
			}

			// If the above didn't ignore the exception, it's for real - rethrow to trigger test failure.
			if (exception != null)
				throw new Exception("Wrapped exception.", exception);
		}

		private TException CatchException<TException>(System.Action action)where TException : Exception
		{
			try
			{
				action();
			}
			catch (TException exception)
			{
				return exception;
			}

			return null;
		}
	}
}
#endif
