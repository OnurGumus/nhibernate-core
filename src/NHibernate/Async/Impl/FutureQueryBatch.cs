using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FutureQueryBatch : FutureBatch<IQuery, IMultiQuery>
	{
		protected override async Task<IList> GetResultsFromAsync(IMultiQuery multiApproach)
		{
			return await (multiApproach.ListAsync());
		}
	}
}