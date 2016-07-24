#if NET_4_5
using NUnit.Framework;
using NHibernate.Engine.Query;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Test.EngineTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CallableParserFixtureAsync
	{
		[Test]
		public void CanDetermineIsCallable()
		{
			string query = @"{ call myFunction(:name) }";
			CallableParser.Detail detail = CallableParser.Parse(query);
			Assert.That(detail.IsCallable, Is.True);
		}

		[Test]
		public void CanDetermineIsNotCallable()
		{
			string query = @"SELECT id FROM mytable";
			CallableParser.Detail detail = CallableParser.Parse(query);
			Assert.That(detail.IsCallable, Is.False);
		}

		[Test]
		public void CanFindCallableFunctionName()
		{
			string query = @"{ call myFunction(:name) }";
			CallableParser.Detail detail = CallableParser.Parse(query);
			Assert.That(detail.FunctionName, Is.EqualTo("myFunction"));
		}

		[Test]
		public void CanFindCallableFunctionNameWithoutParameters()
		{
			string query = @"{ call myFunction }";
			CallableParser.Detail detail = CallableParser.Parse(query);
			Assert.That(detail.FunctionName, Is.EqualTo("myFunction"));
		}

		[Test]
		public void CanFindCallablePackageFunctionName()
		{
			string query = @"{ call myPackage.No2_Function(:name) }";
			CallableParser.Detail detail = CallableParser.Parse(query);
			Assert.That(detail.FunctionName, Is.EqualTo("myPackage.No2_Function"));
		}

		[Test]
		public void CanDetermineHasReturn()
		{
			string query = @"{ ? = call myFunction(:name) }";
			CallableParser.Detail detail = CallableParser.Parse(query);
			Assert.That(detail.HasReturn, Is.True);
		}

		[Test]
		public void CanDetermineHasNoReturn()
		{
			string query = @"{ call myFunction(:name) }";
			CallableParser.Detail detail = CallableParser.Parse(query);
			Assert.That(detail.HasReturn, Is.False);
		}
	}
}
#endif
