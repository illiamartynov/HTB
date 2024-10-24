using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class PersonTests
    {
        private Person person;

        [SetUp]
        public void Setup()
        {
            Person.Extent.Clear(); 
            person = new Person("person@example.com", "John Doe", "password", DateTime.Now, DateTime.Now.AddYears(-25), true, 100, new Profile(500, "Intermediate"), new Leaderboard());
        }

        [Test]
        public void TestPersonCreation()
        {
            Assert.That(person.Name, Is.EqualTo("John Doe"));
        }

        [Test]
        public void TestExtentSerialization()
        {
            var person2 = new Person("person2@example.com", "Jane Doe", "password", DateTime.Now, DateTime.Now.AddYears(-30), true, 150, new Profile(600, "Advanced"), new Leaderboard());

            Person.SaveExtent("test_person_extent.json");

            Person.Extent.Clear();
            Assert.That(Person.Extent.Count, Is.EqualTo(0));  

            Person.LoadExtent("test_person_extent.json");

            Assert.That(Person.Extent.Count, Is.EqualTo(2));
        }
    }
}