#if NET_4_5
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IdentityMapFixtureAsync
	{
		protected MutableHashCode item1 = null;
		protected MutableHashCode item2 = null;
		protected IDictionary expectedMap = null;
		protected NoHashCode noHashCode1 = null;
		protected NoHashCode noHashCode2 = null;
		protected object value1 = null;
		protected object value2 = null;
		[SetUp]
		public void SetUp()
		{
			item1 = new MutableHashCode(1);
			item2 = new MutableHashCode(2);
			value1 = new object ();
			value2 = new object ();
			noHashCode1 = new NoHashCode();
			noHashCode2 = new NoHashCode();
			expectedMap = new Hashtable();
			expectedMap.Add(item1, value1);
			expectedMap.Add(item2, value2);
		}

		protected virtual IDictionary GetIdentityMap()
		{
			return IdentityMap.Instantiate(10);
		}

		/// <summary>
		/// Whenever I run the test in the NUnit Gui two times it throws an error because 
		/// it can't find the method System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(object).
		/// I have isolated it to not be a problem with IdentityMap.IdentityKey and need to figure out if
		/// I have misconfigured NUnit on my machine or if NUnit is falling back to the .NET 1.0 Framework.
		/// The only reason I think it might be falling back is because that method was added in the .NET 1.1 
		/// Framework.
		/// </summary>
		/// <remarks>
		/// This is actually a problem with NUnit settings.  To resolve this go to Tools-Options and make
		/// sure that Reload before each test run is NOT checked.
		/// </remarks>
		 //[Test]
		public void MethodMissingException()
		{
			RuntimeHelpers.GetHashCode(new object ());
		}
	}
}
#endif
