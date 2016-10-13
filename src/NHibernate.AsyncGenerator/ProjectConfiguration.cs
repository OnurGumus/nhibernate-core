using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NHibernate.AsyncGenerator
{
	public class ProjectConfiguration
	{
		public ProjectConfiguration(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Name of the project
		/// </summary>
		public string Name { get; }

		public Func<IMethodSymbol, MethodAsyncConversion> MethodConversionFunc { get; set; } = m => MethodAsyncConversion.None;

		/// <summary>
		/// Predicate for document selection
		/// </summary>
		public Func<Document, bool> CanScanDocumentFunc { get; set; } = m => true;

		/// <summary>
		/// Predicate for reference selection
		/// </summary>
		public Func<IMethodSymbol, bool> CanConvertReferenceFunc { get; set; } = m => true;

		/// <summary>
		/// Predicate for reference selection
		/// </summary>
		public Func<MethodInfo, bool> CanGenerateMethod { get; set; } = m => true;

		/// <summary>
		/// A custom method that will be called when for a method there is not an async counterpart with same parameters
		/// </summary>
		public Func<Project, IMethodSymbol, bool, Task<IMethodSymbol>> FindAsyncCounterpart { get; set; } = null;

		/// <summary>
		/// When enabled it will search all invocation expressions inside a method body and tries to find a async counterpart
		/// </summary>
		public bool ScanMethodsBody { get; set; }

		/// <summary>
		/// When enabled it will not analyze references outside project. For ignoring references from specific projects use property IgnoreReferencesFromProjects
		/// </summary>
		public bool IgnoreExternalReferences { get; set; }

		/// <summary>
		/// Ignores all references that are located in the provided project names
		/// </summary>
		public HashSet<string> IgnoreReferencesFromProjects { get; set; } = new HashSet<string>();

		/// <summary>
		/// When enabled it will scan each type in project for non implemented async members that have a sync counterpart
		/// </summary>
		public bool ScanForMissingAsyncMembers { get; set; }

		/// <summary>
		/// Define how types will be converted to async. Default all types will be generated as partial
		/// </summary>
		public Func<INamedTypeSymbol, TypeTransformation> TypeTransformationFunc { get; set; } = s => TypeTransformation.Partial;

		/// <summary>
		/// Name of the folder where all async partial classes will be stored
		/// </summary>
		public string AsyncFolder { get; set; } = "Async";

		/// <summary>
		/// Allow to add additional usings for a specific document
		/// </summary>
		public Func<CompilationUnitSyntax, IEnumerable<string>> GetAdditionalUsings { get; set; }

	}
}
