#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Cfg;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SchemaValidator
	{
		public static async Task MainAsync(string[] args)
		{
			try
			{
				var cfg = new Configuration();
				//string propFile = null;
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i].StartsWith("--"))
					{
						//if (args[i].StartsWith("--properties="))
						//{
						//  propFile = args[i].Substring(13);
						//}
						//else 
						if (args[i].StartsWith("--config="))
						{
							cfg.Configure(args[i].Substring(9));
						}
						else if (args[i].StartsWith("--naming="))
						{
							cfg.SetNamingStrategy((INamingStrategy)Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(args[i].Substring(9))));
						}
					}
					else
					{
						cfg.AddFile(args[i]);
					}
				}

				/* NH: No props file for .NET
				if ( propFile != null ) {
					Properties props = new Properties();
					props.putAll( cfg.getProperties() );
					props.load( new FileInputStream( propFile ) );
					cfg.setProperties( props );
				}
				*/
				await (new SchemaValidator(cfg).ValidateAsync());
			}
			catch (Exception e)
			{
				log.Error("Error running schema update", e);
				Console.WriteLine(e);
			}
		}

		/// <summary>
		/// Perform the validations.
		/// </summary>
		public async Task ValidateAsync()
		{
			log.Info("Running schema validator");
			try
			{
				DatabaseMetadata meta;
				try
				{
					log.Info("fetching database metadata");
					await (connectionHelper.PrepareAsync());
					DbConnection connection = connectionHelper.Connection;
					meta = new DatabaseMetadata(connection, dialect, false);
				}
				catch (Exception sqle)
				{
					log.Error("could not get database metadata", sqle);
					throw;
				}

				configuration.ValidateSchema(dialect, meta);
			}
			catch (Exception e)
			{
				log.Error("could not complete schema validation", e);
				throw;
			}
			finally
			{
				try
				{
					connectionHelper.Release();
				}
				catch (Exception e)
				{
					log.Error("Error closing connection", e);
				}
			}
		}
	}
}
#endif
