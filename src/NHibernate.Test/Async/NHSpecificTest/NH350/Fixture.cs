#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH350
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH350";
			}
		}

		// We pass an Int32 id to Load and expect an exception, since the class
		// uses Int64 ids.
		[Test]
		public async Task LoadingAsync()
		{
			object parentId;
			using (ISession session = OpenSession())
			{
				SecurityDomain parent = new SecurityDomain();
				parent.Name = "Name";
				parent.ChildDomains.Add(new SecurityDomain());
				parentId = await (session.SaveAsync(parent));
				await (session.FlushAsync());
			}

			try
			{
				using (ISession session = OpenSession())
				{
					Assert.ThrowsAsync<TypeMismatchException>(async () => await (session.LoadAsync(typeof (SecurityDomain), 1)));
				}
			}
			finally
			{
				using (ISession session = OpenSession())
				{
					await (session.DeleteAsync("from SecurityDomain"));
					await (session.FlushAsync());
				}
			}
		}
	}
}
#endif
