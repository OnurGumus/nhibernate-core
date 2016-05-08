using System.Collections;
using System.Data;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Collection
{
	/// <summary>
	/// <para>
	/// Persistent collections are treated as value objects by NHibernate.
	/// ie. they have no independent existence beyond the object holding
	/// a reference to them. Unlike instances of entity classes, they are
	/// automatically deleted when unreferenced and automatically become
	/// persistent when held by a persistent object. Collections can be
	/// passed between different objects (change "roles") and this might
	/// cause their elements to move from one database table to another.
	/// </para>
	/// <para>
	/// NHibernate "wraps" a collection in an instance of
	/// <see cref = "IPersistentCollection"/>. This mechanism is designed
	/// to support tracking of changes to the collection's persistent
	/// state and lazy instantiation of collection elements. The downside
	/// is that only certain abstract collection types are supported and
	/// any extra semantics are lost.
	/// </para>
	/// <para>
	/// Applications should <b>never</b> use classes in this namespace
	/// directly, unless extending the "framework" here.
	/// </para>
	/// <para>
	/// Changes to <b>structure</b> of the collection are recorded by the
	/// collection calling back to the session. Changes to mutable
	/// elements (ie. composite elements) are discovered by cloning their
	/// state when the collection is initialized and comparing at flush
	/// time.
	/// </para>
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPersistentCollection
	{
		Task PreInsertAsync(ICollectionPersister persister);
	}
}