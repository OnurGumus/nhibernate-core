﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;

namespace NHibernate.Test.IdTest
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class HiLoTableGeneratorInt64FixtureAsync : IdFixtureBase
	{
		protected override string TypeName
		{
			get { return "HiLoInt64"; }
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			Int64 id;
			ISession s = OpenSession();
			HiLoInt64Class b = new HiLoInt64Class();
			await (s.SaveAsync(b, CancellationToken.None));
			await (s.FlushAsync(CancellationToken.None));
			id = b.Id;
			s.Close();

			s = OpenSession();
			b = (HiLoInt64Class) await (s.LoadAsync(typeof(HiLoInt64Class), b.Id, CancellationToken.None));
			Assert.AreEqual(id, b.Id);
			await (s.DeleteAsync(b, CancellationToken.None));
			await (s.FlushAsync(CancellationToken.None));
			s.Close();
		}
	}
}