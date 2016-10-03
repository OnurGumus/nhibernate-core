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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = this.OpenSession())
			{
				ArticleGroupItem agrp_1 = new ArticleGroupItem();
				agrp_1.Name = "Article group 1";
				await (session.SaveAsync(agrp_1));
				ArticleGroupItem agrp_2 = new ArticleGroupItem();
				agrp_2.Name = "Article group 2";
				await (session.SaveAsync(agrp_2));
				await (session.FlushAsync());
				ArticleItem article_1 = new ArticleItem();
				article_1.Articlegroup = agrp_1;
				article_1.Name = "Article 1 grp 1";
				article_1.IsFavorite = 0;
				await (session.SaveAsync("Article", article_1));
				ArticleItem article_2 = new ArticleItem();
				article_2.Articlegroup = agrp_1;
				article_2.Name = "Article 2 grp 1";
				article_2.IsFavorite = 1;
				await (session.SaveAsync("Article", article_2));
				ArticleItem article_3 = new ArticleItem();
				article_3.Articlegroup = agrp_2;
				article_3.Name = "Article 1 grp 2";
				article_3.IsFavorite = 0;
				await (session.SaveAsync("Article", article_3));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = this.OpenSession())
			{
				IList<ArticleItem> list = await (session.CreateCriteria("Article").ListAsync<ArticleItem>());
				foreach (ArticleItem item in list)
					await (session.DeleteAsync("Article", item));
				await (session.FlushAsync());
			}

			//Articles where not removed (!?)
			//using (ISession session = this.OpenSession()) {
			//    string hql = "from Article";
			//    session.Delete(hql);
			//    session.Flush();
			//}
			using (ISession session = this.OpenSession())
			{
				string hql = "from ArticleGroupItem";
				await (session.DeleteAsync(hql));
				await (session.FlushAsync());
			}
		}

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
				result = await (session.CreateQuery(HQL).ListAsync<ArticleGroupItem>());
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
				await (session.SaveAsync("Article", item));
				await (session.FlushAsync());
			}

			//here first problem, no entities are returned <========
			HQL = "from Article";
			using (ISession session = this.OpenSession())
			{
				result = await (session.CreateQuery(HQL).ListAsync<ArticleItem>());
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
				await (session.CreateQuery(HQL).SetInt16("Fav", isFavValue) //Exception !!
				//.SetParameter("Fav", isFavValue) //Exception also !!
				.ExecuteUpdateAsync());
				await (session.FlushAsync());
			}

			//Check if some articles have isFavorite=1
			HQL = "from Article a where a.IsFavorite=1";
			using (ISession session = this.OpenSession())
			{
				result = await (session.CreateQuery(HQL).ListAsync<ArticleItem>());
			}

			Assert.That(result.Count, Is.GreaterThan(0));
		}
	}
}
#endif
