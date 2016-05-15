#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2328
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
