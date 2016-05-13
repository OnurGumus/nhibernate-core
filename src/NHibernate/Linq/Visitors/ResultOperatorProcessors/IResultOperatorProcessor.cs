namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
	public partial interface IResultOperatorProcessor<T>
	{
		void Process(T resultOperator, QueryModelVisitor queryModelVisitor, IntermediateHqlTree tree);
	}
}