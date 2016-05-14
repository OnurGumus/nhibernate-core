#if NET_4_5
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
using NHibernate.Util;

namespace NHibernate.Param
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicFilterParameterSpecification : IParameterSpecification
	{
		public Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			return BindAsync(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
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
			public Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
			{
				try
				{
					return Task.FromResult<object>(Disassemble(value, session, owner));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
			{
				try
				{
					return Task.FromResult<object>(Assemble(cached, session, owner));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task BeforeAssembleAsync(object cached, ISessionImplementor session)
			{
				return TaskHelper.CompletedTask;
			}

			public Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session)
			{
				try
				{
					return Task.FromResult<bool>(IsDirty(old, current, session));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<bool>(ex);
				}
			}

			public Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
			{
				try
				{
					return Task.FromResult<bool>(IsDirty(old, current, checkable, session));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<bool>(ex);
				}
			}

			public Task<bool> IsModifiedAsync(object oldHydratedState, object currentState, bool[] checkable, ISessionImplementor session)
			{
				try
				{
					return Task.FromResult<bool>(IsModified(oldHydratedState, currentState, checkable, session));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<bool>(ex);
				}
			}

			public Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
			{
				try
				{
					return Task.FromResult<object>(NullSafeGet(rs, names, session, owner));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
			{
				try
				{
					return Task.FromResult<object>(NullSafeGet(rs, name, session, owner));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
			{
				try
				{
					NullSafeSet(st, value, index, settable, session);
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
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

			public Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
			{
				try
				{
					return Task.FromResult<string>(ToLoggableString(value, factory));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<string>(ex);
				}
			}

			public Task<object> DeepCopyAsync(object val, EntityMode entityMode, ISessionFactoryImplementor factory)
			{
				try
				{
					return Task.FromResult<object>(DeepCopy(val, entityMode, factory));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task<object> HydrateAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
			{
				try
				{
					return Task.FromResult<object>(Hydrate(rs, names, session, owner));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner)
			{
				try
				{
					return Task.FromResult<object>(ResolveIdentifier(value, session, owner));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner)
			{
				try
				{
					return Task.FromResult<object>(SemiResolve(value, session, owner));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copiedAlready)
			{
				try
				{
					return Task.FromResult<object>(Replace(original, target, session, owner, copiedAlready));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
			{
				try
				{
					return Task.FromResult<object>(Replace(original, target, session, owner, copyCache, foreignKeyDirection));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}

			public Task<bool> IsSameAsync(object x, object y, EntityMode entityMode)
			{
				try
				{
					return Task.FromResult<bool>(IsSame(x, y, entityMode));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<bool>(ex);
				}
			}

			public Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
			{
				try
				{
					return Task.FromResult<bool>(IsEqual(x, y, entityMode));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<bool>(ex);
				}
			}

			public Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory)
			{
				try
				{
					return Task.FromResult<bool>(IsEqual(x, y, entityMode, factory));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<bool>(ex);
				}
			}

			public Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
			{
				try
				{
					return Task.FromResult<int>(GetHashCode(x, entityMode));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<int>(ex);
				}
			}

			public Task<int> GetHashCodeAsync(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
			{
				try
				{
					return Task.FromResult<int>(GetHashCode(x, entityMode, factory));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<int>(ex);
				}
			}

			public Task<int> CompareAsync(object x, object y, EntityMode? entityMode)
			{
				try
				{
					return Task.FromResult<int>(Compare(x, y, entityMode));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<int>(ex);
				}
			}

			public Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
			{
				try
				{
					return Task.FromResult<bool[]>(ToColumnNullness(value, mapping));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<bool[]>(ex);
				}
			}
		}
	}
}
#endif
