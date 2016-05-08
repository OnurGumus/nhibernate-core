using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace NHibernate.AsyncGenerator.Extensions
{
	internal static class SymbolInfoExtensions
	{
		public static IEnumerable<ISymbol> GetAllSymbols(this SymbolInfo info)
		{
			return info.Symbol == null && info.CandidateSymbols.Length == 0
				? new List<ISymbol>()
				: GetAllSymbolsWorker(info).Distinct();
		}

		private static IEnumerable<ISymbol> GetAllSymbolsWorker(this SymbolInfo info)
		{
			if (info.Symbol != null)
			{
				yield return info.Symbol;
			}

			foreach (var symbol in info.CandidateSymbols)
			{
				yield return symbol;
			}
		}

		public static ISymbol GetAnySymbol(this SymbolInfo info)
		{
			return info.GetAllSymbols().FirstOrDefault();
		}

		public static IEnumerable<ISymbol> GetBestOrAllSymbols(this SymbolInfo info)
		{
			if (info.Symbol != null)
			{
				return new ISymbol[] { info.Symbol };
			}
			else if (info.CandidateSymbols.Length > 0)
			{
				return info.CandidateSymbols;
			}

			return new List<ISymbol>();
		}
	}
}
