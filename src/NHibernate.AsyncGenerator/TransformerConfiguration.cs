using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AsyncGenerator
{
	public class TransformerConfiguration
	{
		public AsyncConfiguration Async { get; set; } = new AsyncConfiguration();

		public List<Func<DocumentTransformer, ITransformerPlugin>> PluginFactories { get; set; } = new List<Func<DocumentTransformer, ITransformerPlugin>>();

		public string Indentation { get; set; } = "	";

		public string Directive { get; set; }
	}
}
