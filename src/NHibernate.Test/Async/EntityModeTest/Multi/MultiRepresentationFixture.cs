#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.EntityModeTest.Multi
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultiRepresentationFixture : TestCase
	{
		[Test]
		public async Task PocoRetreivalAsync()
		{
			var testData = new TestData(sessions);
			await (testData.CreateAsync());
			ISession session = OpenSession();
			ITransaction txn = session.BeginTransaction();
			var stock = await (session.GetAsync<Stock>(1));
			Assert.That(stock.Id, Is.EqualTo(1L));
			await (txn.CommitAsync());
			session.Close();
			await (testData.DestroyAsync());
		}

		[Test]
		public async Task XmlHQLAsync()
		{
			var testData = new TestData(sessions);
			await (testData.CreateAsync());
			ISession session = OpenSession();
			ITransaction txn = session.BeginTransaction();
			ISession xml = session.GetSession(EntityMode.Xml);
			IList result = await (xml.CreateQuery("from Stock").ListAsync());
			Assert.That(result.Count, Is.EqualTo(1L));
			var element = (XmlElement)result[0];
			Assert.That(element.Attributes["id"], Is.EqualTo(testData.stockId));
			Console.WriteLine("**** XML: ****************************************************");
			//prettyPrint( element );
			Console.WriteLine("**************************************************************");
			txn.Rollback();
			session.Close();
			await (testData.DestroyAsync());
		}

		[Test]
		public async Task XmlRetreivalAsync()
		{
			var testData = new TestData(sessions);
			await (testData.CreateAsync());
			ISession session = OpenSession();
			ITransaction txn = session.BeginTransaction();
			ISession xml = session.GetSession(EntityMode.Xml);
			object rtn = await (xml.GetAsync(typeof (Stock).FullName, testData.stockId));
			var element = (XmlElement)rtn;
			Assert.That(element.Attributes["id"], Is.EqualTo(testData.stockId));
			Console.WriteLine("**** XML: ****************************************************");
			//prettyPrint( element );
			Console.WriteLine("**************************************************************");
			XmlNode currVal = element.GetElementsByTagName("currentValuation")[0];
			Console.WriteLine("**** XML: ****************************************************");
			//prettyPrint( currVal );
			Console.WriteLine("**************************************************************");
			txn.Rollback();
			session.Close();
			await (testData.DestroyAsync());
		}

		[Test]
		public async Task XmlSaveAsync()
		{
			var testData = new TestData(sessions);
			await (testData.CreateAsync());
			ISession pojos = OpenSession();
			ITransaction txn = pojos.BeginTransaction();
			ISession xml = pojos.GetSession(EntityMode.Xml);
			var domDoc = new XmlDocument();
			XmlElement stock = domDoc.CreateElement("stock");
			domDoc.AppendChild(stock);
			XmlElement tradeSymbol = domDoc.CreateElement("tradeSymbol");
			tradeSymbol.InnerText = "Microsoft";
			stock.AppendChild(tradeSymbol);
			XmlElement cval = domDoc.CreateElement("currentValuation");
			XmlElement val = domDoc.CreateElement("valuation");
			stock.AppendChild(cval);
			//val.appendContent(stock); TODO
			XmlElement valuationDate = domDoc.CreateElement("valuationDate");
			tradeSymbol.InnerText = DateTime.Now.ToString();
			val.AppendChild(valuationDate);
			XmlElement value = domDoc.CreateElement("value");
			tradeSymbol.InnerText = "121.00";
			val.AppendChild(value);
			await (xml.SaveAsync(typeof (Stock).FullName, stock));
			await (xml.FlushAsync());
			txn.Rollback();
			pojos.Close();
			Assert.That(!pojos.IsOpen);
			Assert.That(!xml.IsOpen);
			//prettyPrint( stock );
			await (testData.DestroyAsync());
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TestData
		{
			public async Task CreateAsync()
			{
				ISession session = sessions.OpenSession();
				session.BeginTransaction();
				var stock = new Stock{TradeSymbol = "NHForge"};
				var valuation = new Valuation{Stock = stock, ValuationDate = DateTime.Now, Value = 200.0D};
				stock.CurrentValuation = valuation;
				stock.Valuations.Add(valuation);
				await (session.SaveAsync(stock));
				await (session.SaveAsync(valuation));
				await (session.Transaction.CommitAsync());
				session.Close();
				stockId = stock.Id;
			}

			public async Task DestroyAsync()
			{
				ISession session = sessions.OpenSession();
				session.BeginTransaction();
				IList<Stock> stocks = await (session.CreateQuery("from Stock").ListAsync<Stock>());
				foreach (Stock stock in stocks)
				{
					stock.CurrentValuation = null;
					await (session.FlushAsync());
					foreach (Valuation valuation in stock.Valuations)
					{
						await (session.DeleteAsync(valuation));
					}

					await (session.DeleteAsync(stock));
				}

				await (session.Transaction.CommitAsync());
				session.Close();
			}
		}
	}
}
#endif
