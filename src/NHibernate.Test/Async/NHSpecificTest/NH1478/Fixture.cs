#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1478
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task TestIfColonInStringIsNotInterpretedAsParameterInSQLAsync()
		{
			using (ISession session = OpenSession())
			{
				IList lst = await (session.CreateSQLQuery("select Biography from Person where Biography='Born in Istanbul :Turkey'").AddScalar("Biography", NHibernateUtil.String).ListAsync());
				Assert.AreEqual(1, lst.Count);
			}
		}

		[Test]
		public async Task TestIfColonInStringIsNotInterpretedAsParameterInHQLAsync()
		{
			using (ISession session = OpenSession())
			{
				IList lst = await (session.CreateSQLQuery("select p.Biography from Person p where p.Biography='Born in Istanbul :Turkey'").ListAsync());
				Assert.AreEqual(1, lst.Count);
			}
		}
	}
}
#endif
