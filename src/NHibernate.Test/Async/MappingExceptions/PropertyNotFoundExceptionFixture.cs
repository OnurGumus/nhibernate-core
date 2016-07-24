#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingExceptions
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PropertyNotFoundExceptionFixtureAsync
	{
		[Test]
		public void MisspelledPropertyName()
		{
			bool excCaught = false;
			// add a resource that has a bad mapping
			string resource = "NHibernate.Test.MappingExceptions.A.PropertyNotFound.hbm.xml";
			Configuration cfg = new Configuration();
			try
			{
				cfg.AddResource(resource, GetType().Assembly);
				cfg.BuildSessionFactory();
			}
			catch (MappingException me)
			{
				PropertyNotFoundException found = null;
				Exception find = me;
				while (find != null)
				{
					found = find as PropertyNotFoundException;
					find = find.InnerException;
				}

				Assert.IsNotNull(found, "The PropertyNotFoundException is not present in the Exception tree.");
				Assert.AreEqual("Naame", found.PropertyName, "should contain name of missing property 'Naame' in exception");
				Assert.AreEqual(typeof (A), found.TargetType, "should contain name of class that is missing the property");
				excCaught = true;
			}

			Assert.IsTrue(excCaught, "Should have caught the MappingException that contains the property not found exception.");
		}

		[Test]
		public void ConstructWithNullType()
		{
			new PropertyNotFoundException(null, "someField");
			new PropertyNotFoundException(null, "SomeProperty", "getter");
		}
	}
}
#endif
