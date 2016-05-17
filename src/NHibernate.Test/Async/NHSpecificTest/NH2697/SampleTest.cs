#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2697
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		[Test]
		public async Task Can_GetListOfArticleGroupsAsync()
		{
			string HQL;
			IList<ArticleGroupItem> result;
			//add new
			using (ISession session = this.OpenSession())
			{
				ArticleGroupItem item = new ArticleGroupItem();
				item.Name = "Test article group";
				await (session.SaveAsync(item));
				await (session.FlushAsync());
			}

			HQL = "from ArticleGroupItem";
			using (ISession session = this.OpenSession())
			{
				result = session.CreateQuery(HQL).List<ArticleGroupItem>();
			}

			Assert.That(result.Count, Is.GreaterThan(0));
		}

		[Test]
		public async Task Can_GetListOfArticlesAsync()
		{
			string HQL;
			IList<ArticleItem> result;
			//add new
			using (ISession session = this.OpenSession())
			{
				ArticleItem item = new ArticleItem();
				item.Name = "Test article";
				item.IsFavorite = 0;
				session.Save("Article", item);
				await (session.FlushAsync());
			}

			//here first problem, no entities are returned <========
			HQL = "from Article";
			using (ISession session = this.OpenSession())
			{
				result = session.CreateQuery(HQL).List<ArticleItem>();
			}

			Assert.That(result.Count, Is.GreaterThan(0));
		}

		[Test]
		public async Task Can_SetArticleFavoriteWithHQL_NamedParamAsync()
		{
			string HQL;
			IList<ArticleItem> result;
			Int16 isFavValue = 1;
			//set  isFavorite for all articles
			HQL = "update Article a set a.IsFavorite= :Fav";
			using (ISession session = this.OpenSession())
			{
				session.CreateQuery(HQL).SetInt16("Fav", isFavValue) //Exception !!
				//.SetParameter("Fav", isFavValue) //Exception also !!
				.ExecuteUpdate();
				await (session.FlushAsync());
			}

			//Check if some articles have isFavorite=1
			HQL = "from Article a where a.IsFavorite=1";
			using (ISession session = this.OpenSession())
			{
				result = session.CreateQuery(HQL).List<ArticleItem>();
			}

			Assert.That(result.Count, Is.GreaterThan(0));
		}
	}
}
#endif
