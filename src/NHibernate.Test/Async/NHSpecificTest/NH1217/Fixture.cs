#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1217
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		/// <summary>
		/// +--------+          +--------+ 1   from   * +--------+
		///	|        | 1      * |        |--------------|        |
		///	|  Root  |----------|  Node  | 1    to    * |  Edge  |
		///	|        |          |        |--------------|        |
		///	+--------+          +--------+              +--------+ 
		/// </summary>
		[Test]
		public async Task NoExceptionMustBeThrownAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Root r = new Root();
					r.Name = "a root";
					INode n1 = r.AddNode("Node1");
					INode n2 = r.AddNode("Node2");
					IEdge e1 = new Edge();
					e1.Label = "Edge 1";
					e1.FromNode = n1;
					e1.ToNode = n2;
					n1.FromEdges.Add(e1);
					n2.ToEdges.Add(e1);
					await (s.SaveAsync(r));
					await (s.FlushAsync());
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
