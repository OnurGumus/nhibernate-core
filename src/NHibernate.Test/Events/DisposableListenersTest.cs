using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Util;
using NUnit.Framework;

namespace NHibernate.Test.Events
{
	public class MyDisposableListener : IPostUpdateEventListener, IDisposable
	{
		public Task OnPostUpdate(PostUpdateEvent @event)
		{
			return TaskHelper.CompletedTask;
		}

		public bool DisposeCalled { get; private set; }
		public void Dispose()
		{
			DisposeCalled = true;
		}
	}

	public class DisposableListenersTest
	{
		[Test]
		public void WhenCloseSessionFactoryThenCallDisposeOfListener()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			var myDisposableListener = new MyDisposableListener();
			cfg.AppendListeners(ListenerType.PostUpdate, new[]{myDisposableListener});
			var sf = cfg.BuildSessionFactory();
			sf.Close();
			Assert.That(myDisposableListener.DisposeCalled, Is.True);
		}
	}
}