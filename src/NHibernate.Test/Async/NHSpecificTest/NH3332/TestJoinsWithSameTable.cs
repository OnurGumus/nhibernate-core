#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3332
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestJoinsWithSameTableAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (CreateObjectsAsync(session));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from StateDescription"));
					await (session.FlushAsync());
					await (session.DeleteAsync("from DataTypeDescription"));
					await (session.FlushAsync());
					await (session.DeleteAsync("from MasterEntity"));
					await (session.FlushAsync());
					await (session.DeleteAsync("from State"));
					await (session.FlushAsync());
					await (session.DeleteAsync("from DataType"));
					await (session.FlushAsync());
					await (session.DeleteAsync("from Culture"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task TestJoinsAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					// this query should return one row
					var query =
						from me in session.Query<MasterEntity>()from dtd in me.DataType.DataTypeDescriptions
						from std in me.State.StateDescriptions
						where dtd.Culture.CountryCode == "CA" && dtd.Culture.LanguageCode == "en" && std.Culture.CountryCode == "CA" && std.Culture.LanguageCode == "en"
						select new
						{
						me.Id, me.Name, DataTypeName = me.DataType.Name, StateName = me.State.Name
						}

					;
					var list = await (query.ToListAsync());
					Assert.That(list, Has.Count.EqualTo(1));
				}
		}

		private async Task CreateObjectsAsync(ISession session)
		{
			// Create the English culture
			Culture englishCulture = new Culture();
			englishCulture.CountryCode = "CA";
			englishCulture.LanguageCode = "en";
			await (session.SaveOrUpdateAsync(englishCulture));
			await (session.FlushAsync());
			// Create the Spanish culture
			Culture spanishCulture = new Culture();
			spanishCulture.CountryCode = "ES";
			spanishCulture.LanguageCode = "es";
			await (session.SaveOrUpdateAsync(spanishCulture));
			await (session.FlushAsync());
			// Create a DataType and attach it an English description
			DataType dataType1 = new DataType();
			dataType1.Name = "int";
			DataTypeDescription dataTypeDescription1 = new DataTypeDescription();
			dataTypeDescription1.Culture = englishCulture;
			dataTypeDescription1.DataType = dataType1;
			dataType1.DataTypeDescriptions.Add(dataTypeDescription1);
			// Create a State and attach it an English description and a Spanish description
			State state1 = new State();
			state1.Name = "Development";
			StateDescription englishStateDescription = new StateDescription();
			englishStateDescription.Culture = englishCulture;
			englishStateDescription.State = state1;
			//      englishStateDescription.Description = "Development - English";
			state1.StateDescriptions.Add(englishStateDescription);
			StateDescription spanishStateDescription = new StateDescription();
			spanishStateDescription.Culture = spanishCulture;
			spanishStateDescription.State = state1;
			//      spanishStateDescription.Description = "Development - Spanish";
			state1.StateDescriptions.Add(spanishStateDescription);
			MasterEntity masterEntity = new MasterEntity();
			masterEntity.Name = "MasterEntity 1";
			masterEntity.State = state1;
			masterEntity.DataType = dataType1;
			await (session.SaveOrUpdateAsync(masterEntity));
			await (session.FlushAsync());
		}
	}
}
#endif
