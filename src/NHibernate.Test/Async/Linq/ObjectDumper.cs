#if NET_4_5
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ObjectDumper
	{
		public static Task<string> WriteAsync(object o)
		{
			return WriteAsync(o, 0);
		}

		public static async Task<string> WriteAsync(object o, int depth)
		{
			var dumper = new ObjectDumper(depth);
			await (dumper.WriteObjectAsync(null, o));
			return dumper._writer.ToString();
		}

		private async Task WriteAsync(string s)
		{
			if (s != null)
			{
				await (_writer.WriteAsync(s));
				_pos += s.Length;
			}
		}

		private async Task WriteIndentAsync()
		{
			for (int i = 0; i < _level; i++)
				await (_writer.WriteAsync("  "));
		}

		private async Task WriteLineAsync()
		{
			await (_writer.WriteLineAsync());
			_pos = 0;
		}

		private async Task WriteTabAsync()
		{
			await (WriteAsync("  "));
			while (_pos % 8 != 0)
				await (WriteAsync(" "));
		}

		private async Task WriteObjectAsync(string prefix, object o)
		{
			if (o == null || o is ValueType || o is string)
			{
				await (WriteIndentAsync());
				await (WriteAsync(prefix));
				await (WriteValueAsync(o));
				await (WriteLineAsync());
			}
			else if (o is IEnumerable)
			{
				foreach (object element in (IEnumerable)o)
				{
					if (element is IEnumerable && !(element is string))
					{
						await (WriteIndentAsync());
						await (WriteAsync(prefix));
						await (WriteAsync("..."));
						await (WriteLineAsync());
						if (_level < _depth)
						{
							_level++;
							await (WriteObjectAsync(prefix, element));
							_level--;
						}
					}
					else
					{
						await (WriteObjectAsync(prefix, element));
					}
				}
			}
			else
			{
				MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
				await (WriteIndentAsync());
				await (WriteAsync(prefix));
				bool propWritten = false;
				foreach (MemberInfo m in members)
				{
					var f = m as FieldInfo;
					var p = m as PropertyInfo;
					if (f != null || p != null)
					{
						if (propWritten)
						{
							await (WriteTabAsync());
						}
						else
						{
							propWritten = true;
						}

						await (WriteAsync(m.Name));
						await (WriteAsync("="));
						System.Type t = f != null ? f.FieldType : p.PropertyType;
						if (t.IsValueType || t == typeof (string))
						{
							await (WriteValueAsync(f != null ? f.GetValue(o) : p.GetValue(o, null)));
						}
						else
						{
							if (typeof (IEnumerable).IsAssignableFrom(t))
							{
								await (WriteAsync("..."));
							}
							else
							{
								await (WriteAsync("{ }"));
							}
						}
					}
				}

				if (propWritten)
					await (WriteLineAsync());
				if (_level < _depth)
				{
					foreach (MemberInfo m in members)
					{
						var f = m as FieldInfo;
						var p = m as PropertyInfo;
						if (f != null || p != null)
						{
							System.Type t = f != null ? f.FieldType : p.PropertyType;
							if (!(t.IsValueType || t == typeof (string)))
							{
								object value = f != null ? f.GetValue(o) : p.GetValue(o, null);
								if (value != null)
								{
									_level++;
									await (WriteObjectAsync(m.Name + ": ", value));
									_level--;
								}
							}
						}
					}
				}
			}
		}

		private async Task WriteValueAsync(object o)
		{
			if (o == null)
			{
				await (WriteAsync("null"));
			}
			else if (o is DateTime)
			{
				await (WriteAsync(((DateTime)o).ToShortDateString()));
			}
			else if (o is ValueType || o is string)
			{
				await (WriteAsync(o.ToString()));
			}
			else if (o is IEnumerable)
			{
				await (WriteAsync("..."));
			}
			else
			{
				await (WriteAsync("{ }"));
			}
		}
	}
}
#endif
