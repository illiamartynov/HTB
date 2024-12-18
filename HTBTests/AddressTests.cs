using HTB;
using NUnit.Framework;
using System;

namespace HTBTests
{
    public class AddressTests
    {
        [SetUp]
        public void Setup()
        {
            Address.ClearAddresses();
        }

        [Test]
        public void AssignPersonToAddress_ShouldSetBidirectionalAssociation()
        {
            // Arrange
            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);
            var leaderboard = new Leaderboard();
            var rank = new Rank(1, null, leaderboard);
            var completenessLevel = new CompletenessLevel(50, DateTime.Now, null, null);
            var subscription = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

            // Создаем Person без Profile
            var person = Person.AddPerson(
                "person@example.com",
                "TestPerson",
                "password",
                DateTime.Now,
                DateTime.Now.AddYears(-25),
                true,
                100,
                null, // Profile будет присвоен позже
                address,
                rank,
                completenessLevel,
                subscription
            );

            // Создаем профиль и присваиваем его человеку
            var profile = new Profile(0, "Novice", person);
            person.AssignProfile(profile);

            // Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(person.Address, Is.EqualTo(address));
                Assert.That(person.Profile, Is.EqualTo(profile));
                Assert.That(address.Persons.Contains(person), Is.True);
            });
        }

        [Test]
        public void RemovePersonFromAddress_ShouldRemoveBidirectionalAssociation()
        {
            // Arrange
            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);
            var leaderboard = new Leaderboard();
            var rank = new Rank(1, null, leaderboard);
            var completenessLevel = new CompletenessLevel(50, DateTime.Now, null, null);
            var subscription = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

            var person = Person.AddPerson(
                "person@example.com",
                "TestPerson",
                "password",
                DateTime.Now,
                DateTime.Now.AddYears(-25),
                true,
                100,
                null, // Profile будет назначен позже
                address,
                rank,
                completenessLevel,
                subscription
            );

            var profile = new Profile(0, "Novice", person);
            person.AssignProfile(profile);

            // Act
            address.RemovePerson(person);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(address.Persons.Contains(person), Is.False);
                Assert.That(person.Address, Is.Null);
                Assert.That(person.Profile, Is.EqualTo(profile)); // Profile не меняется
            });
        }
    }
}
