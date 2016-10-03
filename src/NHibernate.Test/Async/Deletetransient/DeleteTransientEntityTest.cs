#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Deletetransient
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DeleteTransientEntityTestAsync : TestCaseAsync
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
				return new string[]{"Deletetransient.Person.hbm.xml"};
			}
		}

		[Test]
		public async Task TransientEntityDeletionNoCascadesAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.DeleteAsync(new Address()));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task TransientEntityDeletionCascadingToTransientAssociationAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Person p = new Person();
			p.Addresses.Add(new Address());
			await (s.DeleteAsync(p));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task TransientEntityDeleteCascadingToCircularityAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Person p1 = new Person();
			Person p2 = new Person();
			p1.Friends.Add(p2);
			p2.Friends.Add(p1);
			await (s.DeleteAsync(p1));
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task TransientEntityDeletionCascadingToDetachedAssociationAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Address address = new Address();
			address.Info = "123 Main St.";
			await (s.SaveAsync(address));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			Person p = new Person();
			p.Addresses.Add(address);
			await (s.DeleteAsync(p));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			long count = (await (s.CreateQuery("select count(*) from Address").ListAsync<long>()))[0];
			Assert.That(count, Is.EqualTo(0L), "delete not cascaded properly across transient entity");
			await (t.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task TransientEntityDeletionCascadingToPersistentAssociationAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Address address = new Address();
			address.Info = "123 Main St.";
			await (s.SaveAsync(address));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			address = await (s.GetAsync<Address>(address.Id));
			Person p = new Person();
			p.Addresses.Add(address);
			await (s.DeleteAsync(p));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			long count = (await (s.CreateQuery("select count(*) from Address").ListAsync<long>()))[0];
			Assert.That(count, Is.EqualTo(0L), "delete not cascaded properly across transient entity");
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
