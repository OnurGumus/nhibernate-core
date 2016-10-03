#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using NHibernate.AdoNet.Util;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Util;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SchemaExport
	{
		private async Task InitializeAsync()
		{
			if (wasInitialized)
			{
				return;
			}

			string autoKeyWordsImport = PropertiesHelper.GetString(Environment.Hbm2ddlKeyWords, configProperties, "not-defined");
			autoKeyWordsImport = autoKeyWordsImport.ToLowerInvariant();
			if (autoKeyWordsImport == Hbm2DDLKeyWords.AutoQuote)
			{
				await (SchemaMetadataUpdater.QuoteTableAndColumnsAsync(cfg));
			}

			dialect = Dialect.Dialect.GetDialect(configProperties);
			dropSQL = cfg.GenerateDropSchemaScript(dialect);
			createSQL = cfg.GenerateSchemaCreationScript(dialect);
			formatter = (PropertiesHelper.GetBoolean(Environment.FormatSql, configProperties, true) ? FormatStyle.Ddl : FormatStyle.None).Formatter;
			wasInitialized = true;
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name = "useStdOut"><see langword = "true"/> if the ddl should be outputted in the Console.</param>
		/// <param name = "execute"><see langword = "true"/> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref = "Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public Task CreateAsync(bool useStdOut, bool execute)
		{
			return ExecuteAsync(useStdOut, execute, false);
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name = "scriptAction"> an action that will be called for each line of the generated ddl.</param>
		/// <param name = "execute"><see langword = "true"/> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref = "Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public Task CreateAsync(Action<string> scriptAction, bool execute)
		{
			return ExecuteAsync(scriptAction, execute, false);
		}

		/// <summary>
		/// Run the schema creation script
		/// </summary>
		/// <param name = "exportOutput"> if non-null, the ddl will be written to this TextWriter.</param>
		/// <param name = "execute"><see langword = "true"/> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref = "Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to false.
		/// </remarks>
		public Task CreateAsync(TextWriter exportOutput, bool execute)
		{
			return ExecuteAsync(null, execute, false, exportOutput);
		}

		/// <summary>
		/// Run the drop schema script
		/// </summary>
		/// <param name = "useStdOut"><see langword = "true"/> if the ddl should be outputted in the Console.</param>
		/// <param name = "execute"><see langword = "true"/> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref = "Execute(bool, bool, bool)"/> and sets
		/// the justDrop parameter to true.
		/// </remarks>
		public Task DropAsync(bool useStdOut, bool execute)
		{
			return ExecuteAsync(useStdOut, execute, true);
		}

		/// <summary>
		/// Run the drop schema script
		/// </summary>
		/// <param name = "exportOutput"> if non-null, the ddl will be written to this TextWriter.</param>
		/// <param name = "execute"><see langword = "true"/> if the ddl should be executed against the Database.</param>
		/// <remarks>
		/// This is a convenience method that calls <see cref = "Execute(Action&lt;string&gt;, bool, bool, TextWriter)"/> and sets
		/// the justDrop parameter to true.
		/// </remarks>
		public Task DropAsync(TextWriter exportOutput, bool execute)
		{
			return ExecuteAsync(null, execute, true, exportOutput);
		}

		private async Task ExecuteAsync(Action<string> scriptAction, bool execute, bool throwOnError, TextWriter exportOutput, DbCommand statement, string sql)
		{
			await (InitializeAsync());
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
					await (ExecuteSqlAsync(statement, sql));
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

		private async Task ExecuteSqlAsync(DbCommand cmd, string sql)
		{
			if (dialect.SupportsSqlBatches)
			{
				var objFactory = Environment.BytecodeProvider.ObjectsFactory;
				ScriptSplitter splitter = (ScriptSplitter)objFactory.CreateInstance(typeof (ScriptSplitter), sql);
				foreach (string stmt in splitter)
				{
					log.DebugFormat("SQL Batch: {0}", stmt);
					cmd.CommandText = stmt;
					cmd.CommandType = CommandType.Text;
					await (cmd.ExecuteNonQueryAsync());
				}
			}
			else
			{
				cmd.CommandText = sql;
				cmd.CommandType = CommandType.Text;
				await (cmd.ExecuteNonQueryAsync());
			}
		}

		/// <summary>
		/// Executes the Export of the Schema in the given connection
		/// </summary>
		/// <param name = "useStdOut"><see langword = "true"/> if the ddl should be outputted in the Console.</param>
		/// <param name = "execute"><see langword = "true"/> if the ddl should be executed against the Database.</param>
		/// <param name = "justDrop"><see langword = "true"/> if only the ddl to drop the Database objects should be executed.</param>
		/// <param name = "connection">
		/// The connection to use when executing the commands when export is <see langword = "true"/>.
		/// Must be an opened connection. The method doesn't close the connection.
		/// </param>
		/// <param name = "exportOutput">The writer used to output the generated schema</param>
		/// <remarks>
		/// This method allows for both the drop and create ddl script to be executed.
		/// This overload is provided mainly to enable use of in memory databases. 
		/// It does NOT close the given connection!
		/// </remarks>
		public async Task ExecuteAsync(bool useStdOut, bool execute, bool justDrop, DbConnection connection, TextWriter exportOutput)
		{
			if (useStdOut)
			{
				await (ExecuteAsync(Console.WriteLine, execute, justDrop, connection, exportOutput));
			}
			else
			{
				await (ExecuteAsync(null, execute, justDrop, connection, exportOutput));
			}
		}

		public async Task ExecuteAsync(Action<string> scriptAction, bool execute, bool justDrop, DbConnection connection, TextWriter exportOutput)
		{
			await (InitializeAsync());
			DbCommand statement = null;
			if (execute && connection == null)
			{
				throw new ArgumentNullException("connection", "When export is set to true, you need to pass a non null connection");
			}

			if (execute)
			{
				statement = connection.CreateCommand();
			}

			try
			{
				for (int i = 0; i < dropSQL.Length; i++)
				{
					await (ExecuteAsync(scriptAction, execute, false, exportOutput, statement, dropSQL[i]));
				}

				if (!justDrop)
				{
					for (int j = 0; j < createSQL.Length; j++)
					{
						await (ExecuteAsync(scriptAction, execute, true, exportOutput, statement, createSQL[j]));
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
		/// <param name = "useStdOut"><see langword = "true"/> if the ddl should be outputted in the Console.</param>
		/// <param name = "execute"><see langword = "true"/> if the ddl should be executed against the Database.</param>
		/// <param name = "justDrop"><see langword = "true"/> if only the ddl to drop the Database objects should be executed.</param>
		/// <remarks>
		/// This method allows for both the drop and create ddl script to be executed.
		/// </remarks>
		public async Task ExecuteAsync(bool useStdOut, bool execute, bool justDrop)
		{
			if (useStdOut)
			{
				await (ExecuteAsync(Console.WriteLine, execute, justDrop));
			}
			else
			{
				await (ExecuteAsync(null, execute, justDrop));
			}
		}

		public Task ExecuteAsync(Action<string> scriptAction, bool execute, bool justDrop)
		{
			return ExecuteAsync(scriptAction, execute, justDrop, null);
		}

		public async Task ExecuteAsync(Action<string> scriptAction, bool execute, bool justDrop, TextWriter exportOutput)
		{
			await (InitializeAsync());
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
					connection = await (connectionProvider.GetConnectionAsync());
				}

				await (ExecuteAsync(scriptAction, execute, justDrop, connection, fileOutput));
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
#endif
