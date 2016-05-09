using System;
using System.Runtime.Serialization;
using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class CollectionAction : IExecutable, IComparable<CollectionAction>, IDeserializationCallback
	{
		/// <summary>Execute this action</summary>
		public abstract Task ExecuteAsync();
	}
}