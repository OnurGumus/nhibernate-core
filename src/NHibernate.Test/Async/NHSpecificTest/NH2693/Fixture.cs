#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2693
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private List<FirstLevel> _firstLevels;
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var firstLevel1 = new FirstLevel();
					var secondLevel1 = new SecondLevelComponent{FirstLevel = firstLevel1};
					firstLevel1.SecondLevels.Add(secondLevel1);
					var thirdLevel1 = new SpecificThirdLevel();
					secondLevel1.ThirdLevel = thirdLevel1;
					secondLevel1.SpecificThirdLevel = thirdLevel1;
					var fourthLevel1 = new FourthLevel{SomeString = "first", SpecificThirdLevel = thirdLevel1};
					thirdLevel1.FourthLevels.Add(fourthLevel1);
					var fourthLevel2 = new FourthLevel{SomeString = "second", SpecificThirdLevel = thirdLevel1};
					thirdLevel1.FourthLevels.Add(fourthLevel2);
					var firstLevel2 = new FirstLevel();
					var secondLevel2 = new SecondLevelComponent{FirstLevel = firstLevel2};
					firstLevel2.SecondLevels.Add(secondLevel2);
					var thirdLevel2 = new SpecificThirdLevel();
					secondLevel2.ThirdLevel = thirdLevel2;
					secondLevel2.SpecificThirdLevel = thirdLevel2;
					var fourthLevel3 = new FourthLevel{SomeString = "third", SpecificThirdLevel = thirdLevel2};
					thirdLevel2.FourthLevels.Add(fourthLevel3);
					var fourthLevel4 = new FourthLevel{SomeString = "fourth", SpecificThirdLevel = thirdLevel2};
					thirdLevel2.FourthLevels.Add(fourthLevel4);
					var firstLevel3 = new FirstLevel();
					var secondLevel3 = new SecondLevelComponent{FirstLevel = firstLevel3};
					firstLevel3.SecondLevels.Add(secondLevel3);
					var thirdLevel3 = new SpecificThirdLevel();
					secondLevel3.ThirdLevel = thirdLevel3;
					secondLevel3.SpecificThirdLevel = thirdLevel3;
					await (session.SaveAsync(thirdLevel1));
					await (session.SaveAsync(thirdLevel2));
					await (session.SaveAsync(thirdLevel3));
					await (session.SaveAsync(firstLevel1));
					await (session.SaveAsync(firstLevel2));
					await (session.SaveAsync(firstLevel3));
					_firstLevels = new List<FirstLevel>();
					_firstLevels.Add(firstLevel1);
					_firstLevels.Add(firstLevel2);
					_firstLevels.Add(firstLevel3);
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from FourthLevel"));
					await (session.DeleteAsync("from ThirdLevel"));
					await (session.DeleteAsync("from FirstLevel"));
					await (tx.CommitAsync());
				}
			}
		}

		[Test, Ignore("Not fixed yet")]
		public void _1_Querying_BasedOnFourthLevelExistence_WithIsAndCasting_ShouldReturnSameEntitiesAsLinqToObjects()
		{
			var expected = _firstLevels.Where(first => first.SecondLevels.Any(second => second.ThirdLevel is SpecificThirdLevel && ((SpecificThirdLevel)second.ThirdLevel).FourthLevels.Any())).ToList();
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<FirstLevel>().Where(first => first.SecondLevels.Any(second => second.ThirdLevel is SpecificThirdLevel && ((SpecificThirdLevel)second.ThirdLevel).FourthLevels.Any())).ToList();
					Assert.AreEqual(expected.Count, result.Count);
					Assert.IsTrue(result.All(f => f.SecondLevels.Any(s => ((SpecificThirdLevel)s.ThirdLevel).FourthLevels.Any())));
				}
			}
		}

		[Test, Ignore("Not fixed yet")]
		public void _2_Querying_BasedOnFourthLevelExistence_WithSelectAndOfType_ShouldReturnSameEntitiesAsLinqToObjects()
		{
			var expected = _firstLevels.Where(first => first.SecondLevels.Select(second => second.ThirdLevel).OfType<SpecificThirdLevel>().Any(specificThird => specificThird.FourthLevels.Any())).ToList();
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<FirstLevel>().Where(first => first.SecondLevels.Select(second => second.ThirdLevel).OfType<SpecificThirdLevel>().Any(specificThird => specificThird.FourthLevels.Any())).ToList();
					Assert.AreEqual(expected.Count, result.Count);
					Assert.IsTrue(result.All(f => f.SecondLevels.Any(s => ((SpecificThirdLevel)s.ThirdLevel).FourthLevels.Any())));
				}
			}
		}

		[Test, Ignore("Not fixed yet")]
		public void _3_Querying_BasedOnFourthLevelProperty_WithIsAndCasting_ShouldReturnSameEntitiesAsLinqToObjects()
		{
			var expected = _firstLevels.Where(first => first.SecondLevels.Any(second => second.ThirdLevel is SpecificThirdLevel && ((SpecificThirdLevel)second.ThirdLevel).FourthLevels.Any(fourth => fourth.SomeString == "first"))).ToList();
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<FirstLevel>().Where(first => first.SecondLevels.Any(second => second.ThirdLevel is SpecificThirdLevel && ((SpecificThirdLevel)second.ThirdLevel).FourthLevels.Any(fourth => fourth.SomeString == "first"))).ToList();
					Assert.AreEqual(expected.Count, result.Count);
					Assert.IsTrue(result.All(f => f.SecondLevels.Any(s => ((SpecificThirdLevel)s.ThirdLevel).FourthLevels.Any(fo => fo.SomeString == "first"))));
				}
			}
		}

		[Test, Ignore("Not fixed yet")]
		public void _4_Querying_BasedOnFourthLevelProperty_WithSelectAndOfType_ShouldReturnSameEntitiesAsLinqToObjects()
		{
			var expected = _firstLevels.Where(first => first.SecondLevels.Select(second => second.ThirdLevel).OfType<SpecificThirdLevel>().Any(specificThird => specificThird.FourthLevels.Any(fourth => fourth.SomeString == "first"))).ToList();
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<FirstLevel>().Where(first => first.SecondLevels.Select(second => second.ThirdLevel).OfType<SpecificThirdLevel>().Any(specificThird => specificThird.FourthLevels.Any(fourth => fourth.SomeString == "first"))).ToList();
					Assert.AreEqual(expected.Count, result.Count);
					Assert.IsTrue(result.All(f => f.SecondLevels.Any(s => ((SpecificThirdLevel)s.ThirdLevel).FourthLevels.Any(fo => fo.SomeString == "first"))));
				}
			}
		}

		[Test]
		public void _5_Querying_BasedOnFourthLevelExistence_ByUsingSpecificThirdLevelProperty_ShouldReturnSameEntitiesAsLinqToObjects()
		{
			var expected = _firstLevels.Where(first => first.SecondLevels.Any(second => second.SpecificThirdLevel.FourthLevels.Any())).ToList();
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<FirstLevel>().Where(first => first.SecondLevels.Any(second => second.SpecificThirdLevel.FourthLevels.Any())).ToList();
					Assert.AreEqual(expected.Count, result.Count);
					Assert.IsTrue(result.All(f => f.SecondLevels.Any(second => second.SpecificThirdLevel.FourthLevels.Any())));
				}
			}
		}

		[Test]
		public void _6_Querying_BasedOnFourthLevelProperty_ByUsingSpecificThirdLevelProperty_ShouldReturnSameEntitiesAsLinqToObjects()
		{
			var expected = _firstLevels.Where(first => first.SecondLevels.Any(second => second.SpecificThirdLevel.FourthLevels.Any(fourth => fourth.SomeString == "first"))).ToList();
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<FirstLevel>().Where(first => first.SecondLevels.Any(second => second.SpecificThirdLevel.FourthLevels.Any(fourth => fourth.SomeString == "first"))).ToList();
					Assert.AreEqual(expected.Count, result.Count);
					Assert.IsTrue(result.All(f => f.SecondLevels.Any(second => second.SpecificThirdLevel.FourthLevels.Any(fourth => fourth.SomeString == "first"))));
				}
			}
		}
	}
}
#endif
