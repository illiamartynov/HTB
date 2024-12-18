using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PersonTests
    {
        private Person person;
        private Address address;
        private Profile profile;
        private Rank rank;
        private CompletenessLevel completenessLevel;

        [SetUp]
        public void Setup()
        {
            Person.ClearExtent();

            address = Address.AddAddress("USA", "New York", "5th Avenue", 101);
            profile = new Profile(500, "Intermediate", null);
            rank = new Rank(1, null, new Leaderboard());
            completenessLevel = new CompletenessLevel(80, DateTime.Now, null, new Course("OSINT Basics", "Beginner", new Free(30)));

            person = Person.AddPerson(
                email: "test@example.com",
                name: "John Doe",
                password: "password123",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-30),
                isActive: true,
                balance: 1000,
                profile: profile,
                address: address,
                rank: rank,
                completenessLevel: completenessLevel,
                subscription: null
            );
        }

        [Test]
        public void TestPersonCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(person.Email, Is.EqualTo("test@example.com"));
                Assert.That(person.Name, Is.EqualTo("John Doe"));
                Assert.That(person.IsActive, Is.True);
                Assert.That(person.Balance, Is.EqualTo(1000));
                Assert.That(person.Profile, Is.EqualTo(profile));
                Assert.That(person.Address, Is.EqualTo(address));
                Assert.That(person.Rank, Is.EqualTo(rank));
                Assert.That(person.CompletenessLevel, Is.EqualTo(completenessLevel));
            });
        }

        [Test]
        public void TestAddCertificate()
        {
            var certificate = Certificate.Create(1, DateTime.Now, person);
            person.AddCertificate(certificate);

            Assert.That(person.Certificates, Contains.Item(certificate));
        }

        [Test]
        public void TestRemoveCertificate()
        {
            var certificate = Certificate.Create(1, DateTime.Now, person);
            person.AddCertificate(certificate);
            person.RemoveCertificate(certificate);

            Assert.That(person.Certificates, Does.Not.Contain(certificate));
        }

        [Test]
        public void TestAddPayment()
        {
            var payment = Payment.Create(1, 100.0f, DateTime.Now, "credit card", person, "USD");
            person.AddPayment(payment);

            Assert.That(person.Payments, Contains.Item(payment));
        }

        [Test]
        public void TestRemovePayment()
        {
            var payment = Payment.Create(1, 100.0f, DateTime.Now, "credit card", person, "USD");
            person.AddPayment(payment);
            person.RemovePayment(payment);

            Assert.That(person.Payments, Does.Not.Contain(payment));
        }

        [Test]
        public void TestUpdatePersonDetails()
        {
            person.UpdatePerson(email: "new@example.com", name: "Jane Doe", balance: 2000);

            Assert.Multiple(() =>
            {
                Assert.That(person.Email, Is.EqualTo("new@example.com"));
                Assert.That(person.Name, Is.EqualTo("Jane Doe"));
                Assert.That(person.Balance, Is.EqualTo(2000));
            });
        }

        [Test]
        public void TestDeletePerson()
        {
            Person.DeletePerson(person);

            Assert.That(Person.Extent, Does.Not.Contain(person));
        }

        [Test]
        public void TestAddReferral()
        {
            var referredPerson = Person.AddPerson(
                email: "referral@example.com",
                name: "Referrer",
                password: "password123",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-25),
                isActive: true,
                balance: 500,
                profile: profile,
                address: address,
                rank: rank,
                completenessLevel: completenessLevel,
                subscription: null
            );

            person.AddReferral(referredPerson);

            Assert.Multiple(() =>
            {
                Assert.That(person.Referrals, Contains.Item(referredPerson));
                Assert.That(referredPerson.ReferredBy, Is.EqualTo(person));
            });
        }
    }
}
