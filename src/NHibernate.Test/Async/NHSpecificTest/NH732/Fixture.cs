#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Dialect;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

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
				User user = (User)await (session.LoadAsync(typeof (User), "DOMAIN\\USER"));
				Role role = (Role)await (session.LoadAsync(typeof (Role), "Admins"));
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

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CaseInsensitiveStringType : IEnhancedUserType
	{
		public Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, object owner)
		{
			try
			{
				return Task.FromResult<object>(NullSafeGet(rs, names, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task NullSafeSetAsync(DbCommand cmd, object value, int index)
		{
			try
			{
				NullSafeSet(cmd, value, index);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
