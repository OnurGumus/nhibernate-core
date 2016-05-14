#if NET_4_5
using NHibernate;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Tuple
{
	/// <summary> 
	/// A tuplizer defines the contract for things which know how to manage
	/// a particular representation of a piece of data, given that
	/// representation's <see cref = "EntityMode"/> (the entity-mode
	/// essentially defining which representation).
	/// </summary>
	/// <remarks>
	/// If that given piece of data is thought of as a data structure, then a tuplizer
	/// is the thing which knows how to:
	/// <list type = "bullet">
	/// <item><description>create such a data structure appropriately</description></item>
	/// <item><description>extract values from and inject values into such a data structure</description></item>
	/// </list>
	/// <para/>
	/// For example, a given piece of data might be represented as a POCO class.
	/// Here, it's representation and entity-mode is POCO.  Well a tuplizer for POCO
	/// entity-modes would know how to:
	/// <list type = "bullet">
	/// <item><description>create the data structure by calling the POCO's constructor</description></item>
	/// <item><description>extract and inject values through getters/setter, or by direct field access, etc</description></item>
	/// </list>
	/// <para/>
	/// That same piece of data might also be represented as a DOM structure, using
	/// the tuplizer associated with the XML entity-mode, which would generate instances
	/// of <see cref = "System.Xml.XmlElement"/> as the data structure and know how to access the
	/// values as either nested <see cref = "System.Xml.XmlElement"/>s or as <see cref = "System.Xml.XmlAttribute"/>s.
	/// </remarks>
	/// <seealso cref = "Entity.IEntityTuplizer"/>
	/// <seealso cref = "Component.IComponentTuplizer"/>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ITuplizer
	{
		/// <summary> Generate a new, empty entity. </summary>
		/// <returns> The new, empty entity instance. </returns>
		Task<object> InstantiateAsync();
	}
}
#endif
