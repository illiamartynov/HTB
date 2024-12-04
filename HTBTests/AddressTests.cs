using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class AddressTests
    {
        [SetUp]
        public void Setup()
        {
            Address.ClearAddresses();
        }

        [Test]
        public void TestAddressCreation()
        {
            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);

            Assert.Multiple(() =>
            {
                Assert.That(address.Country, Is.EqualTo("USA"));
                Assert.That(address.City, Is.EqualTo("New York"));
                Assert.That(address.Street, Is.EqualTo("5th Avenue"));
                Assert.That(address.Number, Is.EqualTo(101));
            });
        }

        [Test]
        public void TestAddressAddedToList()
        {
            Address.AddAddress("USA", "New York", "5th Avenue", 101);
            Address.AddAddress("Canada", "Toronto", "Queen St", 22);

            Assert.That(Address.Addresses.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestAssignPersonToAddress()
        {
            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101); // Создаем объект Address

            var profile = new Profile(0, "Novice");
            var leaderboard = new Leaderboard();
            var rank = new Rank(1);
            var completenessLevel = new CompletenessLevel(50, DateTime.Now);
            var subscription = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

            var person = Person.AddPerson(
                "person@example.com",
                "TestPerson",
                "password",
                DateTime.Now,
                DateTime.Now.AddYears(-25),
                true,
                100,
                profile,
                leaderboard,
                address, // Передаем корректный объект Address
                rank,
                completenessLevel,
                subscription
            );

            Assert.Multiple(() =>
            {
                Assert.That(person.Address, Is.EqualTo(address));
                Assert.That(address.Persons.Contains(person), Is.True);
            });
        }


        [Test]
        public void TestRemovePersonFromAddress()
        {
            // Создаем адрес
            var address = Address.AddAddress("USA", "New York", "5th Avenue", 101);

            // Создаем профиль и связанные объекты
            var profile = new Profile(0, "Novice");
            var leaderboard = new Leaderboard();
            var rank = new Rank(1);
            var completenessLevel = new CompletenessLevel(50, DateTime.Now);
            var subscription = new Subscription(1, DateTime.Now, DateTime.Now.AddMonths(1), SubscriptionType.Free, new Free(30));

            // Создаем пользователя и связываем его с адресом
            var person = Person.AddPerson(
                "person@example.com",
                "TestPerson",
                "password",
                DateTime.Now,
                DateTime.Now.AddYears(-25),
                true,
                100,
                profile,
                leaderboard,
                address,
                rank,
                completenessLevel,
                subscription
            );

            // Удаляем пользователя из адреса
            address.RemovePerson(person);

            // Проверяем, что связь удалена
            Assert.Multiple(() =>
            {
                Assert.That(address.Persons.Contains(person), Is.False);
                Assert.That(person.Address, Is.Null);
            });
        }

    }
}
