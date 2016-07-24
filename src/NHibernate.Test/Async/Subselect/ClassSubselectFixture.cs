#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Subselect
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ClassSubselectFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"Subselect.Beings.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		[Test]
		public async Task EntitySubselectAsync()
		{
			var s = OpenSession();
			var t = s.BeginTransaction();
			Human gavin = new Human();
			gavin.Name = "gavin";
			gavin.Sex = 'M';
			gavin.Address = "Melbourne, Australia";
			Alien x23y4 = new Alien();
			x23y4.Identity = "x23y4$$hu%3";
			x23y4.Planet = "Mars";
			x23y4.Species = "martian";
			await (s.SaveAsync(gavin));
			await (s.SaveAsync(x23y4));
			await (s.FlushAsync());
			var beings = await (s.CreateQuery("from Being").ListAsync<Being>());
			Assert.That(beings, Has.Count.GreaterThan(0));
			foreach (var being in beings)
			{
				Assert.That(being.Location, Is.Not.Null.And.Not.Empty);
				Assert.That(being.Identity, Is.Not.Null.And.Not.Empty);
				Assert.That(being.Species, Is.Not.Null.And.Not.Empty);
			}

			s.Clear();
			Sfi.Evict(typeof (Being));
			Being gav = await (s.GetAsync<Being>(gavin.Id));
			Assert.That(gav.Location, Is.Not.Null.And.Not.Empty);
			Assert.That(gav.Identity, Is.Not.Null.And.Not.Empty);
			Assert.That(gav.Species, Is.Not.Null.And.Not.Empty);
			s.Clear();
			//test the <synchronized> tag:
			gavin = await (s.GetAsync<Human>(gavin.Id));
			gavin.Address = "Atlanta, GA";
			gav = await (s.CreateQuery("from Being b where b.Location like '%GA%'").UniqueResultAsync<Being>());
			Assert.That(gav.Location, Is.EqualTo(gavin.Address));
			await (s.DeleteAsync(gavin));
			await (s.DeleteAsync(x23y4));
			Assert.That(await (s.CreateQuery("from Being").ListAsync<Being>()), Is.Empty);
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
