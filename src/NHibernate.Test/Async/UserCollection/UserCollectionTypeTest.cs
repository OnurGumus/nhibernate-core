#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UserCollection
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UserCollectionTypeTestAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"UserCollection.UserPermissions.hbm.xml"};
			}
		}

		[Test]
		public async Task BasicOperationAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			User u = new User("max");
			u.EmailAddresses.Add(new Email("max@hibernate.org"));
			u.EmailAddresses.Add(new Email("max.andersen@jboss.com"));
			await (s.SaveAsync(u));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			User u2 = (User)await (s.CreateCriteria(typeof (User)).UniqueResultAsync());
			Assert.IsTrue(NHibernateUtil.IsInitialized(u2.EmailAddresses));
			Assert.AreEqual(2, u2.EmailAddresses.Count);
			await (s.DeleteAsync(u2));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
