﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NUnit.Framework;

namespace NHibernate.Test.ListIndex
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class ListIndexFixtureAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] { "ListIndex.ListIndex.hbm.xml" }; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from B");
				s.Flush();
				s.Delete("from A");
				s.Flush();
			}
		}

		[Test]
		public async Task ListIndexBaseIsUsedAsync()
		{
			const int TheId = 2000;

			A a = new A();
			a.Name = "First";
			a.Id = TheId;

			B b = new B();
			b.AId = TheId;
			b.Name = "First B";
			a.Items.Add(b);

			B b2 = new B();
			b2.AId = TheId;
			b2.Name = "Second B";
			a.Items.Add(b2);

			ISession s = OpenSession();
			await (s.SaveAsync(a, CancellationToken.None));
			await (s.FlushAsync(CancellationToken.None));
			s.Close();

			s = OpenSession();
			A newA = await (s.GetAsync<A>(TheId, CancellationToken.None));

			Assert.AreEqual(2, newA.Items.Count);
			int counter = 1;
			foreach (B item in newA.Items)
			{
				Assert.AreEqual(counter, item.ListIndex);
				counter++;
			}
			s.Close();
		}
	}
}