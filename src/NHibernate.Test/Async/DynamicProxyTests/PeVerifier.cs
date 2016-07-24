#if NET_4_5
using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.DynamicProxyTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PeVerifier
	{
		public async Task AssertIsValidAsync()
		{
			var process = new Process{StartInfo = {FileName = _peVerifyPath, RedirectStandardOutput = true, UseShellExecute = false, Arguments = "\"" + _assemlyLocation + "\" /VERBOSE", CreateNoWindow = true}};
			process.Start();
			var processOutput = await (process.StandardOutput.ReadToEndAsync());
			process.WaitForExit();
			var result = process.ExitCode + " code ";
			if (process.ExitCode != 0)
				Assert.Fail("PeVerify reported error(s): " + Environment.NewLine + processOutput, result);
		}
	}
}
#endif
