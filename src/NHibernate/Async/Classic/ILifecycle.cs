#if NET_4_5
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Classic
{
	/// <summary>
	/// Provides callbacks from the <see cref = "ISession"/> to the persistent object. Persistent classes may
	/// implement this interface but they are not required to.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref = "OnSave"/>, <see cref = "OnDelete"/>, and <see cref = "OnUpdate"/> are intended to be used
	/// to cascade saves and deletions of dependent objects. This is an alternative to declaring cascaded
	/// operations in the mapping file.
	/// </para>
	/// <para>
	/// <see cref = "OnLoad"/> may be used to initialize transient properties of the object from its persistent
	/// state. It may <em>not</em> be used to load dependent objects since the <see cref = "ISession"/> interface
	/// may not be invoked from inside this method.
	/// </para>
	/// <para>
	/// A further intended usage of <see cref = "OnLoad"/>, <see cref = "OnSave"/>, and <see cref = "OnUpdate"/>
	/// is to store a reference to the <see cref = "ISession"/> for later use.
	/// </para>
	/// <para>
	/// If <see cref = "OnSave"/>, <see cref = "OnUpdate"/>, or <see cref = "OnDelete"/> return
	/// <see cref = "LifecycleVeto.Veto"/>, the operation is silently vetoed. If a <see cref = "CallbackException"/>
	/// is thrown, the operation is vetoed and the exception is passed back to the application.
	/// </para>
	/// <para>
	/// Note that <see cref = "OnSave"/> is called after an identifier is assigned to the object, except when
	/// <c>identity</c> key generation is used.
	/// </para>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILifecycle
	{
		/// <summary>
		/// Called when an entity is saved
		/// </summary>
		/// <param name = "s">The session</param>
		/// <returns>If we should veto the save</returns>
		Task<LifecycleVeto> OnSaveAsync(ISession s);
		/// <summary>
		/// Called when an entity is deleted
		/// </summary>
		/// <param name = "s">The session</param>
		/// <returns>A <see cref = "LifecycleVeto"/> value indicating whether the operation
		/// should be vetoed or allowed to proceed.</returns>
		Task<LifecycleVeto> OnDeleteAsync(ISession s);
	}
}
#endif
