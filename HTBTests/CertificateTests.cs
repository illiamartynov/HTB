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

            certificate = Certificate.Create(1, DateTime.Now, owner);
        }

        [Test]
        public void TestCreateCertificate()
        {
            Assert.DoesNotThrow(() => Certificate.Create(1, DateTime.Now, owner));
        }

        [Test]
        public void TestCreateCertificateThrowsExceptionIfOwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Certificate.Create(1, DateTime.Now, null));
        }

        [Test]
        public void TestViewCertificate()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                certificate.ViewCertificate();

                var expectedOutput = $"Certificate ID: {certificate.CertificateId}, Issue Date: {certificate.IssueDate}, Owner: {owner.Name}";
                var actualOutput = sw.ToString().Trim();

                Assert.That(actualOutput, Is.EqualTo(expectedOutput));
            }
        }

        [Test]
        public void TestUnassignOwner()
        {
            certificate.UnassignOwner();
            Assert.IsNull(certificate.Owner);
        }

        [Test]
        public void TestAssignOwner()
        {
            var newAddress = Address.AddAddress("New Country", "New City", "New Street", 456);
            var newRank = new Rank(2, null, new Leaderboard()); // Adjust leaderboard as per context
            var newAccessType = new Paid(100); // Example of IAccessType
            var newCourse = new Course("Advanced Security", "Intermediate", newAccessType);
            var newCompletenessLevel = new CompletenessLevel(80, DateTime.Now, null, newCourse);
            var newProfile = new Profile(200, "Advanced", null); // Adjust person assignment later

            var newOwner = new Person(
                email: "newowner@example.com",
                name: "Jane Doe",
                password: "newpassword123",
                registrationDate: DateTime.Now,
                birthDate: new DateTime(1992, 2, 2),
                isActive: true,
                balance: 0,
                address: newAddress,
                rank: newRank,
                completenessLevel: newCompletenessLevel,
                profile: newProfile
            );

            certificate.AssignOwner(newOwner);

            Assert.IsNotNull(certificate.Owner);
            Assert.That(certificate.Owner.Name, Is.EqualTo("Jane Doe"));
        }

        [Test]
        public void TestAssignOwnerThrowsExceptionIfOwnerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => certificate.AssignOwner(null));
        }
    }
}
