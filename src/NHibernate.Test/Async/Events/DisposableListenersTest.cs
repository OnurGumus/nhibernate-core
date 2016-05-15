#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Event;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Events
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MyDisposableListener : IPostUpdateEventListener, IDisposable
	{
		public Task OnPostUpdateAsync(PostUpdateEvent @event)
		{
			return TaskHelper.CompletedTask;
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DisposableListenersTest
	{
		[Test]
		public async Task WhenCloseSessionFactoryThenCallDisposeOfListenerAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			var myDisposableListener = new MyDisposableListener();
			cfg.AppendListeners(ListenerType.PostUpdate, new[]{myDisposableListener});
			var sf = cfg.BuildSessionFactory();
			await (sf.CloseAsync());
			Assert.That(myDisposableListener.DisposeCalled, Is.True);
		}
	}
}
#endif
