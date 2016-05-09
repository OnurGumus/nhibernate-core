using System;
using System.Collections.Generic;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Id;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.UserTypes;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Hql.Ast.ANTLR
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class HqlSqlWalker
	{
		async Task PostProcessInsertAsync(IASTNode insert)
		{
			var insertStatement = (InsertStatement)insert;
			insertStatement.Validate();
			SelectClause selectClause = insertStatement.SelectClause;
			var persister = insertStatement.IntoClause.Queryable;
			if (!insertStatement.IntoClause.IsExplicitIdInsertion)
			{
				// We need to generate ids as part of this bulk insert.
				//
				// Note that this is only supported for sequence-style generators and
				// post-insert-style generators; basically, only in-db generators
				IIdentifierGenerator generator = persister.IdentifierGenerator;
				if (!SupportsIdGenWithBulkInsertion(generator))
				{
					throw new QueryException("can only generate ids as part of bulk insert with either sequence or post-insert style generators");
				}

				IASTNode idSelectExprNode = null;
				var seqGenerator = generator as SequenceGenerator;
				if (seqGenerator != null)
				{
					string seqName = seqGenerator.GeneratorKey();
					string nextval = SessionFactoryHelper.Factory.Dialect.GetSelectSequenceNextValString(seqName);
					idSelectExprNode = ASTFactory.CreateNode(SQL_TOKEN, nextval);
				}
				else
				{
				//Don't need this, because we should never ever be selecting no columns in an insert ... select...
				//and because it causes a bug on DB2
				/*String idInsertString = sessionFactoryHelper.getFactory().getDialect().getIdentityInsertString();
					if ( idInsertString != null ) {
					idSelectExprNode = getASTFactory().create( HqlSqlTokenTypes.SQL_TOKEN, idInsertString );
					}*/
				}

				if (idSelectExprNode != null)
				{
					selectClause.InsertChild(0, idSelectExprNode);
					insertStatement.IntoClause.PrependIdColumnSpec();
				}
			}

			bool includeVersionProperty = persister.IsVersioned && !insertStatement.IntoClause.IsExplicitVersionInsertion && persister.VersionPropertyInsertable;
			if (includeVersionProperty)
			{
				// We need to seed the version value as part of this bulk insert
				IVersionType versionType = persister.VersionType;
				IASTNode versionValueNode;
				if (SessionFactoryHelper.Factory.Dialect.SupportsParametersInInsertSelect)
				{
					versionValueNode = ASTFactory.CreateNode(PARAM, "?");
					IParameterSpecification paramSpec = new VersionTypeSeedParameterSpecification(versionType);
					((ParameterNode)versionValueNode).HqlParameterSpecification = paramSpec;
					_parameters.Insert(0, paramSpec);
				}
				else
				{
					if (IsIntegral(versionType))
					{
						try
						{
							object seedValue = await (versionType.SeedAsync(null));
							versionValueNode = ASTFactory.CreateNode(SQL_TOKEN, seedValue.ToString());
						}
						catch (Exception t)
						{
							throw new QueryException("could not determine seed value for version on bulk insert [" + versionType + "]", t);
						}
					}
					else if (IsDatabaseGeneratedTimestamp(versionType))
					{
						string functionName = SessionFactoryHelper.Factory.Dialect.CurrentTimestampSQLFunctionName;
						versionValueNode = ASTFactory.CreateNode(SQL_TOKEN, functionName);
					}
					else
					{
						throw new QueryException("cannot handle version type [" + versionType + "] on bulk inserts with dialects not supporting parameters in insert-select statements");
					}
				}

				selectClause.InsertChild(0, versionValueNode);
				insertStatement.IntoClause.PrependVersionColumnSpec();
			}

			if (insertStatement.IntoClause.IsDiscriminated)
			{
				string sqlValue = insertStatement.IntoClause.Queryable.DiscriminatorSQLValue;
				IASTNode discrimValue = ASTFactory.CreateNode(SQL_TOKEN, sqlValue);
				insertStatement.SelectClause.AddChild(discrimValue);
			}
		}
	}
}