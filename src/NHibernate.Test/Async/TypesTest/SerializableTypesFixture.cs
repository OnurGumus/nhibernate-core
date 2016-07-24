#if NET_4_5
using System;
using System.Reflection;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SerializableTypesFixtureAsync
	{
		[Test]
		public void AllEmbeddedTypesAreMarkedSerializable()
		{
			NHAssert.InheritedAreMarkedSerializable(typeof (IType));
		}

		[Test]
		public void EachEmbeddedBasicTypeIsSerializable()
		{
			FieldInfo[] builtInCustomTypes = typeof (NHibernateUtil).GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (FieldInfo fieldInfo in builtInCustomTypes)
			{
				IType ntp = (IType)fieldInfo.GetValue(null);
				NHAssert.IsSerializable(ntp, fieldInfo.Name + " is not serializable");
			}
		}
	}
}
#endif
