#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2324
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CompositeUserType : ICompositeUserType
	{
		/// <summary>
		/// Retrieve an instance of the mapped class from a DbDataReader. Implementors
		/// should handle possibility of null values.
		/// </summary>
		/// <param name = "dr">DbDataReader</param>
		/// <param name = "names">the column names</param>
		/// <param name = "session"></param>
		/// <param name = "owner">the containing entity</param>
		/// <returns></returns>
		public async Task<object> NullSafeGetAsync(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
		{
			var data = new CompositeData();
			data.DataA = (DateTime)await (NHibernateUtil.DateTime.NullSafeGetAsync(dr, new[]{names[0]}, session, owner));
			data.DataB = (DateTime)await (NHibernateUtil.DateTime.NullSafeGetAsync(dr, new[]{names[1]}, session, owner));
			return data;
		}

		/// <summary>
		/// Write an instance of the mapped class to a prepared statement.
		/// Implementors should handle possibility of null values.
		/// A multi-column type should be written to parameters starting from index.
		/// </summary>
		/// <param name = "cmd"></param>
		/// <param name = "value"></param>
		/// <param name = "index"></param>
		/// <param name = "settable"></param>
		/// <param name = "session"></param>
		public Task NullSafeSetAsync(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session)
		{
			try
			{
				NullSafeSet(cmd, value, index, settable, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <summary>
		/// Transform the object into its cacheable representation.
		/// At the very least this method should perform a deep copy.
		/// That may not be enough for some implementations, method should perform a deep copy. That may not be enough for some implementations, however; for example, associations must be cached as identifier values. (optional operation)
		/// </summary>
		/// <param name = "value">the object to be cached</param>
		/// <param name = "session"></param>
		/// <returns></returns>
		public Task<object> DisassembleAsync(object value, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Disassemble(value, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <summary>
		/// Reconstruct an object from the cacheable representation.
		/// At the very least this method should perform a deep copy. (optional operation)
		/// </summary>
		/// <param name = "cached">the object to be cached</param>
		/// <param name = "session"></param>
		/// <param name = "owner"></param>
		/// <returns></returns>
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

		/// <summary>
		/// During merge, replace the existing (target) value in the entity we are merging to
		/// with a new (original) value from the detached entity we are merging. For immutable
		/// objects, or null values, it is safe to simply return the first parameter. For
		/// mutable objects, it is safe to return a copy of the first parameter. However, since
		/// composite user types often define component values, it might make sense to recursively
		/// replace component values in the target object.
		/// </summary>
		/// <param name = "original"></param>
		/// <param name = "target"></param>
		/// <param name = "session"></param>
		/// <param name = "owner"></param>
		/// <returns></returns>
		public Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(Replace(original, target, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
