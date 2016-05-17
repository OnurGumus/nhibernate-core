#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.GenericTest.Overall
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task CRUDAsync()
		{
			var entity = new A<int>{Property = 10, Collection = new List<int>{20}};
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(entity));
					await (transaction.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync(entity));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task CRUDABAsync()
		{
			var entity = new A<B>{Property = new B{Prop = 2}, Collection = new List<B>{new B{Prop = 3}}};
			Console.WriteLine(entity.GetType().FullName);
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(entity));
					await (transaction.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync(entity));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
