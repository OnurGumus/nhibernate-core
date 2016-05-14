#if NET_4_5
using System.Collections;
using System.Data.Common;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Type
{
	/// <include file = 'IType.cs.xmldoc'
	///path = '//members[@type="IType"]/member[@name="T:IType"]/*'
	////>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IType : ICacheAssembler
	{
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.IsDirty"]/*'
		////> 
		Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session);
		Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session);
		Task<bool> IsModifiedAsync(object oldHydratedState, object currentState, bool[] checkable, ISessionImplementor session);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(DbDataReader, String[], ISessionImplementor, Object)"]/*'
		////> 
		Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(DbDataReader, String, ISessionImplementor, Object)"]/*'
		////> 
		Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeSet(settable)"]/*'
		////> 
		Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeSet"]/*'
		////> 
		Task NullSafeSetAsync(DbCommand st, object value, int index, ISessionImplementor session);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.ToString"]/*'
		////> 
		Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.DeepCopy"]/*'
		////> 
		Task<object> DeepCopyAsync(object val, EntityMode entityMode, ISessionFactoryImplementor factory);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.Hydrate"]/*'
		////> 
		Task<object> HydrateAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.ResolveIdentifier"]/*'
		////> 
		Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner);
		/// <summary>
		/// Given a hydrated, but unresolved value, return a value that may be used to
		/// reconstruct property-ref associations.
		/// </summary>
		Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.Copy"]/*'
		////> 
		Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copiedAlready);
		/// <summary> 
		/// During merge, replace the existing (target) value in the entity we are merging to
		/// with a new (original) value from the detached entity we are merging. For immutable
		/// objects, or null values, it is safe to simply return the first parameter. For
		/// mutable objects, it is safe to return a copy of the first parameter. For objects
		/// with component values, it might make sense to recursively replace component values. 
		/// </summary>
		/// <param name = "original">the value from the detached entity being merged </param>
		/// <param name = "target">the value in the managed entity </param>
		/// <param name = "session"></param>
		/// <param name = "owner"></param>
		/// <param name = "copyCache"></param>
		/// <param name = "foreignKeyDirection"></param>
		/// <returns> the value to be merged </returns>
		Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection);
		/// <summary> 
		/// Compare two instances of the class mapped by this type for persistence
		/// "equality" - equality of persistent state - taking a shortcut for
		/// entity references.
		/// </summary>
		/// <param name = "x"> </param>
		/// <param name = "y"> </param>
		/// <param name = "entityMode"> </param>
		/// <returns> boolean </returns>
		Task<bool> IsSameAsync(object x, object y, EntityMode entityMode);
		/// <summary> 
		/// Compare two instances of the class mapped by this type for persistence
		/// "equality" - equality of persistent state.
		/// </summary>
		/// <param name = "x"> </param>
		/// <param name = "y"> </param>
		/// <param name = "entityMode"> </param>
		/// <returns> boolean </returns>
		Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode);
		/// <summary> 
		/// Compare two instances of the class mapped by this type for persistence
		/// "equality" - equality of persistent state.
		/// </summary>
		/// <param name = "x"> </param>
		/// <param name = "y"> </param>
		/// <param name = "entityMode"> </param>
		/// <param name = "factory"></param>
		/// <returns> boolean </returns>
		Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory);
		/// <summary> Get a hashcode, consistent with persistence "equality"</summary>
		/// <param name = "x"> </param>
		/// <param name = "entityMode"> </param>
		Task<int> GetHashCodeAsync(object x, EntityMode entityMode);
		/// <summary> Get a hashcode, consistent with persistence "equality"</summary>
		/// <param name = "x"> </param>
		/// <param name = "entityMode"> </param>
		/// <param name = "factory"> </param>
		Task<int> GetHashCodeAsync(object x, EntityMode entityMode, ISessionFactoryImplementor factory);
		/// <summary> compare two instances of the type</summary>
		/// <param name = "x"> </param>
		/// <param name = "y"> </param>
		/// <param name = "entityMode"> </param>
		Task<int> CompareAsync(object x, object y, EntityMode? entityMode);
		/// <summary> 
		/// Given an instance of the type, return an array of boolean, indicating
		/// which mapped columns would be null. 
		/// </summary>
		/// <param name = "value">an instance of the type </param>
		/// <param name = "mapping"></param>
		Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping);
	}
}
#endif
