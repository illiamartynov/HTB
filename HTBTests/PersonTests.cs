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

            person = Person.AddPerson(
                email: "test@example.com",
                name: "John Doe",
                password: "password123",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-30),
                isActive: true,
                balance: 1000,
                profile: profile,
                address: address
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
                Assert.That(person.Address, Is.EqualTo(address));
                Assert.That(person.CompletenessLevel, Is.EqualTo(completenessLevel));
            });
        }

        [Test]
        public void AddCertificate_ShouldAddCertificateToPerson()
        {
            var certificate = new Certificate(1, DateTime.Now, person);

            person.AddCertificate(certificate);

            Assert.Contains(certificate, person.Certificates.ToList());
        }
        
        [Test]
        public void RemoveCertificate_ShouldRemoveCertificateFromPerson()
        {
            var certificate = new Certificate(1, DateTime.Now, person);
            person.AddCertificate(certificate);

            person.RemoveCertificate(certificate);

            Assert.IsFalse(person.Certificates.Contains(certificate));
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
    }
}
