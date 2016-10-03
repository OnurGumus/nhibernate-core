#if NET_4_5
using System;
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DoubleTypeFixtureAsync : TypeFixtureBaseAsync
	{
		private double[] _values = new double[2];
		protected override string TypeName
		{
			get
			{
				return "Double";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			if (Dialect is Oracle8iDialect)
			{
				_values[0] = 1.5e20;
				_values[1] = 1.2e-20;
			}
			else
			{
				_values[0] = 1.5e35;
				_values[1] = 1.2e-35;
			}
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			DoubleClass basic = new DoubleClass();
			basic.Id = 1;
			basic.DoubleValue = _values[0];
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = (DoubleClass)await (s.LoadAsync(typeof (DoubleClass), 1));
			Assert.AreEqual(_values[0], basic.DoubleValue);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
