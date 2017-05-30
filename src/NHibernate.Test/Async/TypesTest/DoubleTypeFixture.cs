﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;

namespace NHibernate.Test.TypesTest
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// Tests for mapping a Double Property to a database field.
	/// </summary>
	[TestFixture]
	public class DoubleTypeFixtureAsync : TypeFixtureBase
	{
		private double[] _values = new double[2];

		protected override string TypeName
		{
			get { return "Double"; }
		}

		protected override void OnSetUp()
		{
			base.OnSetUp();
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
			await (s.SaveAsync(basic, CancellationToken.None));
			await (s.FlushAsync(CancellationToken.None));
			s.Close();

			s = OpenSession();
			basic = (DoubleClass) await (s.LoadAsync(typeof(DoubleClass), 1, CancellationToken.None));

			Assert.AreEqual(_values[0], basic.DoubleValue);

			await (s.DeleteAsync(basic, CancellationToken.None));
			await (s.FlushAsync(CancellationToken.None));
			s.Close();
		}
	}
}