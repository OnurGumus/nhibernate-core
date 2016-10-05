using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NHibernate.AsyncGenerator
{
	public class DocumentTransformationResult
	{
		public CompilationUnitSyntax TransformedNode { get; set; }

		public CompilationUnitSyntax OriginalRootNode { get; set; }
	}
}
