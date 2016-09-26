#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2328
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var circle = new Circle();
					var square = new Square();
					await (s.SaveAsync(circle));
					await (s.SaveAsync(square));
					await (s.SaveAsync(new ToyBox()
					{Name = "Box1", Shape = circle}));
					await (s.SaveAsync(new ToyBox()
					{Name = "Box2", Shape = square}));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from ToyBox").ExecuteUpdateAsync());
					await (s.CreateQuery("delete from Circle").ExecuteUpdateAsync());
					await (s.CreateQuery("delete from Square").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task AnyIs_QueryOverAsync()
		{
			using (ISession s = OpenSession())
			{
				var boxes = await (s.QueryOver<ToyBox>().Where(t => t.Shape is Square).ListAsync());
				Assert.That(boxes.Count, Is.EqualTo(1));
				Assert.That(boxes[0].Name, Is.EqualTo("Box2"));
			}
		}

		[Test]
		public async Task AnyIs_HqlWorksWithClassNameInTheRightAsync()
		{
			using (ISession s = OpenSession())
			{
				var boxes = await (s.CreateQuery("from ToyBox t where t.Shape.class = Square").ListAsync<ToyBox>());
				Assert.That(boxes.Count, Is.EqualTo(1));
				Assert.That(boxes[0].Name, Is.EqualTo("Box2"));
			}
		}

		[Test]
		public async Task AnyIs_HqlWorksWithClassNameInTheLeftAsync()
		{
			using (ISession s = OpenSession())
			{
				var boxes = await (s.CreateQuery("from ToyBox t where Square = t.Shape.class").ListAsync<ToyBox>());
				Assert.That(boxes.Count, Is.EqualTo(1));
				Assert.That(boxes[0].Name, Is.EqualTo("Box2"));
			}
		}
	}
}
#endif
