using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NHibernate.Test
{
	[SetUpFixture]
	public class DirectorySetupFixture
	{
		[OneTimeSetUp]
		public void SetUp()
		{
			// Setup current directory for backwards compatiblilty with nunit 2.6
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		[OneTimeTearDown]
		public void TearDown()
		{
		}
	}
}
