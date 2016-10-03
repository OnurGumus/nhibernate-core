﻿#if NET_4_5
using System;
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Criteria.Lambda
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FunctionsIntegrationFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Criteria.Lambda.Mappings.hbm.xml"};
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person{Name = "p2", BirthDate = new DateTime(2008, 07, 07)}));
					await (s.SaveAsync(new Person{Name = "p1", BirthDate = new DateTime(2009, 08, 07), Age = 90}));
					await (s.SaveAsync(new Person{Name = "pP3", BirthDate = new DateTime(2007, 06, 05)}));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task YearPartEqualAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = await (s.QueryOver<Person>().Where(p => p.BirthDate.YearPart() == 2008).ListAsync());
					Assert.That(persons.Count, Is.EqualTo(1));
					Assert.That(persons[0].Name, Is.EqualTo("p2"));
				}
		}

		[Test]
		public async Task YearPartIsInAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = await (s.QueryOver<Person>().Where(p => p.BirthDate.YearPart().IsIn(new[]{2008, 2009})).OrderBy(p => p.Name).Asc.ListAsync());
					Assert.That(persons.Count, Is.EqualTo(2));
					Assert.That(persons[0].Name, Is.EqualTo("p1"));
					Assert.That(persons[1].Name, Is.EqualTo("p2"));
				}
		}

		[Test]
		public async Task YearPartSingleOrDefaultAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var yearOfBirth = await (s.QueryOver<Person>().Where(p => p.Name == "p2").Select(p => p.BirthDate.YearPart()).SingleOrDefaultAsync<object>());
					Assert.That(yearOfBirth.GetType(), Is.EqualTo(typeof (int)));
					Assert.That(yearOfBirth, Is.EqualTo(2008));
				}
		}

		[Test]
		public async Task SelectAvgYearPartAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var avgYear = await (s.QueryOver<Person>().SelectList(list => list.SelectAvg(p => p.BirthDate.YearPart())).SingleOrDefaultAsync<object>());
					Assert.That(avgYear.GetType(), Is.EqualTo(typeof (double)));
					Assert.That(string.Format("{0:0}", avgYear), Is.EqualTo("2008"));
				}
		}

		[Test]
		public async Task SqrtSingleOrDefaultAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var sqrtOfAge = await (s.QueryOver<Person>().Where(p => p.Name == "p1").Select(p => Math.Round(p.Age.Sqrt(), 2)).SingleOrDefaultAsync<object>());
					Assert.That(sqrtOfAge, Is.InstanceOf<double>());
					Assert.That(string.Format("{0:0.00}", sqrtOfAge), Is.EqualTo((9.49).ToString()));
				}
		}

		[Test]
		public async Task RoundDoubleWithOneArgumentAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var roundedValue = await (s.QueryOver<Person>().Where(p => p.Name == "p1").Select(p => Math.Round(p.Age.Sqrt())).SingleOrDefaultAsync<object>());
					Assert.That(roundedValue, Is.InstanceOf<double>());
					Assert.That(roundedValue, Is.EqualTo(9));
				}
		}

		[Test]
		public async Task RoundDecimalWithOneArgumentAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var roundedValue = await (s.QueryOver<Person>().Where(p => p.Name == "p1").Select(p => Math.Round((decimal)p.Age.Sqrt())).SingleOrDefaultAsync<object>());
					Assert.That(roundedValue, Is.InstanceOf<double>());
					Assert.That(roundedValue, Is.EqualTo(9));
				}
		}

		[Test]
		public async Task RoundDoubleWithTwoArgumentsAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var roundedValue = await (s.QueryOver<Person>().Where(p => p.Name == "p1").Select(p => Math.Round(p.Age.Sqrt(), 3)).SingleOrDefaultAsync<object>());
					Assert.That(roundedValue, Is.InstanceOf<double>());
					Assert.That(roundedValue, Is.EqualTo(9.487).Within(0.000001));
				}
		}

		[Test]
		public async Task RoundDecimalWithTwoArgumentsAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var roundedValue = await (s.QueryOver<Person>().Where(p => p.Name == "p1").Select(p => Math.Round((decimal)p.Age.Sqrt(), 3)).SingleOrDefaultAsync<object>());
					Assert.That(roundedValue, Is.InstanceOf<double>());
					Assert.That(roundedValue, Is.EqualTo(9.487).Within(0.000001));
				}
		}

		[Test]
		public async Task FunctionsToLowerToUpperAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var names = await (s.QueryOver<Person>().Where(p => p.Name == "pP3").Select(p => p.Name.Lower(), p => p.Name.Upper()).SingleOrDefaultAsync<object[]>());
					Assert.That(names[0], Is.EqualTo("pp3"));
					Assert.That(names[1], Is.EqualTo("PP3"));
				}
		}

		[Test]
		public async Task ConcatAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var name = await (s.QueryOver<Person>().Where(p => p.Name == "p1").Select(p => Projections.Concat(p.Name, ", ", p.Name)).SingleOrDefaultAsync<string>());
					Assert.That(name, Is.EqualTo("p1, p1"));
				}
		}

		[Test]
		public async Task MonthPartEqualsDayPartAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = await (s.QueryOver<Person>().Where(p => p.BirthDate.MonthPart() == p.BirthDate.DayPart()).ListAsync());
					Assert.That(persons.Count, Is.EqualTo(1));
					Assert.That(persons[0].Name, Is.EqualTo("p2"));
				}
		}

		[Test]
		public async Task OrderByYearPartAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = await (s.QueryOver<Person>().OrderBy(p => p.BirthDate.YearPart()).Desc.ListAsync());
					Assert.That(persons.Count, Is.EqualTo(3));
					Assert.That(persons[0].Name, Is.EqualTo("p1"));
					Assert.That(persons[1].Name, Is.EqualTo("p2"));
					Assert.That(persons[2].Name, Is.EqualTo("pP3"));
				}
		}

		[Test]
		public async Task YearEqualAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = await (s.QueryOver<Person>().Where(p => p.BirthDate.Year == 2008).ListAsync());
					Assert.That(persons.Count, Is.EqualTo(1));
					Assert.That(persons[0].Name, Is.EqualTo("p2"));
				}
		}

		[Test]
		public async Task YearIsInAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = await (s.QueryOver<Person>().Where(p => p.BirthDate.Year.IsIn(new[]{2008, 2009})).OrderBy(p => p.Name).Asc.ListAsync());
					Assert.That(persons.Count, Is.EqualTo(2));
					Assert.That(persons[0].Name, Is.EqualTo("p1"));
					Assert.That(persons[1].Name, Is.EqualTo("p2"));
				}
		}

		[Test]
		public async Task YearSingleOrDefaultAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var yearOfBirth = await (s.QueryOver<Person>().Where(p => p.Name == "p2").Select(p => p.BirthDate.Year).SingleOrDefaultAsync<object>());
					Assert.That(yearOfBirth.GetType(), Is.EqualTo(typeof (int)));
					Assert.That(yearOfBirth, Is.EqualTo(2008));
				}
		}

		[Test]
		public async Task SelectAvgYearAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var avgYear = await (s.QueryOver<Person>().SelectList(list => list.SelectAvg(p => p.BirthDate.Year)).SingleOrDefaultAsync<object>());
					Assert.That(avgYear.GetType(), Is.EqualTo(typeof (double)));
					Assert.That(string.Format("{0:0}", avgYear), Is.EqualTo("2008"));
				}
		}

		[Test]
		public async Task OrderByYearAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = await (s.QueryOver<Person>().OrderBy(p => p.BirthDate.Year).Desc.ListAsync());
					Assert.That(persons.Count, Is.EqualTo(3));
					Assert.That(persons[0].Name, Is.EqualTo("p1"));
					Assert.That(persons[1].Name, Is.EqualTo("p2"));
					Assert.That(persons[2].Name, Is.EqualTo("pP3"));
				}
		}

		[Test]
		public async Task MonthEqualsDayAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = await (s.QueryOver<Person>().Where(p => p.BirthDate.Month == p.BirthDate.Day).ListAsync());
					Assert.That(persons.Count, Is.EqualTo(1));
					Assert.That(persons[0].Name, Is.EqualTo("p2"));
				}
		}
	}
}
#endif
