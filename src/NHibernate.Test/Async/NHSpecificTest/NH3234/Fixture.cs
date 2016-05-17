#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3234
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		private static async Task SaveAsync(ISession session, GridWidget widget)
		{
			if (widget.Id != Guid.Empty && !session.Contains(widget))
				widget = session.Merge(widget);
			session.SaveOrUpdate(widget);
			await (session.FlushAsync());
		}

		[Test]
		public async Task ShouldNotFailWhenAddingNewLevelsAsync()
		{
			using (var session = OpenSession())
			{
				var widget = new GridWidget{Levels = {new GridLevel(), new GridLevel()}, };
				await (SaveAsync(session, widget));
				Evict(session, widget);
				widget.Levels.Add(new GridLevel());
				await (SaveAsync(session, widget));
				Evict(session, widget);
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
				Evict(session, widget);
				widget.Levels.Clear();
				widget.Levels.Add(new GridLevel());
				await (SaveAsync(session, widget));
				Evict(session, widget);
				var loaded = await (session.GetAsync<GridWidget>(widget.Id));
				Assert.That(loaded.Levels.Count, Is.EqualTo(1));
			}
		}
	}
}
#endif
