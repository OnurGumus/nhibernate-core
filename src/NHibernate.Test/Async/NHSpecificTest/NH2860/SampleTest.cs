#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2860
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		private int objId;
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var obj = new ClassA();
					obj.Text = "Test existing object";
					obj.Blob_Field = new byte[]{0, 0, 1, 1, 2, 2};
					objId = (int)await (session.SaveAsync(obj));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from ClassA"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task Can_LazyPropertyCastingExceptionAsync()
		{
			//following causes exception "Unable to cast object of type 'System.Object' to type 'System.Byte[]'.
			//notice that ClassA.Blob_Field s declared as lazy property
			//similar to fixed NH-2510 (?)
			//PS. Exception is beeing thrown only if object was created within the same session
			using (var session = OpenSession())
			{
				var classA = new ClassA();
				classA.Text = "new entity";
				classA.Blob_Field = new byte[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
				using (var trans = session.BeginTransaction())
				{
					await (session.SaveAsync(classA));
					await (trans.CommitAsync());
				}

				await (session.RefreshAsync(classA));
				classA.Text = "updated entity";
				using (var trans = session.BeginTransaction())
				{
					await (session.SaveOrUpdateAsync(classA));
					await (trans.CommitAsync());
				}

				await (session.RefreshAsync(classA));
			}
		}

		[Test]
		public async Task Can_LazyPropertyNotCastingExceptionAsync()
		{
			//here none exception beeing thrown
			using (var session = OpenSession())
			{
				var classA = await (session.GetAsync<ClassA>(objId));
				Assert.IsNotNull(classA);
				await (session.RefreshAsync(classA));
				classA.Text = "updated entity";
				using (var trans = session.BeginTransaction())
				{
					await (session.SaveOrUpdateAsync(classA));
					await (trans.CommitAsync());
				}

				await (session.RefreshAsync(classA));
			}
		}
	}
}
#endif
