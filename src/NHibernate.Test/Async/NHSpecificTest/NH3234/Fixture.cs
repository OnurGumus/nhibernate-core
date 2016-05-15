#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3234
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		private async Task EvictAsync(ISession session, GridWidget widget)
		{
			await (session.EvictAsync(widget));
			sessions.Evict(widget.GetType());
		}

		private static async Task SaveAsync(ISession session, GridWidget widget)
		{
			if (widget.Id != Guid.Empty && !await (session.ContainsAsync(widget)))
				widget = await (session.MergeAsync(widget));
			await (session.SaveOrUpdateAsync(widget));
			await (session.FlushAsync());
		}

		[Test]
		public async Task ShouldNotFailWhenAddingNewLevelsAsync()
		{
			using (var session = OpenSession())
			{
				var widget = new GridWidget{Levels = {new GridLevel(), new GridLevel()}, };
				await (SaveAsync(session, widget));
				await (EvictAsync(session, widget));
				widget.Levels.Add(new GridLevel());
				await (SaveAsync(session, widget));
				await (EvictAsync(session, widget));
				var loaded = await (session.GetAsync<GridWidget>(widget.Id));
				Assert.That(loaded.Levels.Count, Is.EqualTo(3));
			}
		}

		[Test]
		public async Task ShouldNotFailWhenReplacingLevelsAsync()
		{
			using (var session = OpenSession())
			{
				var widget = new GridWidget{Levels = {new GridLevel(), new GridLevel()}, };
				await (SaveAsync(session, widget));
				await (EvictAsync(session, widget));
				widget.Levels.Clear();
				widget.Levels.Add(new GridLevel());
				await (SaveAsync(session, widget));
				await (EvictAsync(session, widget));
				var loaded = await (session.GetAsync<GridWidget>(widget.Id));
				Assert.That(loaded.Levels.Count, Is.EqualTo(1));
			}
		}
	}
}
#endif
