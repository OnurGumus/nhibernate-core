﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NHibernate.Type;
using NUnit.Framework;

namespace NHibernate.Test.TypesTest
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// The Unit Tests for the DecimalType
	/// </summary>
	[TestFixture]
	public class DecimalTypeFixtureAsync : TypeFixtureBase
	{
		protected override string TypeName
		{
			get { return "Decimal"; }
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			decimal expected = 5.64351M;

			DecimalClass basic = new DecimalClass();
			basic.Id = 1;
			basic.DecimalValue = expected;

			ISession s = OpenSession();
			await (s.SaveAsync(basic, CancellationToken.None));
			await (s.FlushAsync(CancellationToken.None));
			s.Close();

			s = OpenSession();
			basic = (DecimalClass) await (s.LoadAsync(typeof(DecimalClass), 1, CancellationToken.None));

			Assert.AreEqual(expected, basic.DecimalValue);
			Assert.AreEqual(5.643510M, basic.DecimalValue);

			await (s.DeleteAsync(basic, CancellationToken.None));
			await (s.FlushAsync(CancellationToken.None));
			s.Close();
		}
	}
}