using NHibernate.Hql.Ast;
using Remotion.Linq.Clauses.ResultOperators;

namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
    public partial class ProcessAny : IResultOperatorProcessor<AnyResultOperator>
    {
        public void Process(AnyResultOperator anyOperator, QueryModelVisitor queryModelVisitor, IntermediateHqlTree tree)
        {
            tree.SetRoot(tree.TreeBuilder.Exists((HqlQuery) tree.Root));
        }
    }
}