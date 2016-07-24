#if NET_4_5
using NHibernate.Engine.Query;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.EngineTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ParameterParserFixtureAsync
	{
		[Test]
		public Task CanFindParameterAfterCommentAsync()
		{
			try
			{
				string query = @"
SELECT id 
FROM tablea 
/* Comment with ' number 2 */ 
WHERE Name = :name 
ORDER BY Name";
				var recognizer = new ParamLocationRecognizer();
				ParameterParser.Parse(query, recognizer);
				ParamLocationRecognizer.NamedParameterDescription p;
				Assert.DoesNotThrow(() => p = recognizer.NamedParameterDescriptionMap["name"]);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task CanFindParameterAfterInlineCommentAsync()
		{
			try
			{
				string query = @"
SELECT id 
FROM tablea 
-- Comment with ' number 1 
WHERE Name = :name 
ORDER BY Name";
				var recognizer = new ParamLocationRecognizer();
				ParameterParser.Parse(query, recognizer);
				ParamLocationRecognizer.NamedParameterDescription p;
				Assert.DoesNotThrow(() => p = recognizer.NamedParameterDescriptionMap["name"]);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task CanFindParameterAfterAnyCommentAsync()
		{
			try
			{
				string query = @"
SELECT id 
FROM tablea 
-- Comment with ' number 1 
WHERE Name = :name 
/* Comment with ' number 2 */ 
ORDER BY Name + :pizza";
				var recognizer = new ParamLocationRecognizer();
				ParameterParser.Parse(query, recognizer);
				ParamLocationRecognizer.NamedParameterDescription p;
				Assert.DoesNotThrow(() => p = recognizer.NamedParameterDescriptionMap["name"]);
				Assert.DoesNotThrow(() => p = recognizer.NamedParameterDescriptionMap["pizza"]);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
