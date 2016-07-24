#if NET_4_5
using System;
using System.Reflection;
using NHibernate.DomainModel;
using NHibernate.Util;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReflectHelperFixtureAsync
	{
		[Test]
		public void GetConstantValueEnum()
		{
			object result = ReflectHelper.GetConstantValue(typeof (FooStatus), "ON");
			Assert.AreEqual(1, (int)result, "Should have found value of 1");
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public interface IMyBaseWithEqual
		{
			bool Equals(object that);
			int GetHashCode();
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public interface IMyInheritedWithEqual : IMyBaseWithEqual
		{
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public interface IEmpty
		{
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public interface IComplex : IEmpty, IMyInheritedWithEqual
		{
		}

		[Test]
		public void OverridesEquals()
		{
			Assert.IsFalse(ReflectHelper.OverridesEquals(this.GetType()), "ReflectHelperFixture does not override equals");
			Assert.IsTrue(ReflectHelper.OverridesEquals(typeof (string)), "String does override equals");
			Assert.IsFalse(ReflectHelper.OverridesEquals(typeof (IDisposable)), "IDisposable does not override equals");
			Assert.IsTrue(ReflectHelper.OverridesEquals(typeof (BRhf)), "Base class overrides equals");
			Assert.That(!ReflectHelper.OverridesEquals(typeof (object)), "System.Object does not override.");
		}

		[Test]
		public void InheritedInterfaceOverridesEquals()
		{
			Assert.That(ReflectHelper.OverridesEquals(typeof (IMyBaseWithEqual)), "does override.");
			Assert.That(ReflectHelper.OverridesEquals(typeof (IMyInheritedWithEqual)), "does override.");
			Assert.That(ReflectHelper.OverridesEquals(typeof (IComplex)), "does override.");
		}

		[Test]
		public void OverridesGetHashCode()
		{
			Assert.IsFalse(ReflectHelper.OverridesGetHashCode(this.GetType()), "ReflectHelperFixture does not override GetHashCode");
			Assert.IsTrue(ReflectHelper.OverridesGetHashCode(typeof (string)), "String does override equals");
			Assert.IsFalse(ReflectHelper.OverridesGetHashCode(typeof (IDisposable)), "IDisposable does not override GetHashCode");
			Assert.IsTrue(ReflectHelper.OverridesGetHashCode(typeof (BRhf)), "Base class overrides GetHashCode");
			Assert.That(!ReflectHelper.OverridesGetHashCode(typeof (object)), "System.Object does not override.");
		}

		[Test]
		public void InheritedInterfaceOverridesGetHashCode()
		{
			Assert.That(ReflectHelper.OverridesGetHashCode(typeof (IMyBaseWithEqual)), "does override.");
			Assert.That(ReflectHelper.OverridesGetHashCode(typeof (IMyInheritedWithEqual)), "does override.");
			Assert.That(ReflectHelper.OverridesGetHashCode(typeof (IComplex)), "does override.");
		}

		[Test]
		public void NoTypeFoundReturnsNull()
		{
			System.Type noType = ReflectHelper.TypeFromAssembly("noclass", "noassembly", false);
			Assert.IsNull(noType);
		}

		[Test]
		public void TypeFoundInNotLoadedAssembly()
		{
			System.Type httpRequest = ReflectHelper.TypeFromAssembly("System.Web.HttpRequest", "System.Web", false);
			Assert.IsNotNull(httpRequest);
			System.Type sameType = ReflectHelper.TypeFromAssembly("System.Web.HttpRequest", "System.Web", false);
			Assert.AreEqual(httpRequest, sameType, "should be the exact same Type");
		}

		[Test]
		public void SystemTypes()
		{
			System.Type int32 = ReflectHelper.ClassForName("System.Int32");
			Assert.AreEqual(typeof (Int32), int32);
		}

		[Test]
		public void TryGetMethod()
		{
			//const BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
			const BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			MethodInfo mig = typeof (MyBaseImplementation).GetMethod("get_Id", bf);
			MethodInfo mis = typeof (MyBaseImplementation).GetMethod("set_Id", bf);
			MethodInfo mng = typeof (MyBaseImplementation).GetMethod("get_Name", bf);
			MethodInfo mns = typeof (MyBaseImplementation).GetMethod("set_Name", bf);
			Assert.That(ReflectHelper.TryGetMethod(typeof (IMyBaseInterface), mig), Is.Not.Null);
			Assert.That(ReflectHelper.TryGetMethod(typeof (IMyBaseInterface), mis), Is.Null);
			Assert.That(ReflectHelper.TryGetMethod(typeof (IMyBaseInterface), mng), Is.Not.Null);
			Assert.That(ReflectHelper.TryGetMethod(typeof (IMyBaseInterface), mns), Is.Not.Null);
			mig = typeof (IMyBaseInterface).GetMethod("get_Id", bf);
			Assert.That(ReflectHelper.TryGetMethod(typeof (IMyInterface), mig), Is.Not.Null);
		}

		[Test]
		public void GetGenericMethodFrom()
		{
			var signature = new[]{typeof (string), typeof (string), typeof (bool)};
			Assert.That(ReflectHelper.GetGenericMethodFrom<ISomething>("List", new[]{typeof (BRhf)}, signature), Is.Not.Null);
			Assert.That(ReflectHelper.GetGenericMethodFrom<ISomething>("List", new[]{typeof (int), typeof (string)}, signature), Is.Not.Null);
			Assert.That(ReflectHelper.GetGenericMethodFrom<ISomething>("List", new[]{typeof (int), typeof (string)}, new[]{typeof (string), typeof (string), typeof (bool), typeof (IComparer<>).MakeGenericType(typeof (int))}), Is.Not.Null);
		}
	}
}
#endif
