using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Util;

namespace NHibernate.Hql.Ast
{
	public partial class HqlTreeNode
	{
		public IASTFactory Factory { get; private set; }
		private readonly IASTNode _node;
		private readonly List<HqlTreeNode> _children;

		protected HqlTreeNode(int type, string text, IASTFactory factory, IEnumerable<HqlTreeNode> children)
		{
			Factory = factory;
			_node = factory.CreateNode(type, text);
			_children = new List<HqlTreeNode>();

			AddChildren(children);
		}

		protected HqlTreeNode(int type, string text, IASTFactory factory, params HqlTreeNode[] children) : this(type, text, factory, (IEnumerable<HqlTreeNode>) children)
		{
		}

		private void AddChildren(IEnumerable<HqlTreeNode> children)
		{
			foreach (var child in children)
			{
				if (child != null)
				{
					AddChild(child);
				}
			}
		}

		public IEnumerable<HqlTreeNode> NodesPreOrder
		{
			get
			{
				yield return this;

				foreach (var child in _children)
				{
					foreach (var node in child.NodesPreOrder)
					{
						yield return node;
					}
				}
			}
		}

		public IEnumerable<HqlTreeNode> NodesPostOrder
		{
			get
			{
				foreach (var child in _children)
				{
					foreach (var node in child.NodesPostOrder)
					{
						yield return node;
					}
				}

				yield return this;
			}
		}

		public IEnumerable<HqlTreeNode> Children
		{
			get { return _children; }
		}

		public void ClearChildren()
		{
			_children.Clear();
			_node.ClearChildren();
		}

		protected void SetText(string text)
		{
			_node.Text = text;
		}

		internal IASTNode AstNode
		{
			get { return _node; }
		}

		internal void AddChild(HqlTreeNode child)
		{
			if (child is HqlExpressionSubTreeHolder) 
			{
				AddChildren(child.Children);
			}
			else
			{
				_children.Add(child);
				_node.AddChild(child.AstNode);
			}
		}
	}

	public static partial class HqlTreeNodeExtensions
	{
		public static HqlExpression AsExpression(this HqlTreeNode node)
		{
			// TODO - nice error handling if cast fails
			return (HqlExpression)node;
		}

		public static HqlBooleanExpression AsBooleanExpression(this HqlTreeNode node)
		{
			if (node is HqlDot)
			{
				return new HqlBooleanDot(node.Factory, (HqlDot) node);
			}

			// TODO - nice error handling if cast fails
			return (HqlBooleanExpression)node;
		}
	}

	public abstract partial class HqlStatement : HqlTreeNode
	{
		protected HqlStatement(int type, string text, IASTFactory factory, params HqlTreeNode[] children)
			: base(type, text, factory, children)
		{
		}

		protected HqlStatement(int type, string text, IASTFactory factory, IEnumerable<HqlTreeNode> children)
			: base(type, text, factory, children)
		{
		}
	}

	public abstract partial class HqlExpression : HqlTreeNode
	{
		protected HqlExpression(int type, string text, IASTFactory factory, IEnumerable<HqlTreeNode> children)
			: base(type, text, factory, children)
		{
		}

		protected HqlExpression(int type, string text, IASTFactory factory, params HqlTreeNode[] children)
			: base(type, text, factory, children)
		{
		}
	}

	public abstract partial class HqlBooleanExpression : HqlExpression
	{
		protected HqlBooleanExpression(int type, string text, IASTFactory factory, IEnumerable<HqlTreeNode> children)
			: base(type, text, factory, children)
		{
		}

		protected HqlBooleanExpression(int type, string text, IASTFactory factory, params HqlTreeNode[] children)
			: base(type, text, factory, children)
		{
		}
	}

	public partial class HqlQuery : HqlExpression
	{
		internal HqlQuery(IASTFactory factory, params HqlStatement[] children)
			: base(HqlSqlWalker.QUERY, "query", factory, children)
		{
		}
	}

	public partial class HqlIdent : HqlExpression
	{
		internal HqlIdent(IASTFactory factory, string ident)
			: base(HqlSqlWalker.IDENT, ident, factory)
		{
		}

		internal HqlIdent(IASTFactory factory, System.Type type)
			: base(HqlSqlWalker.IDENT, "", factory)
		{
			type = type.UnwrapIfNullable();

			switch (System.Type.GetTypeCode(type))
			{
				case TypeCode.Boolean:
					SetText("bool");
					break;
				case TypeCode.Int16:
					SetText("short");
					break;
				case TypeCode.Int32:
					SetText("integer");
					break;
				case TypeCode.Int64:
					SetText("long");
					break;
				case TypeCode.Decimal:
					SetText("decimal");
					break;
				case TypeCode.Single:
					SetText("single");
					break;
				case TypeCode.DateTime:
					SetText("datetime");
					break;
				case TypeCode.String:
					SetText("string");
					break;
				case TypeCode.Double:
					SetText("double");
					break;
				default:
					if (type == typeof(Guid))
					{
						SetText("guid");
						break;
					}
					if (type == typeof(DateTimeOffset))
					{
					    SetText("datetimeoffset");
					    break;
					}
					throw new NotSupportedException(string.Format("Don't currently support idents of type {0}", type.Name));
			}
		}
	}

	public partial class HqlRange : HqlStatement
	{
		internal HqlRange(IASTFactory factory, params HqlTreeNode[] children)
			: base(HqlSqlWalker.RANGE, "range", factory, children)
		{
		}
	}

	public partial class HqlFrom : HqlStatement
	{
		internal HqlFrom(IASTFactory factory, params HqlTreeNode[] children)
			: base(HqlSqlWalker.FROM, "from", factory, children)
		{
		}
	}

	public partial class HqlSelectFrom : HqlStatement
	{
		internal HqlSelectFrom(IASTFactory factory, params HqlTreeNode[] children)
			: base(HqlSqlWalker.SELECT_FROM, "select_from", factory, children)
		{
		}
	}

	public partial class HqlAlias : HqlExpression
	{
		public HqlAlias(IASTFactory factory, string @alias)
			: base(HqlSqlWalker.ALIAS, alias, factory)
		{
		}
	}

	public partial class HqlDivide : HqlExpression
	{
		public HqlDivide(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.DIV, "/", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlMultiplty : HqlExpression
	{
		public HqlMultiplty(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.STAR, "*", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlSubtract : HqlExpression
	{
		public HqlSubtract(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.MINUS, "-", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlAdd : HqlExpression
	{
		public HqlAdd(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.PLUS, "+", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlBooleanOr : HqlBooleanExpression
	{
		public HqlBooleanOr(IASTFactory factory, HqlBooleanExpression lhs, HqlBooleanExpression rhs)
			: base(HqlSqlWalker.OR, "or", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlBooleanAnd : HqlBooleanExpression
	{
		public HqlBooleanAnd(IASTFactory factory, HqlBooleanExpression lhs, HqlBooleanExpression rhs)
			: base(HqlSqlWalker.AND, "and", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlEquality : HqlBooleanExpression
	{
		public HqlEquality(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.EQ, "==", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlParameter : HqlExpression
	{
		public HqlParameter(IASTFactory factory, string name)
			: base(HqlSqlWalker.COLON, ":", factory)
		{
			AddChild(new HqlIdent(factory, name));
		}
	}

	public partial class HqlDot : HqlExpression
	{
		public HqlDot(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.DOT, ".", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlBooleanDot : HqlBooleanExpression
	{
		public HqlBooleanDot(IASTFactory factory, HqlDot dot) : base(dot.AstNode.Type, dot.AstNode.Text, factory, dot.Children)
		{
		}
	}

	public partial class HqlWhere : HqlStatement
	{
		public HqlWhere(IASTFactory factory, HqlExpression expression)
			: base(HqlSqlWalker.WHERE, "where", factory, expression)
		{
		}
	}

	public partial class HqlWith : HqlStatement
	{
		public HqlWith(IASTFactory factory, HqlExpression expression)
			: base(HqlSqlWalker.WITH, "with", factory, expression)
		{
		}
	}

	public partial class HqlHaving : HqlStatement
	{
		public HqlHaving(IASTFactory factory, HqlExpression expression)
			: base(HqlSqlWalker.HAVING, "having", factory, expression)
		{
		}
	}

	public partial class HqlSkip : HqlStatement
	{
		public HqlSkip(IASTFactory factory, HqlExpression parameter)
			: base(HqlSqlWalker.SKIP, "skip", factory, parameter) { }
	}

	public partial class HqlTake : HqlStatement
	{
		public HqlTake(IASTFactory factory, HqlExpression parameter)
			: base(HqlSqlWalker.TAKE, "take", factory, parameter) {}
	}

	public partial class HqlConstant : HqlExpression
	{
		public HqlConstant(IASTFactory factory, int type, string value)
			: base(type, value, factory)
		{
		}
	}

	public partial class HqlStringConstant : HqlConstant
	{
		public HqlStringConstant(IASTFactory factory, string s)
			: base(factory, HqlSqlWalker.QUOTED_String, s)
		{
		}
	}

	public partial class HqlDoubleConstant : HqlConstant
	{
		public HqlDoubleConstant(IASTFactory factory, string s)
			: base(factory, HqlSqlWalker.NUM_DOUBLE, s)
		{
		}
	}

	public partial class HqlFloatConstant : HqlConstant
	{
		public HqlFloatConstant(IASTFactory factory, string s)
			: base(factory, HqlSqlWalker.NUM_FLOAT, s)
		{
		}
	}

	public partial class HqlIntegerConstant : HqlConstant
	{
		public HqlIntegerConstant(IASTFactory factory, string s)
			: base(factory, HqlSqlWalker.NUM_INT, s)
		{
		}
	}

	public partial class HqlDecimalConstant : HqlConstant
	{
		public HqlDecimalConstant(IASTFactory factory, string s)
			: base(factory, HqlSqlWalker.NUM_DECIMAL, s)
		{
		}
	}

	public partial class HqlFalse : HqlConstant
	{
		public HqlFalse(IASTFactory factory)
			: base(factory, HqlSqlWalker.FALSE, "false")
		{
		}
	}

	public partial class HqlTrue : HqlConstant
	{
		public HqlTrue(IASTFactory factory)
			: base(factory, HqlSqlWalker.TRUE, "true")
		{
		}
	}

	public partial class HqlNull : HqlConstant
	{
		public HqlNull(IASTFactory factory)
			: base(factory, HqlSqlWalker.NULL, "null")
		{
		}
	}

	public partial class HqlOrderBy : HqlStatement
	{
		public HqlOrderBy(IASTFactory factory)
			: base(HqlSqlWalker.ORDER, "order by", factory)
		{
		}
	}

	public enum HqlDirection
	{
		Ascending,
		Descending
	}

	public partial class HqlDirectionStatement : HqlStatement
	{
		public HqlDirectionStatement(int type, string text, IASTFactory factory)
			: base(type, text, factory)
		{
		}
	}

	public partial class HqlDirectionAscending : HqlDirectionStatement
	{
		public HqlDirectionAscending(IASTFactory factory)
			: base(HqlSqlWalker.ASCENDING, "asc", factory)
		{
		}
	}

	public partial class HqlDirectionDescending : HqlDirectionStatement
	{
		public HqlDirectionDescending(IASTFactory factory)
			: base(HqlSqlWalker.DESCENDING, "desc", factory)
		{
		}
	}

	public partial class HqlSelect : HqlStatement
	{
		public HqlSelect(IASTFactory factory, params HqlExpression[] expression)
			: base(HqlSqlWalker.SELECT, "select", factory, expression)
		{
		}
	}

	public partial class HqlElse : HqlStatement
	{
		public HqlElse(IASTFactory factory, HqlExpression ifFalse)
			: base(HqlSqlWalker.ELSE, "else", factory, ifFalse)
		{
		}
	}

	public partial class HqlWhen : HqlStatement
	{
		public HqlWhen(IASTFactory factory, HqlExpression predicate, HqlExpression ifTrue)
			: base(HqlSqlWalker.WHEN, "when", factory, predicate, ifTrue)
		{
		}
	}

	public partial class HqlCase : HqlExpression
	{
		public HqlCase(IASTFactory factory, HqlWhen[] whenClauses, HqlExpression ifFalse)
			: base(HqlSqlWalker.CASE, "case", factory, whenClauses)
		{
			if (ifFalse != null)
			{
				AddChild(new HqlElse(factory, ifFalse));
			}
		}
	}

	public partial class HqlGreaterThanOrEqual : HqlBooleanExpression
	{
		public HqlGreaterThanOrEqual(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.GE, "ge", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlGreaterThan : HqlBooleanExpression
	{
		public HqlGreaterThan(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.GT, "gt", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlLessThanOrEqual : HqlBooleanExpression
	{
		public HqlLessThanOrEqual(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.LE, "le", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlLessThan : HqlBooleanExpression
	{
		public HqlLessThan(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.LT, "lt", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlInequality : HqlBooleanExpression
	{
		public HqlInequality(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.NE, "ne", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlRowStar : HqlStatement
	{
		public HqlRowStar(IASTFactory factory)
			: base(HqlSqlWalker.ROW_STAR, "*", factory)
		{
		}
	}

	public partial class HqlCount : HqlExpression
	{
		public HqlCount(IASTFactory factory)
			: base(HqlSqlWalker.COUNT, "count", factory)
		{
		}

		public HqlCount(IASTFactory factory, HqlExpression child)
			: base(HqlSqlWalker.COUNT, "count", factory, child)
		{
		}
	}

	public partial class HqlAs : HqlExpression
	{
		public HqlAs(IASTFactory factory, HqlExpression expression, System.Type type)
			: base(HqlSqlWalker.AS, "as", factory, expression)
		{
			switch (System.Type.GetTypeCode(type))
			{
				case TypeCode.Int32:
					AddChild(new HqlIdent(factory, "integer"));
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}

	public partial class HqlCast : HqlExpression
	{
		public HqlCast(IASTFactory factory, HqlExpression expression, System.Type type)
			: base(HqlSqlWalker.METHOD_CALL, "method", factory)
		{
			AddChild(new HqlIdent(factory, "cast"));
			AddChild(new HqlExpressionList(factory, expression, new HqlIdent(factory, type)));
		}
	}

	public partial class HqlCoalesce : HqlExpression
	{
		public HqlCoalesce(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.METHOD_CALL, "coalesce", factory)
		{
			AddChild(new HqlIdent(factory, "coalesce"));
			AddChild(new HqlExpressionList(factory, lhs, rhs));
		}
	}

	public partial class HqlDictionaryIndex : HqlExpression
	{
		public HqlDictionaryIndex(IASTFactory factory, HqlExpression dictionary, HqlExpression index)
			: base(HqlSqlWalker.INDEX_OP, "[", factory, dictionary, index)
		{
		}
	}

	public partial class HqlIndices : HqlExpression
	{
		public HqlIndices(IASTFactory factory, HqlExpression dictionary)
			: base(HqlSqlWalker.INDICES, "indices", factory, dictionary)
		{
		}
	}

	public partial class HqlExpressionList : HqlStatement
	{
		public HqlExpressionList(IASTFactory factory, params HqlExpression[] expressions)
			: base(HqlSqlWalker.EXPR_LIST, "expr_list", factory, expressions)
		{
		}

		public HqlExpressionList(IASTFactory factory, IEnumerable<HqlExpression> expressions)
			: base(HqlSqlWalker.EXPR_LIST, "expr_list", factory, expressions.Cast<HqlTreeNode>())
		{
		}
	}

	public partial class HqlBooleanNot : HqlBooleanExpression
	{
		public HqlBooleanNot(IASTFactory factory, HqlBooleanExpression operand)
			: base(HqlSqlWalker.NOT, "not", factory, operand)
		{
		}
	}

	public partial class HqlAverage : HqlExpression
	{
		public HqlAverage(IASTFactory factory, HqlExpression expression)
			: base(HqlSqlWalker.AGGREGATE, "avg", factory, expression)
		{
		}
	}

	public partial class HqlBitwiseNot : HqlExpression
	{
		public HqlBitwiseNot(IASTFactory factory) : base(HqlSqlWalker.BNOT, "not", factory)
		{
		}
	}

	public partial class HqlSum : HqlExpression
	{
		public HqlSum(IASTFactory factory)
			: base(HqlSqlWalker.AGGREGATE, "sum", factory)
		{
		}

		public HqlSum(IASTFactory factory, HqlExpression expression)
			: base(HqlSqlWalker.AGGREGATE, "sum", factory, expression)
		{
		}
	}

	public partial class HqlMax : HqlExpression
	{
		public HqlMax(IASTFactory factory, HqlExpression expression)
			: base(HqlSqlWalker.AGGREGATE, "max", factory, expression)
		{
		}
}

	public partial class HqlMin : HqlExpression
	{
		public HqlMin(IASTFactory factory, HqlExpression expression)
			: base(HqlSqlWalker.AGGREGATE, "min", factory, expression)
		{
		}
	}

	public partial class HqlJoin : HqlStatement
	{
		public HqlJoin(IASTFactory factory, HqlExpression expression, HqlAlias @alias) : base(HqlSqlWalker.JOIN, "join", factory, expression, @alias)
		{
		}
	}

	public partial class HqlLeftJoin : HqlTreeNode
	{
		public HqlLeftJoin(IASTFactory factory, HqlExpression expression, HqlAlias @alias) : base(HqlSqlWalker.JOIN, "join", factory, new HqlLeft(factory), expression, @alias)
		{
		}
	}

	public partial class HqlFetchJoin : HqlTreeNode
	{
		public HqlFetchJoin(IASTFactory factory, HqlExpression expression, HqlAlias @alias)
			: base(HqlSqlWalker.JOIN, "join", factory, new HqlFetch(factory), expression, @alias)
		{
		}
	}

	public partial class HqlLeftFetchJoin : HqlTreeNode
	{
		public HqlLeftFetchJoin(IASTFactory factory, HqlExpression expression, HqlAlias @alias)
			: base(HqlSqlWalker.JOIN, "join", factory, new HqlLeft(factory), new HqlFetch(factory), expression, @alias)
		{
		}
	}

	public partial class HqlFetch : HqlTreeNode
	{
		public HqlFetch(IASTFactory factory) : base(HqlSqlWalker.FETCH, "fetch", factory)
		{
		}
	}

	public partial class HqlClass : HqlExpression
	{
		public HqlClass(IASTFactory factory)
			: base(HqlSqlWalker.CLASS, "class", factory)
		{
		}
	}

	public partial class HqlBitwiseOr : HqlExpression
	{
		public HqlBitwiseOr(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.BOR, "bor", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlBitwiseAnd : HqlExpression
	{
		public HqlBitwiseAnd(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.BAND, "band", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlLeft : HqlTreeNode
	{
		public HqlLeft(IASTFactory factory)
			: base(HqlSqlWalker.LEFT, "left", factory)
		{
		}
	}

	public partial class HqlAny : HqlBooleanExpression
	{
		public HqlAny(IASTFactory factory) : base(HqlSqlWalker.ANY, "any", factory)
		{
		}
	}

	public partial class HqlExists : HqlBooleanExpression
	{
		public HqlExists(IASTFactory factory, HqlQuery query) : base(HqlSqlWalker.EXISTS, "exists", factory, query)
		{
		}
	}

	public partial class HqlElements : HqlBooleanExpression
	{
		public HqlElements(IASTFactory factory) : base(HqlSqlWalker.ELEMENTS, "elements", factory)
		{
		}
	}

	public partial class HqlDistinct : HqlStatement
	{
		public HqlDistinct(IASTFactory factory) : base(HqlSqlWalker.DISTINCT, "distinct", factory)
		{
		}
	}

	public partial class HqlGroupBy : HqlStatement
	{
		public HqlGroupBy(IASTFactory factory, params HqlExpression[] expressions) : base(HqlSqlWalker.GROUP, "group by", factory, expressions)
		{
		}
	}

	public partial class HqlAll : HqlBooleanExpression
	{
		public HqlAll(IASTFactory factory)
			: base(HqlSqlWalker.ALL, "all", factory)
		{
		}
	}

	public partial class HqlLike : HqlBooleanExpression
	{
		public HqlLike(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
			: base(HqlSqlWalker.LIKE, "like", factory, lhs, rhs)
		{
		}
	}

	public partial class HqlConcat : HqlExpression
	{
		public HqlConcat(IASTFactory factory, params HqlExpression[] args)
			: base(HqlSqlWalker.METHOD_CALL, "method", factory)
		{
			AddChild(new HqlIdent(factory, "concat"));
			AddChild(new HqlExpressionList(factory, args));
		}
	}

	public partial class HqlMethodCall : HqlExpression
	{
		public HqlMethodCall(IASTFactory factory, string methodName, IEnumerable<HqlExpression> parameters)
			: base(HqlSqlWalker.METHOD_CALL, "method", factory)
		{
			AddChild(new HqlIdent(factory, methodName));
			AddChild(new HqlExpressionList(factory, parameters));
		}
	}

	public partial class HqlBooleanMethodCall : HqlBooleanExpression
	{
		public HqlBooleanMethodCall(IASTFactory factory, string methodName, IEnumerable<HqlExpression> parameters)
			: base(HqlSqlWalker.METHOD_CALL, "method", factory)
		{
			AddChild(new HqlIdent(factory, methodName));
			AddChild(new HqlExpressionList(factory, parameters));
		}
	}

	public partial class HqlExpressionSubTreeHolder : HqlExpression
	{
		public HqlExpressionSubTreeHolder(IASTFactory factory, HqlTreeNode[] children) : base(int.MinValue, "expression sub-tree holder", factory, children)
		{
		}

		public HqlExpressionSubTreeHolder(IASTFactory factory, IEnumerable<HqlTreeNode> children) : base(int.MinValue, "expression sub-tree holder", factory, children)
		{
		}
	}

	public partial class HqlIsNull : HqlBooleanExpression
	{
		public HqlIsNull(IASTFactory factory, HqlExpression lhs)
			: base(HqlSqlWalker.IS_NULL, "is null", factory, lhs)
		{
		}
	}

	public partial class HqlIsNotNull : HqlBooleanExpression
	{
		public HqlIsNotNull(IASTFactory factory, HqlExpression lhs) : base(HqlSqlWalker.IS_NOT_NULL, "is not null", factory, lhs)
		{
		}
	}

	public partial class HqlStar : HqlExpression
	{
		public HqlStar(IASTFactory factory) : base(HqlSqlWalker.ROW_STAR, "*", factory)
		{
		}
	}

	public partial class HqlIn : HqlBooleanExpression
	{
		public HqlIn(IASTFactory factory, HqlExpression itemExpression, HqlTreeNode source)
			: base(HqlSqlWalker.IN, "in", factory, itemExpression)
		{
			AddChild(new HqlInList(factory, source));
		}
	}

	public partial class HqlInList : HqlTreeNode
	{
		public HqlInList(IASTFactory factory, HqlTreeNode source)
			: base(HqlSqlWalker.IN_LIST, "inlist", factory, source)
		{
		}
	}
}