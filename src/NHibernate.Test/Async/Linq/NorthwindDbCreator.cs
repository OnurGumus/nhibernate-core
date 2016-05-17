#if NET_4_5
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NHibernate.DomainModel.Northwind.Entities;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class NorthwindDbCreator
	{
		public static async Task CreateMiscTestDataAsync(ISession session)
		{
			var roles = new[]{new Role()
			{Name = "Admin", IsActive = true, Entity = new AnotherEntity()
			{Output = "output"}}, new Role()
			{Name = "User", IsActive = false}};
			var users = new[]{new User("ayende", KnownDate)
			{Id = 1, Role = roles[0], InvalidLoginAttempts = 4, Enum1 = EnumStoredAsString.Medium, Enum2 = EnumStoredAsInt32.High, Component = new UserComponent()
			{Property1 = "test1", Property2 = "test2", OtherComponent = new UserComponent2()
			{OtherProperty1 = "othertest1"}}}, new User("rahien", new DateTime(1998, 12, 31))
			{Id = 2, Role = roles[1], InvalidLoginAttempts = 5, Enum1 = EnumStoredAsString.Small, Component = new UserComponent()
			{Property2 = "test2"}}, new User("nhibernate", new DateTime(2000, 1, 1))
			{Id = 3, InvalidLoginAttempts = 6, LastLoginDate = DateTime.Now.AddDays(-1), Enum1 = EnumStoredAsString.Medium, Features = FeatureSet.HasAll}};
			var timesheets = new[]{new Timesheet{Id = 1, SubmittedDate = KnownDate, Submitted = true}, new Timesheet{Id = 2, SubmittedDate = KnownDate.AddDays(-1), Submitted = false, Entries = new List<TimesheetEntry>{new TimesheetEntry{Id = 1, EntryDate = KnownDate, NumberOfHours = 6, Comments = "testing 123"}, new TimesheetEntry{Id = 2, EntryDate = KnownDate.AddDays(1), NumberOfHours = 14}}}, new Timesheet{Id = 3, SubmittedDate = DateTime.Now.AddDays(1), Submitted = true, Entries = new List<TimesheetEntry>{new TimesheetEntry{Id = 3, EntryDate = DateTime.Now.AddMinutes(20), NumberOfHours = 4}, new TimesheetEntry{Id = 4, EntryDate = DateTime.Now.AddMinutes(10), NumberOfHours = 8, Comments = "testing 456"}, new TimesheetEntry{Id = 5, EntryDate = DateTime.Now.AddMinutes(13), NumberOfHours = 7}, new TimesheetEntry{Id = 6, EntryDate = DateTime.Now.AddMinutes(45), NumberOfHours = 38}}}};
			((IList<User>)timesheets[0].Users).Add(users[0]);
			((IList<User>)timesheets[1].Users).Add(users[0]);
			((IList<User>)timesheets[0].Users).Add(users[1]);
			var animals = new Animal[]{new Animal()
			{SerialNumber = "123", BodyWeight = 100}, new Lizard()
			{SerialNumber = "789", BodyWeight = 40, BodyTemperature = 14}, new Lizard()
			{SerialNumber = "1234", BodyWeight = 30, BodyTemperature = 18}, new Dog()
			{SerialNumber = "5678", BodyWeight = 156, BirthDate = new DateTime(1980, 07, 11)}, new Dog()
			{SerialNumber = "9101", BodyWeight = 205, BirthDate = new DateTime(1980, 12, 13)}, new Cat()
			{SerialNumber = "1121", BodyWeight = 115, Pregnant = true}};
			animals[0].Children = new[]{animals[3], animals[4]}.ToList();
			animals[5].Father = animals[3];
			animals[5].Mother = animals[4];
			animals[1].Children = new[]{animals[5]}.ToList();
			List<AnotherEntity> otherEntities = new List<AnotherEntity>();
			// AnotherEntity with only Output set is created above.
			otherEntities.Add(new AnotherEntity{Input = "input"});
			otherEntities.Add(new AnotherEntity{Input = "i/o", Output = "i/o"});
			otherEntities.Add(new AnotherEntity()); // Input and Output both null.
			otherEntities.Add(new AnotherEntity{Input = "input", Output = "output"});
			foreach (AnotherEntity otherEntity in otherEntities)
				await (session.SaveAsync(otherEntity));
			foreach (Role role in roles)
				await (session.SaveAsync(role));
			foreach (User user in users)
				await (session.SaveAsync(user));
			foreach (Timesheet timesheet in timesheets)
				await (session.SaveAsync(timesheet));
			foreach (Animal animal in animals)
				await (session.SaveAsync(animal));
		}

		public static async Task CreatePatientDataAsync(ISession session)
		{
			State newYork = new State{Abbreviation = "NY", FullName = "New York"};
			State florida = new State{Abbreviation = "FL", FullName = "Florida"};
			Physician drDobbs = new Physician{Name = "Dr Dobbs"};
			Physician drWatson = new Physician{Name = "Dr Watson"};
			PatientRecord bobBarkerRecord = new PatientRecord{Name = new PatientName{FirstName = "Bob", LastName = "Barker"}, Address = new PatientAddress{AddressLine1 = "123 Main St", City = "New York", State = newYork, ZipCode = "10001"}, BirthDate = new DateTime(1930, 1, 1), Gender = Gender.Male};
			PatientRecord johnDoeRecord1 = new PatientRecord{Name = new PatientName{FirstName = "John", LastName = "Doe"}, Address = new PatientAddress{AddressLine1 = "123 Main St", City = "Tampa", State = florida, ZipCode = "33602"}, BirthDate = new DateTime(1969, 1, 1), Gender = Gender.Male};
			PatientRecord johnDoeRecord2 = new PatientRecord{Name = new PatientName{FirstName = "John", LastName = "Doe"}, Address = new PatientAddress{AddressLine1 = "123 Main St", AddressLine2 = "Apt 2", City = "Tampa", State = florida, ZipCode = "33602"}, BirthDate = new DateTime(1969, 1, 1)};
			Patient bobBarker = new Patient(new[]{bobBarkerRecord}, false, drDobbs);
			Patient johnDoe = new Patient(new[]{johnDoeRecord1, johnDoeRecord2}, true, drWatson);
			await (session.SaveAsync(newYork));
			await (session.SaveAsync(florida));
			await (session.SaveAsync(drDobbs));
			await (session.SaveAsync(drWatson));
			await (session.SaveAsync(bobBarker));
			await (session.SaveAsync(johnDoe));
		}
	}
}
#endif
