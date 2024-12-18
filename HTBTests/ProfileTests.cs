using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class ProfileTests
    {
        private Profile profile;
        private Person person;

        [SetUp]
        public void Setup()
        {
            Profile.Extent.Clear();

            person = Person.AddPerson(
                email: "test@example.com",
                name: "John Doe",
                password: "password123",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-30),
                isActive: true,
                balance: 1000,
                profile: null,
                address: null,
                rank: null,
                completenessLevel: null,
                subscription: null
            );

            profile = new Profile(500, "Intermediate", person);
        }

        [Test]
        public void TestProfileCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(profile.Points, Is.EqualTo(500));
                Assert.That(profile.AcademyLevel, Is.EqualTo("Intermediate"));
                Assert.That(profile.Person, Is.EqualTo(person));
            });
        }

        [Test]
        public void TestUpdateProfile()
        {
            profile.UpdateProfile(1000, "Advanced");

            Assert.Multiple(() =>
            {
                Assert.That(profile.Points, Is.EqualTo(1000));
                Assert.That(profile.AcademyLevel, Is.EqualTo("Advanced"));
            });
        }

        [Test]
        public void TestAssignPerson()
        {
            var newPerson = Person.AddPerson(
                email: "new@example.com",
                name: "Jane Doe",
                password: "password456",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-25),
                isActive: true,
                balance: 2000,
                profile: null,
                address: null,
                rank: null,
                completenessLevel: null,
                subscription: null
            );

            profile.AssignPerson(newPerson);

            Assert.Multiple(() =>
            {
                Assert.That(profile.Person, Is.EqualTo(newPerson));
                Assert.That(newPerson.Profile, Is.EqualTo(profile));
            });
        }

        [Test]
        public void TestUnassignPerson()
        {
            profile.UnassignPerson();

            Assert.That(profile.Person, Is.Null);
        }

        [Test]
        public void TestSaveExtent()
        {
            var filename = "test_profile_extent.json";
            Profile.SaveExtent(filename);

            Assert.That(File.Exists(filename), Is.True);

            var content = File.ReadAllText(filename);
            Assert.That(content, Does.Contain("\"Points\":500"));
            Assert.That(content, Does.Contain("\"AcademyLevel\":\"Intermediate\""));

            File.Delete(filename);
        }

        [Test]
        public void TestLoadExtent()
        {
            var filename = "test_profile_extent.json";
            Profile.SaveExtent(filename);

            Profile.Extent.Clear();
            Profile.LoadExtent(filename);

            Assert.That(Profile.Extent.Count, Is.EqualTo(1));
            var loadedProfile = Profile.Extent[0];

            Assert.Multiple(() =>
            {
                Assert.That(loadedProfile.Points, Is.EqualTo(profile.Points));
                Assert.That(loadedProfile.AcademyLevel, Is.EqualTo(profile.AcademyLevel));
            });

            File.Delete(filename);
        }
    }
}
