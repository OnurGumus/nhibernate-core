#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1867
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AddMappingTestAsync
	{
		private const string mappingTemplate = @"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2' auto-import='true'>
  <class name='{0},NHibernate.Test'>
    <id name='Id' column='Id' type='string'>
      <generator class='assigned' />
    </id>
  </class>
  <class name='{1},NHibernate.Test'>
    <id name='OwnerId' column='Id' type='string'>
      <generator class='assigned' />
    </id>
  </class>
</hibernate-mapping>";
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class A
		{
			private string Id
			{
				get;
				set;
			}

			[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
			public class B
			{
				private string OwnerId
				{
					get;
					set;
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class A<T>
		{
			private string Id
			{
				get;
				set;
			}

			[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
			public class B
			{
				private string OwnerId
				{
					get;
					set;
				}
			}
		}

		[Test]
		public void NestedWithinNonGeneric()
		{
			var configuration = new Configuration().Configure();
			configuration.AddXml(string.Format(mappingTemplate, typeof (A).FullName, typeof (A.B).FullName));
		}

		[Test]
		public void NestedWithinGeneric()
		{
			var configuration = new Configuration().Configure();
			configuration.AddXml(string.Format(mappingTemplate, typeof (A<int>).FullName, typeof (A<int>.B).FullName));
		}
	}
}
#endif
