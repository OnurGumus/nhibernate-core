using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Param
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicFilterParameterSpecification : IParameterSpecification
	{
		public async Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			await (BindAsync(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session));
		}

		public async Task BindAsync(IDbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			string backTrackId = GetIdsForBackTrack(session.Factory).First(); // just the first because IType suppose the oders in certain sequence
			// The same filterName-parameterName can appear more than once in the whole query
			object value = session.GetFilterParameterValue(filterParameterFullName);
			foreach (int position in multiSqlQueryParametersList.GetEffectiveParameterLocations(backTrackId))
			{
				await (ExpectedType.NullSafeSetAsync(command, value, position, session));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class CollectionOfValuesType : IType
		{
			public async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
			{
				throw new InvalidOperationException();
			}

			public async Task BeforeAssembleAsync(object cached, ISessionImplementor session)
			{
			}

			public async Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner)
			{
				throw new InvalidOperationException();
			}

			public async Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
			{
				throw new InvalidOperationException();
			}

			public async Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
			{
				throw new InvalidOperationException();
			}

			public async Task<object> HydrateAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
			{
				throw new InvalidOperationException();
			}

			public async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copiedAlready)
			{
				throw new InvalidOperationException();
			}

			public async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
			{
				throw new InvalidOperationException();
			}

			public async Task<bool> IsModifiedAsync(object oldHydratedState, object currentState, bool[] checkable, ISessionImplementor session)
			{
				return false;
			}

			public async Task<bool> IsSameAsync(object x, object y, EntityMode entityMode)
			{
				return false;
			}

			public async Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session)
			{
				return false;
			}

			public async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
			{
				return false;
			}

			public async Task<int> CompareAsync(object x, object y, EntityMode? entityMode)
			{
				return 1;
			}

			public async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
			{
				return false;
			}

			public async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory)
			{
				return false;
			}

			public async Task<object> DeepCopyAsync(object val, EntityMode entityMode, ISessionFactoryImplementor factory)
			{
				throw new InvalidOperationException();
			}

			public async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
			{
				throw new InvalidOperationException();
			}

			public async Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
			{
				throw new InvalidOperationException();
			}

			public async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
			{
				return GetHashCode();
			}

			public async Task<int> GetHashCodeAsync(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
			{
				return GetHashCode();
			}

			public async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
			{
				throw new InvalidOperationException();
			}

			public async Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
			{
				throw new InvalidOperationException();
			}

			public async Task NullSafeSetAsync(IDbCommand st, object value, int index, ISessionImplementor session)
			{
				var start = index;
				var positions = 0;
				var singleParameterColumnSpan = elementType.GetColumnSpan(session.Factory);
				var collection = (IEnumerable)value;
				foreach (var element in collection)
				{
					await (elementType.NullSafeSetAsync(st, element, start + positions, session));
					positions += singleParameterColumnSpan;
				}
			}

			public async Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner)
			{
				throw new InvalidOperationException();
			}
		}
	}
}