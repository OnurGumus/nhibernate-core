#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Dialect;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH732
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CaseInsensitiveIdAsync()
		{
			using (ISession session = OpenSession())
			{
				User user = new User();
				user.UserName = @"Domain\User";
				await (session.SaveAsync(user));
				Role role = new Role();
				role.RoleName = "ADMINS";
				await (session.SaveAsync(role));
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				User user = (User)session.Load(typeof (User), "DOMAIN\\USER");
				Role role = (Role)session.Load(typeof (Role), "Admins");
				UserToRole userToRole = new UserToRole();
				userToRole.User = user;
				userToRole.Role = role;
				await (session.SaveAsync(userToRole));
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				User user = await (session.GetAsync<User>("domain\\user"));
				Assert.AreEqual(1, user.UserToRoles.Count);
			}

			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from System.Object o"));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
