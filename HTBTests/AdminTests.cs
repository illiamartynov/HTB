using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class AdminTests
    {
        private Admin admin;
        private User user;
        private Course course;

        [SetUp]
        public void Setup()
        {
            admin = new Admin("admin@example.com", "AdminUser", "password", DateTime.Now, DateTime.Now.AddYears(-30), true, 1000, 1, new Profile(1000, "Beginner"), new Leaderboard());
            user = new User("user@example.com", "UserTest", "password", DateTime.Now, DateTime.Now.AddYears(-20), true, 500, "user123", new Profile(500, "Intermediate"), new Leaderboard());
            course = new Course("Pentesting 101", "Medium", false);
        }

        [Test]
        public void TestAdminCreateCourse()
        {
            Assert.DoesNotThrow(() => admin.CreateCourse(course));
        }

        [Test]
        public void TestAdminModifyUser()
        {
            Assert.DoesNotThrow(() => admin.ModifyUser(user));
        }

        [Test]
        public void TestAdminViewReports()
        {
            Assert.DoesNotThrow(() => admin.ViewReports());
        }
    }
}