#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Events.Collections.Association.Bidirectional.ManyToMany
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BidirectionalManyToManyBagToSetCollectionEventFixtureAsync : AbstractAssociationCollectionEventFixtureAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"Events.Collections.Association.Bidirectional.ManyToMany.BidirectionalManyToManyBagToSetMapping.hbm.xml"};
			}
		}

		public override IParentWithCollection CreateParent(string name)
		{
			return new ParentWithBidirectionalManyToMany(name);
		}

		public override ICollection<IChild> CreateCollection()
		{
			return new List<IChild>();
		}

		public override Task UpdateParentOneToTwoSameChildrenAsync()
		{
			try
			{
				Assert.Ignore("Not supported");
				return TaskHelper.CompletedTask;
			// This test need some more deep study if it really work in H3.2
			// because <bag> allow duplication.
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
