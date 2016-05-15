#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Cfg;
using NHibernate.Util;
using Environment = NHibernate.Cfg.Environment;
using NHibernate.AdoNet.Util;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SchemaUpdate
	{
		public static async Task MainAsync(string[] args)
		{
			try
			{
				var cfg = new Configuration();
				bool script = true;
				// If true then execute db updates, otherwise just generate and display updates
				bool doUpdate = true;
				//String propFile = null;
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i].StartsWith("--"))
					{
						if (args[i].Equals("--quiet"))
						{
							script = false;
						}
						else if (args[i].StartsWith("--properties="))
						{
							throw new NotSupportedException("No properties file for .NET, use app.config instead");
						//propFile = args[i].Substring( 13 );
						}
						else if (args[i].StartsWith("--config="))
						{
							cfg.Configure(args[i].Substring(9));
						}
						else if (args[i].StartsWith("--text"))
						{
							doUpdate = false;
						}
						else if (args[i].StartsWith("--naming="))
						{
							cfg.SetNamingStrategy((INamingStrategy)Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(args[i].Substring(9))));
						}
					}
					else
					{
						cfg.AddFile(args[i]);
					}
				}

				/* NH: No props file for .NET
				 * if ( propFile != null ) {
					Hashtable props = new Hashtable();
					props.putAll( cfg.Properties );
					props.load( new FileInputStream( propFile ) );
					cfg.SetProperties( props );
				}*/
				await (new SchemaUpdate(cfg).ExecuteAsync(script, doUpdate));
			}
			catch (Exception e)
			{
				log.Error("Error running schema update", e);
				Console.WriteLine(e);
			}
		}

		/// <summary>
		/// Execute the schema updates
		/// </summary>
		public async Task ExecuteAsync(bool useStdOut, bool doUpdate)
		{
			if (useStdOut)
			{
				await (ExecuteAsync(Console.WriteLine, doUpdate));
			}
			else
			{
				await (ExecuteAsync(null, doUpdate));
			}
		}

		/// <summary>
		/// Execute the schema updates
		/// </summary>
		/// <param name = "scriptAction">The action to write the each schema line.</param>
		/// <param name = "doUpdate">Commit the script to DB</param>
		public async Task ExecuteAsync(Action<string> scriptAction, bool doUpdate)
		{
			log.Info("Running hbm2ddl schema update");
			string autoKeyWordsImport = PropertiesHelper.GetString(Environment.Hbm2ddlKeyWords, configuration.Properties, "not-defined");
			autoKeyWordsImport = autoKeyWordsImport.ToLowerInvariant();
			if (autoKeyWordsImport == Hbm2DDLKeyWords.AutoQuote)
			{
				await (SchemaMetadataUpdater.QuoteTableAndColumnsAsync(configuration));
			}

			DbConnection connection;
			DbCommand stmt = null;
			exceptions.Clear();
			try
			{
				DatabaseMetadata meta;
				try
				{
					log.Info("fetching database metadata");
					await (connectionHelper.PrepareAsync());
					connection = connectionHelper.Connection;
					meta = new DatabaseMetadata(connection, dialect);
					stmt = connection.CreateCommand();
				}
				catch (Exception sqle)
				{
					exceptions.Add(sqle);
					log.Error("could not get database metadata", sqle);
					throw;
				}

				log.Info("updating schema");
				string[] createSQL = configuration.GenerateSchemaUpdateScript(dialect, meta);
				for (int j = 0; j < createSQL.Length; j++)
				{
					string sql = createSQL[j];
					string formatted = formatter.Format(sql);
					try
					{
						if (scriptAction != null)
						{
							scriptAction(formatted);
						}

						if (doUpdate)
						{
							log.Debug(sql);
							stmt.CommandText = sql;
							await (stmt.ExecuteNonQueryAsync());
						}
					}
					catch (Exception e)
					{
						exceptions.Add(e);
						log.Error("Unsuccessful: " + sql, e);
					}
				}

				log.Info("schema update complete");
			}
			catch (Exception e)
			{
				exceptions.Add(e);
				log.Error("could not complete schema update", e);
			}
			finally
			{
				try
				{
					if (stmt != null)
					{
						stmt.Dispose();
					}

					connectionHelper.Release();
				}
				catch (Exception e)
				{
					exceptions.Add(e);
					log.Error("Error closing connection", e);
				}
			}
		}
	}
}
#endif
