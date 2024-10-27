using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class ProfileTests
    {
        private Profile profile;

        [SetUp]
        public void Setup()
        {
            Profile.Extent.Clear();
            profile = new Profile(1000, "intermediate");
        }

        [Test]
        public void TestProfileCreation()
        {
            Assert.That(profile.Points, Is.EqualTo(1000));
            Assert.That(profile.AcademyLevel, Is.EqualTo("intermediate"));
        }

        [Test]
        public void TestUpdateProfile()
        {
            profile.UpdateProfile(1500, "advanced");
            Assert.That(profile.Points, Is.EqualTo(1500));
            Assert.That(profile.AcademyLevel, Is.EqualTo("advanced"));
        }

        [Test]
        public void TestExtentSerialization()
        {
            var profile2 = new Profile(500, "Beginner");

            Profile.SaveExtent("test_profile_extent.json");
            Profile.LoadExtent("test_profile_extent.json");

            Assert.That(Profile.Extent.Count, Is.EqualTo(2));
        }
    }
}