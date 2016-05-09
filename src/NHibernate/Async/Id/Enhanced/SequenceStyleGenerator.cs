using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Id.Enhanced
{
	/// <summary>
	/// Generates identifier values based on an sequence-style database structure.
	/// Variations range from actually using a sequence to using a table to mimic
	/// a sequence. These variations are encapsulated by the <see cref = "IDatabaseStructure"/>
	/// interface internally.
	/// </summary>
	/// <remarks>
	/// General configuration parameters:
	/// <table>
	///   <tr>
	///     <td><b>NAME</b></td>
	///     <td><b>DEFAULT</b></td>
	///     <td><b>DESCRIPTION</b></td>
	///   </tr>
	///   <tr>
	///     <td><see cref = "SequenceParam"/></td>
	///     <td><see cref = "DefaultSequenceName"/></td>
	///     <td>The name of the sequence/table to use to store/retrieve values</td>
	///   </tr>
	///   <tr>
	///     <td><see cref = "InitialParam"/></td>
	///     <td><see cref = "DefaultInitialValue"/></td>
	///     <td>The initial value to be stored for the given segment; the effect in terms of storage varies based on <see cref = "Optimizer"/> and <see cref = "DatabaseStructure"/></td>
	///   </tr>
	///   <tr>
	///     <td><see cref = "IncrementParam"/></td>
	///     <td><see cref = "DefaultIncrementSize"/></td>
	///     <td>The increment size for the underlying segment; the effect in terms of storage varies based on <see cref = "Optimizer"/> and <see cref = "DatabaseStructure"/></td>
	///   </tr>
	///   <tr>
	///     <td><see cref = "OptimizerParam"/></td>
	///     <td><i>depends on defined increment size</i></td>
	///     <td>Allows explicit definition of which optimization strategy to use</td>
	///   </tr>
	///   <tr>
	///     <td><see cref = "ForceTableParam"/></td>
	///     <td><b><i>false</i></b></td>
	///     <td>Allows explicit definition of which optimization strategy to use</td>
	///   </tr>
	/// </table>
	/// <p/>
	/// Configuration parameters used specifically when the underlying structure is a table:
	/// <table>
	///   <tr>
	///     <td><b>NAME</b></td>
	///     <td><b>DEFAULT</b></td>
	///     <td><b>DESCRIPTION</b></td>
	///   </tr>
	///   <tr>
	///     <td><see cref = "ValueColumnParam"/></td>
	///     <td><see cref = "DefaultValueColumnName"/></td>
	///     <td>The name of column which holds the sequence value for the given segment</td>
	///   </tr>
	/// </table>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SequenceStyleGenerator : IPersistentIdentifierGenerator, IConfigurable
	{
		public virtual async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			return await (Optimizer.GenerateAsync(DatabaseStructure.BuildCallback(session)));
		}
	}
}