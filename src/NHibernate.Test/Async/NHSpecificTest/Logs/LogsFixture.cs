#if NET_4_5
using System.Collections;
using NHibernate.Impl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Logs
{
	using System;
	using System.IO;
	using System.Text;
	using log4net;
	using log4net.Appender;
	using log4net.Core;
	using log4net.Layout;
	using log4net.Repository.Hierarchy;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LogsFixture : TestCase
	{
		[Test]
		public async Task WillGetSessionIdFromSessionLogsAsync()
		{
			ThreadContext.Properties["sessionId"] = new SessionIdCapturer();
			using (var spy = new TextLogSpy("NHibernate.SQL", "%message | SessionId: %property{sessionId}"))
				using (var s = sessions.OpenSession())
				{
					var sessionId = ((SessionImpl)s).SessionId;
					await (s.GetAsync<Person>(1)); //will execute some sql
					var loggingEvent = spy.Events[0];
					Assert.That(loggingEvent.Contains(sessionId.ToString()), Is.True);
				}
		}
	}
}
#endif
