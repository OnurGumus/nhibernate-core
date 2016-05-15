#if NET_4_5
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3121
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		// Some notes:
		// Mappings for all three properties use either unspecified length (defaulting to 8000 bytes)
		// or a length specified to a value smaller than 8001 bytes. This is since for larger values
		// the driver will increase the parameter size to int.MaxValue/2.
		[Test]
		public Task ShouldThrowWhenByteArrayTooLongAsync()
		{
			try
			{
				ShouldThrowWhenByteArrayTooLong();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task ShouldThrowWhenImageTooLargeAsync()
		{
			try
			{
				ShouldThrowWhenImageTooLarge();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task ShouldThrowWhenImageAsISerializableTooLargeAsync()
		{
			try
			{
				ShouldThrowWhenImageAsISerializableTooLarge();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		private async Task PersistReportAsync(Report report)
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					await (session.SaveAsync(report));
					await (session.FlushAsync());
				// No commit to avoid DB pollution (test success means we should throw and never insert anyway).
				}
		}
	}
}
#endif
