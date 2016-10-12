#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CharEqualityTestsAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Person>(ca =>
			{
				ca.Id(x => x.Id, map => map.Generator(Generators.Assigned));
				ca.Property(x => x.Name, map => map.Length(150));
				ca.Property(x => x.Type, map => map.Length(1));
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{Id = 1000, Name = "Person Type A", Type = 'A'}));
					await (session.SaveAsync(new Person{Id = 1001, Name = "Person Type B", Type = 'B'}));
					await (session.SaveAsync(new Person{Id = 1002, Name = "Person Type C", Type = 'C'}));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task CharPropertyEqualToCharLiteralAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => x.Type == 'C')));
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0].Name, Is.EqualTo("Person Type C"));
		}

		[Test]
		public async Task CharLiteralEqualToCharPropertyAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => 'C' == x.Type)));
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0].Name, Is.EqualTo("Person Type C"));
		}

		[Test]
		public async Task CharPropertyEqualToCharVariableAsync()
		{
			char value = 'C';
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => x.Type == value)));
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0].Name, Is.EqualTo("Person Type C"));
		}

		[Test]
		public async Task CharVariableEqualToCharPropertyAsync()
		{
			char value = 'C';
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => value == x.Type)));
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0].Name, Is.EqualTo("Person Type C"));
		}

		[Test]
		public async Task CharPropertyNotEqualToCharLiteralAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => x.Type != 'C')));
			Assert.That(results.Count, Is.EqualTo(2));
			Assert.That(results.Select(p => p.Name), Is.EquivalentTo(new[]{"Person Type A", "Person Type B"}));
		}

		[Test]
		public async Task CharLiteralNotEqualToCharPropertyAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => 'C' != x.Type)));
			Assert.That(results.Count, Is.EqualTo(2));
			Assert.That(results.Select(p => p.Name), Is.EquivalentTo(new[]{"Person Type A", "Person Type B"}));
		}

		[Test]
		public async Task CharPropertyNotEqualToCharVariableAsync()
		{
			char value = 'C';
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => x.Type != value)));
			Assert.That(results.Count, Is.EqualTo(2));
			Assert.That(results.Select(p => p.Name), Is.EquivalentTo(new[]{"Person Type A", "Person Type B"}));
		}

		[Test]
		public async Task CharVariableNotEqualToCharPropertyAsync()
		{
			char value = 'C';
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => value != x.Type)));
			Assert.That(results.Count, Is.EqualTo(2));
			Assert.That(results.Select(p => p.Name), Is.EquivalentTo(new[]{"Person Type A", "Person Type B"}));
		}

		[Test]
		public async Task CharPropertyGreaterThanCharLiteralAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => x.Type > 'B')));
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0].Name, Is.EqualTo("Person Type C"));
		}

		[Test]
		public async Task CharLiteralLessThanCharPropertyAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => 'B' < x.Type)));
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0].Name, Is.EqualTo("Person Type C"));
		}

		[Test]
		public async Task CharPropertyGreaterThanOrEqualToCharLiteralAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => x.Type >= 'B')));
			Assert.That(results.Count, Is.EqualTo(2));
			Assert.That(results.Select(p => p.Name), Is.EquivalentTo(new[]{"Person Type B", "Person Type C"}));
		}

		[Test]
		public async Task CharLiteralLessThanOrEqualToCharPropertyAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => 'B' <= x.Type)));
			Assert.That(results.Count, Is.EqualTo(2));
			Assert.That(results.Select(p => p.Name), Is.EquivalentTo(new[]{"Person Type B", "Person Type C"}));
		}

		[Test]
		public async Task CharPropertyLessThanCharLiteralAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => x.Type < 'B')));
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0].Name, Is.EqualTo("Person Type A"));
		}

		[Test]
		public async Task CharLiteralGreaterThanCharPropertyAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => 'B' > x.Type)));
			Assert.That(results.Count, Is.EqualTo(1));
			Assert.That(results[0].Name, Is.EqualTo("Person Type A"));
		}

		[Test]
		public async Task CharPropertyLessThanOrEqualToCharLiteralAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => x.Type <= 'B')));
			Assert.That(results.Count, Is.EqualTo(2));
			Assert.That(results.Select(p => p.Name), Is.EquivalentTo(new[]{"Person Type A", "Person Type B"}));
		}

		[Test]
		public async Task CharLiteralGreaterThanOrEqualToCharPropertyAsync()
		{
			var results = await (ExecuteAsync(session => session.Query<Person>().Where(x => 'B' >= x.Type)));
			Assert.That(results.Count, Is.EqualTo(2));
			Assert.That(results.Select(p => p.Name), Is.EquivalentTo(new[]{"Person Type A", "Person Type B"}));
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (transaction.CommitAsync());
				}
		}

		private async Task<IList<Person>> ExecuteAsync(Func<IStatelessSession, IQueryable<Person>> query)
		{
			using (var session = Sfi.OpenStatelessSession())
				using (session.BeginTransaction())
				{
					return await (query(session).ToListAsync());
				}
		}
	}
}
#endif
