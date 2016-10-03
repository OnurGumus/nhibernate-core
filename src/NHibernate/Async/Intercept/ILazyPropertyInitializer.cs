﻿#if NET_4_5
using System;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Intercept
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILazyPropertyInitializer
	{
		/// <summary> Initialize the property, and return its new value</summary>
		Task<object> InitializeLazyPropertyAsync(string fieldName, object entity, ISessionImplementor session);
	}
}
#endif
