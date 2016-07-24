#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BooleanTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "Boolean";
			}
		}

		/// <summary>
		/// Verify Equals will correctly determine when the property
		/// is dirty.
		/// </summary>
		[Test]
		public void Equals()
		{
			BooleanType type = (BooleanType)NHibernateUtil.Boolean;
			Assert.IsTrue(type.IsEqual(true, true));
			Assert.IsTrue(type.IsEqual(false, false));
			Assert.IsFalse(type.IsEqual(true, false));
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			BooleanClass basic = new BooleanClass();
			basic.Id = 1;
			basic.BooleanValue = true;
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = (BooleanClass)await (s.LoadAsync(typeof (BooleanClass), 1));
			Assert.AreEqual(true, basic.BooleanValue);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
