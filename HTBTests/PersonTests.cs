using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class PersonTests
    {
        private Person person;

        [SetUp]
        public void Setup()
        {
            Person.ClearExtent();
            Address.ClearAddresses();

            var initialAddress = Address.AddAddress("Ukraine", "Kyiv", "Main Street", 1);

            person = Person.AddPerson(
                "person@example.com",
                "Volodymyr Zelensky",
                "password",
                DateTime.Now,
                DateTime.Now.AddYears(-25),
                true,
                100,
                new Profile(500, "Intermediate"),
                new Leaderboard(),
                initialAddress, // Передаем корректный адрес
                new Rank(3),
                new CompletenessLevel(85, DateTime.Now),
                new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Premium, new Paid(100))
            );
        }



        [Test]
        public void TestPersonCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(person.Name, Is.EqualTo("Volodymyr Zelensky"));
                Assert.That(person.Email, Is.EqualTo("person@example.com"));
                Assert.That(person.Balance, Is.EqualTo(100));
                Assert.That(person.UserProfile.AcademyLevel, Is.EqualTo("Intermediate"));
            });
        }

        [Test]
        public void TestAssignAddressToPerson()
        {
            var newAddress = Address.AddAddress("Ukraine", "Kyiv", "Main Street", 2);
            person.AssignAddress(newAddress);

            Assert.Multiple(() =>
            {
                Assert.That(person.Address, Is.EqualTo(newAddress), "Person should have the assigned address.");
                Assert.That(newAddress.Persons.Contains(person), Is.True, "Address should contain the assigned person.");
            });
        }




        [Test]
        public void TestRemoveAddressFromPerson()
        {
            var address = Address.AddAddress("Ukraine", "Kyiv", "Main Street", 1);
            person.AssignAddress(address);
            person.RemoveAddress();

            Assert.Multiple(() =>
            {
                Assert.That(person.Address, Is.Null);
                Assert.That(!address.Persons.Contains(person));
            });
        }

        [Test]
        public void TestExtentSerialization()
        {
            var address1 = Address.AddAddress("Ukraine", "Kyiv", "Main Street", 1);
            var address2 = Address.AddAddress("USA", "New York", "5th Avenue", 10);

            var person2 = Person.AddPerson(
                "person2@example.com", 
                "Test User 2", 
                "password", 
                DateTime.Now, 
                DateTime.Now.AddYears(-30), 
                true, 
                150, 
                new Profile(600, "Advanced"), 
                new Leaderboard(), 
                address2, // Передаем существующий адрес
                new Rank(4), 
                new CompletenessLevel(70, DateTime.Now.AddMonths(-1)), 
                new Subscription(2, DateTime.Now, DateTime.Now.AddMonths(2), SubscriptionType.Free, new Free(30))
            );

            Person.SaveExtent("test_person_extent.json");

            Person.ClearExtent();
            Assert.That(Person.Extent.Count, Is.EqualTo(0));

            Person.LoadExtent("test_person_extent.json");
            Assert.That(Person.Extent.Count, Is.EqualTo(2));
        }

    }
}
