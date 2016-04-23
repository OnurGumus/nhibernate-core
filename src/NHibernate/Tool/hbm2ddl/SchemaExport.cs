using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using NHibernate.AdoNet.Util;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Util;
using Environment=NHibernate.Cfg.Environment;

namespace NHibernate.Tool.hbm2ddl
{
	/// <summary>
	/// Generates ddl to export table schema for a configured <c>Configuration</c> to the database
	/// </summary>
	/// <remarks>
	/// This Class can be used directly or the command line wrapper NHibernate.Tool.hbm2ddl.exe can be
	/// used when a dll can not be directly used.
	/// </remarks>
	public class SchemaExport
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SchemaExport));
		private bool wasInitialized;
		private readonly Configuration cfg;
		private readonly IDictionary<string, string> configProperties;
		private string[] createSQL;
		private Dialect.Dialect dialect;
		private string[] dropSQL;
		private IFormatter formatter;
		private string delimiter;
		private string outputFile;

		/// <summary>
		/// Create a schema exported for a given Configuration
		/// </summary>
		/// <param name="cfg">The NHibernate Configuration to generate the schema from.</param>
		public SchemaExport(Configuration cfg) : this(cfg, cfg.Properties) {}

		/// <summary>
		/// Create a schema exporter for the given Configuration, with the given
		/// database connection properties
		/// </summary>
		/// <param name="cfg">The NHibernate Configuration to generate the schema from.</param>
		/// <param name="configProperties">The Properties to use when connecting to the Database.</param>
		public SchemaExport(Configuration cfg, IDictionary<string, string> configProperties)
		{
			this.cfg = cfg;
			this.configProperties = configProperties;
		}

		private async Task Initialize()
		{
			if(wasInitialized)
			{
				return;
			}
			string autoKeyWordsImport = PropertiesHelper.GetString(Environment.Hbm2ddlKeyWords, configProperties, "not-defined");
			autoKeyWordsImport = autoKeyWordsImport.ToLowerInvariant();
			if (autoKeyWordsImport == Hbm2DDLKeyWords.AutoQuote)
			{
				await SchemaMetadataUpdater.QuoteTableAndColumnsAsync(cfg).ConfigureAwait(false);
			}

			dialect = Dialect.Dialect.GetDialect(configProperties);
			dropSQL = cfg.GenerateDropSchemaScript(dialect);
			createSQL = cfg.GenerateSchemaCreationScript(dialect);
			formatter = (PropertiesHelper.GetBoolean(Environment.FormatSql, configProperties, true) ? FormatStyle.Ddl : FormatStyle.None).Formatter;
			wasInitialized = true;
		}

		/// <summary>
		/// Set the output filename. The generated script will be written to this file
		/// </summary>
		/// <param name="filename">The name of the file to output the ddl to.</param>
		/// <returns>The SchemaExport object.</returns>
		public SchemaExport SetOutputFile(string filename)
		{
			outputFile = filename;
			return this;
		}

		/// <summary>
		/// Set the end of statement delimiter 
		/// </summary>
		/// <param name="delimiter">The end of statement delimiter.</param>
		/// <returns>The SchemaExport object.</returns>
		public SchemaExport SetDelimiter(string delimiter)
		{
			this.delimiter = delimiter;
			return this;
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name="useStdOut"><see langword="true" /> if the ddl should be outputted in the Console.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public void Create(bool useStdOut, bool execute)
		{
			Execute(useStdOut, execute, false);
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name="useStdOut"><see langword="true" /> if the ddl should be outputted in the Console.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public Task CreateAsync(bool useStdOut, bool execute)
		{
			return ExecuteAsync(useStdOut, execute, false);
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name="scriptAction"> an action that will be called for each line of the generated ddl.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public void Create(Action<string> scriptAction, bool execute)
		{
			Execute(scriptAction, execute, false);
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name="scriptAction"> an action that will be called for each line of the generated ddl.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public Task CreateAsync(Action<string> scriptAction, bool execute)
		{
			return ExecuteAsync(scriptAction, execute, false);
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name="exportOutput"> if non-null, the ddl will be written to this TextWriter.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public void Create(TextWriter exportOutput, bool execute)
		{
			Execute(null, execute, false, exportOutput);
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name="exportOutput"> if non-null, the ddl will be written to this TextWriter.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public Task CreateAsync(TextWriter exportOutput, bool execute)
		{
			return ExecuteAsync(null, execute, false, exportOutput);
		}

		/// <summary>
		/// Run the drop schema script
		/// </summary>
		/// <param name="useStdOut"><see langword="true" /> if the ddl should be outputted in the Console.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to true.
		/// </remarks>
		public void Drop(bool useStdOut, bool execute)
		{
			Execute(useStdOut, execute, true);
		}

		/// <summary>
		/// Run the drop schema script
		/// </summary>
		/// <param name="useStdOut"><see langword="true" /> if the ddl should be outputted in the Console.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to true.
		/// </remarks>
		public Task DropAsync(bool useStdOut, bool execute)
		{
			return ExecuteAsync(useStdOut, execute, true);
		}

		/// <summary>
		/// Run the drop schema script
		/// </summary>
		/// <param name="exportOutput"> if non-null, the ddl will be written to this TextWriter.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(Action&lt;string&gt;, bool, bool, TextWriter)"/> and sets
		/// the justDrop parameter to true.
		/// </remarks>
		public void Drop(TextWriter exportOutput, bool execute)
		{
			Execute(null, execute, true, exportOutput);
		}

		/// <summary>
		/// Run the drop schema script
		/// </summary>
		/// <param name="exportOutput"> if non-null, the ddl will be written to this TextWriter.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref="Execute(Action&lt;string&gt;, bool, bool, TextWriter)"/> and sets
		/// the justDrop parameter to true.
		/// </remarks>
		public Task DropAsync(TextWriter exportOutput, bool execute)
		{
			return ExecuteAsync(null, execute, true, exportOutput);
		}

		private async Task Execute(Action<string> scriptAction, bool execute, bool throwOnError, TextWriter exportOutput,
							 DbCommand statement, string sql)
		{
			await Initialize().ConfigureAwait(false);
			try
			{
				string formatted = formatter.Format(sql);

				if (delimiter != null)
				{
					formatted += delimiter;
				}
				if (scriptAction != null)
				{
					scriptAction(formatted);
				}
				log.Debug(formatted);
				if (exportOutput != null)
				{
					exportOutput.WriteLine(formatted);
				}
				if (execute)
				{
					await ExecuteSql(statement, sql).ConfigureAwait(false);
				}
			}
			catch (Exception e)
			{
				log.Warn("Unsuccessful: " + sql);
				log.Warn(e.Message);
				if (throwOnError)
				{
					throw;
				}
			}
		}

		private async Task ExecuteSql(DbCommand cmd, string sql)
		{
			if (dialect.SupportsSqlBatches)
			{
				var objFactory = Environment.BytecodeProvider.ObjectsFactory;
				ScriptSplitter splitter = (ScriptSplitter)objFactory.CreateInstance(typeof(ScriptSplitter), sql);

				foreach (string stmt in splitter)
				{
					log.DebugFormat("SQL Batch: {0}", stmt);
					cmd.CommandText = stmt;
					cmd.CommandType = CommandType.Text;
					await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
				}
			}
			else
			{
				cmd.CommandText = sql;
				cmd.CommandType = CommandType.Text;
				await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Executes the Export of the Schema in the given connection
		/// </summary>
		/// <param name="useStdOut"><see langword="true" /> if the ddl should be outputted in the Console.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <param name="justDrop"><see langword="true" /> if only the ddl to drop the Database objects should be executed.</param>
		/// <param name="connection">
		/// The connection to use when executing the commands when export is <see langword="true" />.
		/// Must be an opened connection. The method doesn't close the connection.
		/// </param>
		/// <param name="exportOutput">The writer used to output the generated schema</param>
		/// <remarks>
		/// This method allows for both the drop and create ddl script to be executed.
		/// This overload is provided mainly to enable use of in memory databases. 
		/// It does NOT close the given connection!
		/// </remarks>
		public void Execute(bool useStdOut, bool execute, bool justDrop, IDbConnection connection,
							TextWriter exportOutput)
		{
			ExecuteAsync(useStdOut, execute, justDrop, connection, exportOutput).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Executes the Export of the Schema in the given connection
		/// </summary>
		/// <param name="useStdOut"><see langword="true" /> if the ddl should be outputted in the Console.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <param name="justDrop"><see langword="true" /> if only the ddl to drop the Database objects should be executed.</param>
		/// <param name="connection">
		/// The connection to use when executing the commands when export is <see langword="true" />.
		/// Must be an opened connection. The method doesn't close the connection.
		/// </param>
		/// <param name="exportOutput">The writer used to output the generated schema</param>
		/// <remarks>
		/// This method allows for both the drop and create ddl script to be executed.
		/// This overload is provided mainly to enable use of in memory databases. 
		/// It does NOT close the given connection!
		/// </remarks>
		public Task ExecuteAsync(bool useStdOut, bool execute, bool justDrop, IDbConnection connection,
							TextWriter exportOutput)
		{
			if (useStdOut)
			{
				return ExecuteAsync(Console.WriteLine, execute, justDrop, connection, exportOutput);
			}
			else
			{
				return ExecuteAsync(null, execute, justDrop, connection, exportOutput);
			}
		}

		public void Execute(Action<string> scriptAction, bool execute, bool justDrop, IDbConnection connection,
							TextWriter exportOutput)
		{
			ExecuteAsync(scriptAction, execute, justDrop, connection, exportOutput).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task ExecuteAsync(Action<string> scriptAction, bool execute, bool justDrop, IDbConnection connection,
							TextWriter exportOutput)
		{
			await Initialize().ConfigureAwait(false);
			DbCommand statement = null;

			if (execute && connection == null)
			{
				throw new ArgumentNullException("connection", "When export is set to true, you need to pass a non null connection");
			}
			if (execute)
			{
				statement = (DbCommand)connection.CreateCommand();
			}

			try
			{
				for (int i = 0; i < dropSQL.Length; i++)
				{
					await Execute(scriptAction, execute, false, exportOutput, statement, dropSQL[i]).ConfigureAwait(false);
				}

				if (!justDrop)
				{
					for (int j = 0; j < createSQL.Length; j++)
					{
						await Execute(scriptAction, execute, true, exportOutput, statement, createSQL[j]).ConfigureAwait(false);
					}
				}
			}
			finally
			{
				try
				{
					if (statement != null)
					{
						statement.Dispose();
					}
				}
				catch (Exception e)
				{
					log.Error("Could not close connection: " + e.Message, e);
				}
				if (exportOutput != null)
				{
					try
					{
						exportOutput.Close();
					}
					catch (Exception ioe)
					{
						log.Error("Error closing output file " + outputFile + ": " + ioe.Message, ioe);
					}
				}
			}
		}

		/// <summary>
		/// Executes the Export of the Schema.
		/// </summary>
		/// <param name="useStdOut"><see langword="true" /> if the ddl should be outputted in the Console.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <param name="justDrop"><see langword="true" /> if only the ddl to drop the Database objects should be executed.</param>
		/// <remarks>
		/// This method allows for both the drop and create ddl script to be executed.
		/// </remarks>
		public void Execute(bool useStdOut, bool execute, bool justDrop)
		{
			ExecuteAsync(useStdOut, execute, justDrop).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Executes the Export of the Schema.
		/// </summary>
		/// <param name="useStdOut"><see langword="true" /> if the ddl should be outputted in the Console.</param>
		/// <param name="execute"><see langword="true" /> if the ddl should be executed against the Database.</param>
		/// <param name="justDrop"><see langword="true" /> if only the ddl to drop the Database objects should be executed.</param>
		/// <remarks>
		/// This method allows for both the drop and create ddl script to be executed.
		/// </remarks>
		public Task ExecuteAsync(bool useStdOut, bool execute, bool justDrop)
		{
			if (useStdOut)
			{
				return ExecuteAsync(Console.WriteLine, execute, justDrop);
			}
			else
			{
				return ExecuteAsync(null, execute, justDrop);
			}
		}


		public void Execute(Action<string> scriptAction, bool execute, bool justDrop)
		{
			Execute(scriptAction, execute, justDrop, null);
		}

		public Task ExecuteAsync(Action<string> scriptAction, bool execute, bool justDrop)
		{
			return ExecuteAsync(scriptAction, execute, justDrop, null);
		}

		public void Execute(Action<string> scriptAction, bool execute, bool justDrop, TextWriter exportOutput)
		{
			ExecuteAsync(scriptAction, execute, justDrop, exportOutput).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task ExecuteAsync(Action<string> scriptAction, bool execute, bool justDrop, TextWriter exportOutput)
		{
			await Initialize().ConfigureAwait(false);
			DbConnection connection = null;
			TextWriter fileOutput = exportOutput;
			IConnectionProvider connectionProvider = null;

			try
			{
				if (fileOutput == null && outputFile != null)
				{
					fileOutput = new StreamWriter(outputFile);
				}

				if (execute)
				{
					var props = new Dictionary<string, string>();
					foreach (var de in dialect.DefaultProperties)
					{
						props[de.Key] = de.Value;
					}

					if (configProperties != null)
					{
						foreach (var de in configProperties)
						{
							props[de.Key] = de.Value;
						}
					}

					connectionProvider = ConnectionProviderFactory.NewConnectionProvider(props);
					connection = connectionProvider.GetConnection();
				}

				await ExecuteAsync(scriptAction, execute, justDrop, connection, fileOutput).ConfigureAwait(false);
			}
			catch (HibernateException)
			{
				// So that we don't wrap HibernateExceptions in HibernateExceptions
				throw;
			}
			catch (Exception e)
			{
				log.Error(e.Message, e);
				throw new HibernateException(e.Message, e);
			}
			finally
			{
				if (connection != null)
				{
					connectionProvider.CloseConnection(connection);
					connectionProvider.Dispose();
				}
			}
		}
	}
}