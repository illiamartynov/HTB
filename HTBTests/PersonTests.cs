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

            
            person = new Person(
                email: "person@example.com", 
                name: "Volodymyr Zelensky", 
                password: "password", 
                registrationDate: DateTime.Now, 
                birthDate: DateTime.Now.AddYears(-25), 
                isActive: true, 
                balance: 100, 
                profile: new Profile(500, "intermediate"), 
                leaderboard: new Leaderboard(),
                address: new Address("Ukraine", "Kyiv", "Main Street", 1),
                rank: new Rank(3),
                completenessLevel: new CompletenessLevel(85, DateTime.Now),
                subscription: new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Premium, new Paid(100))
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
                Assert.That(person.UserProfile.AcademyLevel, Is.EqualTo("intermediate"));
            });
        }

        [Test]
        public void TestExtentSerialization()
        {
            
            var person2 = new Person(
                email: "person2@example.com", 
                name: "Test User 2", 
                password: "password", 
                registrationDate: DateTime.Now, 
                birthDate: DateTime.Now.AddYears(-30), 
                isActive: true, 
                balance: 150, 
                profile: new Profile(600, "advanced"), 
                leaderboard: new Leaderboard(),
                address: new Address("Canada", "Toronto", "Queen St", 10),
                rank: new Rank(4),
                completenessLevel: new CompletenessLevel(70, DateTime.Now.AddMonths(-1)),
                subscription: new Subscription(2, DateTime.Now, DateTime.Now.AddMonths(2), SubscriptionType.Free, new Free(30))
            );

            
            Person.SaveExtent("test_person_extent.json");

            
            Person.ClearExtent();
            Assert.That(Person.Extent.Count, Is.EqualTo(0));  

            
            Person.LoadExtent("test_person_extent.json");
            Assert.That(Person.Extent.Count, Is.EqualTo(2));
        }
    }
}
