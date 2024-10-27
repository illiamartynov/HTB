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
            person = new Person("person@example.com", "Volodymyr Zelensky", "password", DateTime.Now, DateTime.Now.AddYears(-25), true, 100, new Profile(500, "intermediate"), new Leaderboard());
        }

        [Test]
        public void TestPersonCreation()
        {
            Assert.That(person.Name, Is.EqualTo("Volodymyr Zelensky"));
        }

        [Test]
        public void TestExtentSerialization()
        {
            var person2 = new Person("person2@example.com", "Volodymyr Zelensky", "password", DateTime.Now, DateTime.Now.AddYears(-30), true, 150, new Profile(600, "advanced"), new Leaderboard());

            Person.SaveExtent("test_person_extent.json");

            Person.Extent.Clear();
            Assert.That(Person.Extent.Count, Is.EqualTo(0));  

            Person.LoadExtent("test_person_extent.json");

            Assert.That(Person.Extent.Count, Is.EqualTo(2));
        }
    }
}