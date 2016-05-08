using NHibernate.Engine;
using NHibernate.Id.Insert;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// A generator which combines sequence generation with immediate retrieval
	/// by attaching a output parameter to the SQL command
	/// In this respect it works much like ANSI-SQL IDENTITY generation.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SequenceIdentityGenerator : SequenceGenerator, IPostInsertIdentifierGenerator
	{
		public override async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			return IdentifierGeneratorFactory.PostInsertIndicator;
		}
	}
}