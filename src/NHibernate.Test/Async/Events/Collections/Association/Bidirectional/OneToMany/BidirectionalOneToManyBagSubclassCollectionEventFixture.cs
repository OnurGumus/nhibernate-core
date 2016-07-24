#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Events.Collections.Association.Bidirectional.OneToMany
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BidirectionalOneToManyBagSubclassCollectionEventFixtureAsync : BidirectionalOneToManyBagCollectionEventFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"Events.Collections.Association.Bidirectional.OneToMany.BidirectionalOneToManyBagSubclassMapping.hbm.xml"};
			}
		}

		public override IParentWithCollection CreateParent(string name)
		{
			return new ParentWithBidirectionalOneToManySubclass(name);
		}
	}
}
#endif
