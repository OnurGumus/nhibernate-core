using System;

namespace NHibernate.Test.NHSpecificTest.NH1082
{
	public partial class SessionInterceptorThatThrowsExceptionAtBeforeTransactionCompletion : EmptyInterceptor
	{
		public override void BeforeTransactionCompletion(ITransaction tx)
		{
			throw new BadException();
		}
	}

	public partial class BadException : Exception
	{ }
}