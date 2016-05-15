#if NET_4_5
using System;
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.CompositeId
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ClassWithCompositeIdFixture : TestCase
	{
		/// <summary>
		/// Test the basic CRUD operations for a class with a Composite Identifier
		/// </summary>
		/// <remarks>
		/// The following items are tested in this Test Script
		/// <list type = "">
		///		<item>
		///			<term>Save</term>
		///		</item>
		///		<item>
		///			<term>Load</term>
		///		</item>
		///		<item>
		///			<term>Criteria</term>
		///		</item>
		///		<item>
		///			<term>Update</term>
		///		</item>
		///		<item>
		///			<term>Delete</term>
		///		</item>
		///		<item>
		///			<term>Criteria - No Results</term>
		///		</item>
		/// </list>
		/// </remarks>
		[Test]
		public async Task TestSimpleCRUDAsync()
		{
			// insert the new objects
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			ClassWithCompositeId theClass = new ClassWithCompositeId(id);
			theClass.OneProperty = 5;
			ClassWithCompositeId theSecondClass = new ClassWithCompositeId(secondId);
			theSecondClass.OneProperty = 10;
			await (s.SaveAsync(theClass));
			await (s.SaveAsync(theSecondClass));
			await (t.CommitAsync());
			s.Close();
			// verify they were inserted and test the SELECT
			ISession s2 = OpenSession();
			ITransaction t2 = s2.BeginTransaction();
			ClassWithCompositeId theClass2 = (ClassWithCompositeId)await (s2.LoadAsync(typeof (ClassWithCompositeId), id));
			Assert.AreEqual(id, theClass2.Id);
			IList results2 = await (s2.CreateCriteria(typeof (ClassWithCompositeId)).Add(Expression.Eq("Id", secondId)).ListAsync());
			Assert.AreEqual(1, results2.Count);
			ClassWithCompositeId theSecondClass2 = (ClassWithCompositeId)results2[0];
			ClassWithCompositeId theClass2Copy = (ClassWithCompositeId)await (s2.LoadAsync(typeof (ClassWithCompositeId), id));
			// verify the same results through Criteria & Load were achieved
			Assert.AreSame(theClass2, theClass2Copy);
			// compare them to the objects created in the first session
			Assert.AreEqual(theClass.Id, theClass2.Id);
			Assert.AreEqual(theClass.OneProperty, theClass2.OneProperty);
			Assert.AreEqual(theSecondClass.Id, theSecondClass2.Id);
			Assert.AreEqual(theSecondClass.OneProperty, theSecondClass2.OneProperty);
			// test the update functionallity
			theClass2.OneProperty = 6;
			theSecondClass2.OneProperty = 11;
			await (s2.UpdateAsync(theClass2));
			await (s2.UpdateAsync(theSecondClass2));
			await (t2.CommitAsync());
			s2.Close();
			// lets verify the update went through
			ISession s3 = OpenSession();
			ITransaction t3 = s3.BeginTransaction();
			ClassWithCompositeId theClass3 = (ClassWithCompositeId)await (s3.LoadAsync(typeof (ClassWithCompositeId), id));
			ClassWithCompositeId theSecondClass3 = (ClassWithCompositeId)await (s3.LoadAsync(typeof (ClassWithCompositeId), secondId));
			// check the update properties
			Assert.AreEqual(theClass3.OneProperty, theClass2.OneProperty);
			Assert.AreEqual(theSecondClass3.OneProperty, theSecondClass2.OneProperty);
			// test the delete method
			await (s3.DeleteAsync(theClass3));
			await (s3.DeleteAsync(theSecondClass3));
			await (t3.CommitAsync());
			s3.Close();
			// lets verify the delete went through
			ISession s4 = OpenSession();
			try
			{
				ClassWithCompositeId theClass4 = (ClassWithCompositeId)await (s4.LoadAsync(typeof (ClassWithCompositeId), id));
			}
			catch (ObjectNotFoundException onfe)
			{
				// I expect this to be thrown because the object no longer exists...
				Assert.IsNotNull(onfe); //getting ride of 'onfe' is never used compile warning
			}

			IList results = await (s4.CreateCriteria(typeof (ClassWithCompositeId)).Add(Expression.Eq("Id", secondId)).ListAsync());
			Assert.AreEqual(0, results.Count);
			s4.Close();
		}

		[Test]
		public async Task CriteriaAsync()
		{
			Id id = new Id("stringKey", 3, firstDateTime);
			ClassWithCompositeId cId = new ClassWithCompositeId(id);
			cId.OneProperty = 5;
			// add the new instance to the session so I have something to get results 
			// back for
			ISession s = OpenSession();
			await (s.SaveAsync(cId));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			ICriteria c = s.CreateCriteria(typeof (ClassWithCompositeId));
			c.Add(Expression.Eq("Id", id));
			// right now just want to see if the Criteria is valid
			IList results = await (c.ListAsync());
			Assert.AreEqual(1, results.Count);
			s.Close();
		}

		[Test]
		public async Task HqlAsync()
		{
			// insert the new objects
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			ClassWithCompositeId theClass = new ClassWithCompositeId(id);
			theClass.OneProperty = 5;
			ClassWithCompositeId theSecondClass = new ClassWithCompositeId(secondId);
			theSecondClass.OneProperty = 10;
			await (s.SaveAsync(theClass));
			await (s.SaveAsync(theSecondClass));
			await (t.CommitAsync());
			s.Close();
			ISession s2 = OpenSession();
			IQuery hql = s2.CreateQuery("from ClassWithCompositeId as cwid where cwid.Id.KeyString = :keyString");
			hql.SetString("keyString", id.KeyString);
			IList results = await (hql.ListAsync());
			Assert.AreEqual(1, results.Count);
			s2.Close();
		}
	}
}
#endif
