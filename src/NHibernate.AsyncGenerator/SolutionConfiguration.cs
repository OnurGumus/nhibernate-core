using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace NHibernate.AsyncGenerator
{
	public class SolutionConfiguration
	{
		public SolutionConfiguration(string path)
		{
			Path = path;
		}

		/// <summary>
		/// Path where the solution is located
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// Configurations for projects inside the solution. If there is no configuration for a project a default one will be used
		/// </summary>
		public List<ProjectConfiguration> ProjectConfigurations { get; } = new List<ProjectConfiguration>();

		/// <summary>
		/// List of project names that will be excluded when analyzing the solution
		/// </summary>
		public HashSet<string> IgnoreProjectNames { get; set; } = new HashSet<string>();

		/// <summary>
		/// Function that will be executed when validating a method symbol. The return value indicates if the symbol is valid or not
		/// </summary>
		public Func<ProjectInfo, IMethodSymbol, bool> IsSymbolValidFunc { get; set; }

		/// <summary>
		/// Names of the attributes used for unit testing. In a unit test we cannot return a Task
		/// </summary>
		public HashSet<string> TestAttributeNames { get; set; } = new HashSet<string>();
	}
}
