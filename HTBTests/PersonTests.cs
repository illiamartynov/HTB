using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
}

namespace ProjectTests
{
    public class PersonTests
    {
        private Person person;

        [SetUp]
        public void Setup()
        {
            var profile = new Profile(127, "Intermediate");
            var leaderboard = new Leaderboard(1, 500);

            person = new Person("qwe@qwe.com", "qwe qwe", "qweqweqwe", DateTime.Now, new DateTime(1999, 1, 1), true, 100, profile, leaderboard);
        }

        [Test]
        public void TestPersonCreation()
        {
            Assert.That(person.Email, Is.EqualTo("qwe@qwe.com"));
            Assert.That(person.Name, Is.EqualTo("qwe qwe"));
            Assert.That(person.Balance, Is.EqualTo(100));
        }

        [Test]
        public void TestPersonAgeCalculation()
        {
            int expectedAge = DateTime.Now.Year - 1999;
            if (DateTime.Now.DayOfYear < new DateTime(1999, 1, 1).DayOfYear)
            {
                expectedAge--;
            }
            Assert.That(person.Age, Is.EqualTo(expectedAge));
        }

        [Test]
        public void TestPersonLogin()
        {
            Assert.DoesNotThrow(() => person.Login());
        }

        [Test]
        public void TestExtentSerialization()
        {
            var profile2 = new Profile(100, "Beginner");
            var leaderboard2 = new Leaderboard(2, 250);

            var person2 = new Person("person2@example.com", "Person Two", "password", DateTime.Now, new DateTime(1992, 7, 15), false, 50, profile2, leaderboard2);

            Person.SaveExtent("test_person_extent.json");

            Person.LoadExtent("test_person_extent.json");

            Assert.That(Person.Extent.Count, Is.EqualTo(2));
        }
    }
}