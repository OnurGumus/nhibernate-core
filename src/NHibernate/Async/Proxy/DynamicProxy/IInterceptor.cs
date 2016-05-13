using System.Threading.Tasks;
using System;
using NHibernate.Util;

#region Credits
// This work is based on LinFu.DynamicProxy framework (c) Philip Laureano who has donated it to NHibernate project.
// The license is the same of NHibernate license (LGPL Version 2.1, February 1999).
// The source was then modified to be the default DynamicProxy of NHibernate project.
#endregion
namespace NHibernate.Proxy.DynamicProxy
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IInterceptor
	{
		Task<object> InterceptAsync(InvocationInfo info);
	}
}