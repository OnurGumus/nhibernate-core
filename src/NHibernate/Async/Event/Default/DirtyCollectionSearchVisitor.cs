#if NET_4_5
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event.Default
{
	/// <summary>
	/// A Visitor that determines if a dirty collection was found.
	/// </summary>
	/// <remarks>
	/// <list type = "number">
	///		<listheader>
	///			<description>Reason for dirty collection</description>
	///		</listheader>
	///		<item>
	///			<description>
	///			If it is a new application-instantiated collection, return true (does not occur anymore!)
	///			</description>
	///		</item>
	///		<item>
	///			<description>
	///			If it is a component, recurse.
	///			</description>
	///		</item>
	///		<item>
	///			<description>
	///			If it is a wrapped collection, ask the collection entry.
	///			</description>
	///		</item>
	/// </list>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DirtyCollectionSearchVisitor : AbstractVisitor
	{
		internal override Task<object> ProcessCollectionAsync(object collection, CollectionType type)
		{
			try
			{
				return Task.FromResult<object>(ProcessCollection(collection, type));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
