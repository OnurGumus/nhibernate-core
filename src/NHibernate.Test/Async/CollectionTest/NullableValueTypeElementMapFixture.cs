﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NUnit.Framework;

namespace NHibernate.Test.CollectionTest
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class NullableValueTypeElementMapFixtureAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new[] {"CollectionTest.NullableValueTypeElementMapFixture.hbm.xml"}; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override void OnTearDown()
		{
			using (var s = sessions.OpenSession())
			{
				s.Delete("from Parent");
				s.Flush();
			}
		}

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

				await (s.SaveAsync(parent, CancellationToken.None));
				parentId = parent.Id;
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var parent = await (s.LoadAsync<Parent>(parentId, CancellationToken.None));

				Assert.That(parent.TypedDates.Count, Is.EqualTo(1),
								"Should have one child on first reload");

				Assert.That(parent.TypedDates[0], Is.Not.Null,
								 "Should have value in map for 0 on first reload");

				Assert.That(parent.TypedDates[0].Value, Is.EqualTo(date),
								"Should have same date as saved in map for 0 on first reload");

				parent.TypedDates[0] = null;
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var parent = await (s.LoadAsync<Parent>(parentId, CancellationToken.None));

				Assert.That(parent.TypedDates.Count, Is.EqualTo(1),
								"Should have one child on reload after nulling");

				Assert.That(parent.TypedDates[0], Is.Null,
							  "Should have null value for child on reload after nulling");
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

				await (s.SaveAsync(parent, CancellationToken.None));
				parentId = parent.Id;
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var parent = await (s.LoadAsync<Parent>(parentId, CancellationToken.None));

				Assert.That(parent.TypedDates.Count, Is.EqualTo(1),
					"Should have 1 child after first reload");

				Assert.That(parent.TypedDates[0], Is.Null,
					"Should have null value after first reload");

				parent.TypedDates[0] = date;
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var parent = await (s.LoadAsync<Parent>(parentId, CancellationToken.None));

				Assert.That(parent.TypedDates.Count, Is.EqualTo(1),
					"Should have 1 child on reload after setting value");

				Assert.That(parent.TypedDates[0], Is.Not.Null,
					"Should have child with value on reload after setting value");

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

				await (s.SaveAsync(parent, CancellationToken.None));
				parentId = parent.Id;
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var parent = await (s.LoadAsync<Parent>(parentId, CancellationToken.None));

				Assert.That(parent.TypedDates.Count, Is.EqualTo(2));
				Assert.That(parent.TypedDates[0], Is.Null);
				Assert.That(parent.TypedDates[1], Is.EqualTo(date));

				parent.TypedDates.Remove(0);
				parent.TypedDates[2] = null;
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var parent = await (s.LoadAsync<Parent>(parentId, CancellationToken.None));

				Assert.That(parent.TypedDates.Count, Is.EqualTo(2));
				Assert.That(parent.TypedDates[1], Is.EqualTo(date));
				Assert.That(parent.TypedDates[2], Is.Null);
			}
		}
	}
}
