#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Events.Collections.Association.Unidirectional.OneToMany
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UnidirectionalOneToManyBagCollectionEventFixtureAsync : AbstractAssociationCollectionEventFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"Events.Collections.Association.Unidirectional.OneToMany.UnidirectionalOneToManyBagMapping.hbm.xml"};
			}
		}

		public override IParentWithCollection CreateParent(string name)
		{
			return new ParentWithCollectionOfEntities(name);
		}

		public override ICollection<IChild> CreateCollection()
		{
			return new List<IChild>();
		}
	}
}
#endif
