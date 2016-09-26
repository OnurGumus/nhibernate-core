#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UriTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "Uri";
			}
		}

		[Test]
		public async Task ReadWriteAsync()
		{
			using (var s = OpenSession())
			{
				var entity = new UriClass{Id = 1};
				entity.Url = new Uri("http://www.fabiomaulo.blogspot.com/");
				await (s.SaveAsync(entity));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var entity = await (s.GetAsync<UriClass>(1));
				Assert.That(entity.Url, Is.Not.Null);
				Assert.That(entity.Url.OriginalString, Is.EqualTo("http://www.fabiomaulo.blogspot.com/"));
				entity.Url = new Uri("http://fabiomaulo.blogspot.com/2010/10/nhibernate-30-cookbook.html");
				await (s.SaveAsync(entity));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var entity = await (s.GetAsync<UriClass>(1));
				Assert.That(entity.Url.OriginalString, Is.EqualTo("http://fabiomaulo.blogspot.com/2010/10/nhibernate-30-cookbook.html"));
				await (s.DeleteAsync(entity));
				await (s.FlushAsync());
			}
		}

		[Test(Description = "NH-2887")]
		public async Task ReadWriteRelativeUriAsync()
		{
			using (var s = OpenSession())
			{
				var entity = new UriClass{Id = 1};
				entity.Url = new Uri("/", UriKind.Relative);
				await (s.SaveAsync(entity));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var entity = await (s.GetAsync<UriClass>(1));
				Assert.That(entity.Url, Is.Not.Null);
				Assert.That(entity.Url.OriginalString, Is.EqualTo("/"));
				entity.Url = new Uri("/2010/10/nhibernate-30-cookbook.html", UriKind.Relative);
				await (s.SaveAsync(entity));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var entity = await (s.GetAsync<UriClass>(1));
				Assert.That(entity.Url.OriginalString, Is.EqualTo("/2010/10/nhibernate-30-cookbook.html"));
				await (s.DeleteAsync(entity));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task InsertNullValueAsync()
		{
			using (ISession s = OpenSession())
			{
				var entity = new UriClass{Id = 1};
				entity.Url = null;
				await (s.SaveAsync(entity));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				var entity = await (s.GetAsync<UriClass>(1));
				Assert.That(entity.Url, Is.Null);
				await (s.DeleteAsync(entity));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
