using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class CertificateTests
    {
        private Certificate certificate;
        private Person owner;

        [SetUp]
        public void Setup()
        {
            var address = Address.AddAddress("Test Country", "Test City", "Test Street", 123);
            var rank = new Rank(1, null, new Leaderboard()); // Adjust leaderboard as per context
            var accessType = new Free(30); // Example of IAccessType
            var course = new Course("Intro to Security", "Beginner", accessType);
            var completenessLevel = new CompletenessLevel(50, DateTime.Now, null, course);
            var profile = new Profile(100, "Basic", null); // Adjust person assignment later

            owner = new Person(
                email: "test@example.com",
                name: "John Doe",
                password: "password123",
                registrationDate: DateTime.Now,
                birthDate: new DateTime(1990, 1, 1),
                isActive: true,
                balance: 0,
                address: address,
                rank: rank,
                completenessLevel: completenessLevel,
                profile: profile
            );

            certificate = new Certificate(1, DateTime.Now, owner);
        }

        [Test]
        public void AddOwner_ShouldSetOwnerCorrectly()
        {
            var certificate = new Certificate(1, DateTime.Now, null);

            certificate.AddOwner(owner);

            Assert.AreEqual(owner, certificate.Owner);
        }

        [Test]
        public void RemoveOwner_ShouldClearOwner()
        {
            var certificate = new Certificate(1, DateTime.Now, owner);

            certificate.RemoveOwner(owner);

            Assert.IsNull(certificate.Owner);
        }
    }
}