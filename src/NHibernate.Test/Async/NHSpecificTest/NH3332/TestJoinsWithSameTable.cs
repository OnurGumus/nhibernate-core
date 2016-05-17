#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3332
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestJoinsWithSameTable : BugTestCase
	{
		private async Task CreateObjectsAsync(ISession session)
		{
			// Create the English culture
			Culture englishCulture = new Culture();
			englishCulture.CountryCode = "CA";
			englishCulture.LanguageCode = "en";
			session.SaveOrUpdate(englishCulture);
			await (session.FlushAsync());
			// Create the Spanish culture
			Culture spanishCulture = new Culture();
			spanishCulture.CountryCode = "ES";
			spanishCulture.LanguageCode = "es";
			session.SaveOrUpdate(spanishCulture);
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
			session.SaveOrUpdate(masterEntity);
			await (session.FlushAsync());
		}
	}
}
#endif
