#if NET_4_5
using System;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AssemblyQualifiedTypeNameFixtureAsync
	{
		[Test]
		public void Construct()
		{
			string typeName = "MyType";
			string assemblyName = "MyAssembly";
			AssemblyQualifiedTypeName tn = new AssemblyQualifiedTypeName(typeName, assemblyName);
			Assert.AreEqual(typeName, tn.Type);
			Assert.AreEqual(assemblyName, tn.Assembly);
		}

		private AssemblyQualifiedTypeName CreateDefaultName()
		{
			return new AssemblyQualifiedTypeName("MyType", "MyAssembly");
		}

		[Test]
		public void Equals()
		{
			Assert.AreEqual(CreateDefaultName(), CreateDefaultName());
			Assert.IsFalse(new AssemblyQualifiedTypeName("T1", "A1").Equals(new AssemblyQualifiedTypeName("T2", "A2")));
			Assert.AreEqual(CreateDefaultName().GetHashCode(), CreateDefaultName().GetHashCode());
		}

		[Test]
		public Task ConstructWithNullTypeAsync()
		{
			try
			{
				Assert.Throws<ArgumentNullException>(() => new AssemblyQualifiedTypeName(null, "SomeAssembly"));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void ConstructWithNullAssembly()
		{
			new AssemblyQualifiedTypeName("SomeType", null);
		}

		[Test]
		public void ToStringSimple()
		{
			AssemblyQualifiedTypeName tn = new AssemblyQualifiedTypeName("MyType", null);
			Assert.AreEqual("MyType", tn.ToString());
		}

		[Test]
		public void ToStringComplex()
		{
			AssemblyQualifiedTypeName tn = new AssemblyQualifiedTypeName("MyType", "MyAssembly");
			Assert.AreEqual("MyType, MyAssembly", tn.ToString());
		}

		[Test]
		public void ToStringEscaped()
		{
			AssemblyQualifiedTypeName tn = new AssemblyQualifiedTypeName("Escaped\\,Type", "Escaped\\,Assembly");
			Assert.AreEqual(tn.Type + ", " + tn.Assembly, tn.ToString());
		}
	}
}
#endif
