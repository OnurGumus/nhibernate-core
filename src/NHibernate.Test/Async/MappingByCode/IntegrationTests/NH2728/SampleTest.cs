#if NET_4_5
using System;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH2728
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ConventionModelMapper();
			// Working Example
			//mapper.Class<Toy>(rc => rc.Set(x => x.Animals, cmap => { }, rel => rel.ManyToAny<int>(meta =>
			//                                                                                      {
			//                                                                                        meta.MetaValue(1, typeof (Cat));
			//                                                                                        meta.MetaValue(2, typeof (Dog));
			//                                                                                      })));
			// User needs
			mapper.Class<Toy>(rc => rc.Set(x => x.Animals, cmap =>
			{
				cmap.Table("Animals_Toys");
				cmap.Key(km => km.Column("Cat_Id"));
			}

			, rel => rel.ManyToAny<int>(meta =>
			{
				meta.MetaValue(1, typeof (Cat));
				meta.MetaValue(2, typeof (Dog));
				meta.Columns(cid =>
				{
					cid.Name("Animal_Id");
					cid.NotNullable(true);
				}

				, ctype =>
				{
					ctype.Name("Animal_Type");
					ctype.NotNullable(true);
				}

				);
			}

			)));
			var mappings = mapper.CompileMappingFor(new[]{typeof (Cat), typeof (Dog), typeof (Toy)});
			//Console.WriteLine(mappings.AsString()); // <=== uncomment this line to see the XML mapping
			return mappings;
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = this.OpenSession())
			{
				var cat1 = new Cat()
				{Id = 1, Name = "Cat 1", Born = DateTime.Now, };
				await (session.SaveAsync(cat1));
				var cat2 = new Cat()
				{Id = 2, Name = "Cat 2", Born = DateTime.Now, };
				await (session.SaveAsync(cat2));
				var dog1 = new Dog()
				{Id = 1, Name = "Dog 1", Walks = 11, };
				await (session.SaveAsync(dog1));
				var toy1 = new Toy()
				{Id = 1, Name = "Toy 1", };
				toy1.Animals.Add(cat1);
				toy1.Animals.Add(cat2);
				toy1.Animals.Add(dog1);
				await (session.SaveAsync(toy1));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = this.OpenSession())
			{
				string hql = "from System.Object";
				await (session.DeleteAsync(hql));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task ShouldBeAbleToGetFromToyToAnimalsAsync()
		{
			using (ISession session = this.OpenSession())
			{
				var toy1 = await (session.GetAsync<Toy>(1));
				Assert.AreEqual(1, toy1.Id);
				Assert.AreEqual(3, toy1.Animals.Count);
			}
		}
	}
}
#endif
