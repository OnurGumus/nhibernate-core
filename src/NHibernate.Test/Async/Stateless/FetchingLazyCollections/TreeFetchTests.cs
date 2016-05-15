#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Stateless.FetchingLazyCollections
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TreeFetchTests : TestCaseMappingByCode
	{
		[Test]
		public async Task FetchMultipleHierarchiesAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var root = new TreeNode{Content = "Root"};
					var child1 = new TreeNode{Content = "Child1"};
					root.Children.Add(child1);
					root.Children.Add(new TreeNode{Content = "Child2"});
					child1.Children.Add(new TreeNode{Content = "Child1Child1"});
					child1.Children.Add(new TreeNode{Content = "Child1Child2"});
					await (s.SaveAsync(root));
					await (tx.CommitAsync());
				}

			using (IStatelessSession s = sessions.OpenStatelessSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<TreeNode> rootNodes = s.Query<TreeNode>().Where(t => t.Content == "Root").FetchMany(f => f.Children).ThenFetchMany(f => f.Children).ToList();
					Assert.That(rootNodes.Count, Is.EqualTo(1));
					Assert.That(rootNodes.First().Children.Count, Is.EqualTo(2));
					await (tx.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from TreeNode"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
