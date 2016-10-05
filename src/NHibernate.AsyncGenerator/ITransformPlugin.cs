using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NHibernate.AsyncGenerator
{
	public interface ITransformerPlugin
	{
		CompilationUnitSyntax BeforeNormalization(CompilationUnitSyntax syntax);

		CompilationUnitSyntax AfterNormalization(CompilationUnitSyntax syntax);
	}
}
