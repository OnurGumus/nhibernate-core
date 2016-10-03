#if NET_4_5
using System;
using System.Linq;
using System.Xml.Linq;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3405
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Cfg.Environment.WrapResultSets, Boolean.TrueString);
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect;
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<XmlTest>(rc =>
			{
				rc.Table("`XML_TEST`");
				rc.Id(x => x.Id, x =>
				{
					x.Column("`ID`");
					x.Generator(Generators.HighLow);
				}

				);
				rc.Property(x => x.Data, x =>
				{
					x.Column("`DATA`");
					x.Type<XDocType>();
					x.NotNullable(true);
				}

				);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var t = new XmlTest{Data = XDocument.Parse("<test>123</test>")};
					await (session.SaveAsync(t));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
