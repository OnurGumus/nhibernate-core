#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.CollectionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NullableValueTypeElementMapFixture : TestCase
	{
		[Test]
		public async Task ShouldOverwriteElementValueWithNullAsync()
		{
			Guid parentId;
			var date = new DateTime(2010, 09, 08);
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = new Parent();
					parent.TypedDates[0] = date;
					await (s.SaveAsync(parent));
					parentId = parent.Id;
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = s.Load<Parent>(parentId);
					Assert.That(parent.TypedDates.Count, Is.EqualTo(1), "Should have one child on first reload");
					Assert.That(parent.TypedDates[0], Is.Not.Null, "Should have value in map for 0 on first reload");
					Assert.That(parent.TypedDates[0].Value, Is.EqualTo(date), "Should have same date as saved in map for 0 on first reload");
					parent.TypedDates[0] = null;
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = s.Load<Parent>(parentId);
					Assert.That(parent.TypedDates.Count, Is.EqualTo(1), "Should have one child on reload after nulling");
					Assert.That(parent.TypedDates[0], Is.Null, "Should have null value for child on reload after nulling");
				}
		}

		[Test]
		public async Task ShouldOverwriteNullElementWithValueAsync()
		{
			Guid parentId;
			var date = new DateTime(2010, 09, 08);
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = new Parent();
					parent.TypedDates[0] = null;
					await (s.SaveAsync(parent));
					parentId = parent.Id;
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = s.Load<Parent>(parentId);
					Assert.That(parent.TypedDates.Count, Is.EqualTo(1), "Should have 1 child after first reload");
					Assert.That(parent.TypedDates[0], Is.Null, "Should have null value after first reload");
					parent.TypedDates[0] = date;
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = s.Load<Parent>(parentId);
					Assert.That(parent.TypedDates.Count, Is.EqualTo(1), "Should have 1 child on reload after setting value");
					Assert.That(parent.TypedDates[0], Is.Not.Null, "Should have child with value on reload after setting value");
					Assert.That(parent.TypedDates[0].Value, Is.EqualTo(date));
				}
		}

		[Test]
		public async Task ShouldAddAndRemoveNullElementsAsync()
		{
			Guid parentId;
			var date = new DateTime(2010, 09, 08);
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = new Parent();
					parent.TypedDates[0] = null;
					parent.TypedDates[1] = date;
					await (s.SaveAsync(parent));
					parentId = parent.Id;
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = s.Load<Parent>(parentId);
					Assert.That(parent.TypedDates.Count, Is.EqualTo(2));
					Assert.That(parent.TypedDates[0], Is.Null);
					Assert.That(parent.TypedDates[1], Is.EqualTo(date));
					parent.TypedDates.Remove(0);
					parent.TypedDates[2] = null;
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var parent = s.Load<Parent>(parentId);
					Assert.That(parent.TypedDates.Count, Is.EqualTo(2));
					Assert.That(parent.TypedDates[1], Is.EqualTo(date));
					Assert.That(parent.TypedDates[2], Is.Null);
				}
		}
	}
}
#endif
