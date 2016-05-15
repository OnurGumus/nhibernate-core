#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.GeneratedTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractGeneratedPropertyTest : TestCase
	{
		[Test]
		public async Task GeneratedPropertyAsync()
		{
			GeneratedPropertyEntity entity = new GeneratedPropertyEntity();
			entity.Name = "entity-1";
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(entity));
			await (s.FlushAsync());
			Assert.IsNotNull(entity.LastModified, "no timestamp retrieved");
			await (t.CommitAsync());
			s.Close();
			byte[] bytes = entity.LastModified;
			s = OpenSession();
			t = s.BeginTransaction();
			entity = (GeneratedPropertyEntity)await (s.GetAsync(typeof (GeneratedPropertyEntity), entity.Id));
			Assert.IsTrue(NHibernateUtil.Binary.IsEqual(bytes, entity.LastModified));
			await (t.CommitAsync());
			s.Close();
			Assert.IsTrue(NHibernateUtil.Binary.IsEqual(bytes, entity.LastModified));
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(entity));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
