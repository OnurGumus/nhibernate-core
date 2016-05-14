#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Action;
using NHibernate.AdoNet.Util;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Event;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Transaction;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Hql.Ast.ANTLR.Exec
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractStatementExecutor : IStatementExecutor
	{
		public abstract Task<int> ExecuteAsync(QueryParameters parameters, ISessionImplementor session);
	}
}
#endif
