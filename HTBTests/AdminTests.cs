using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

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
            course = new Course("Pentesting 101");
        }

        [Test]
        public void TestAdminCreateCourse()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); 
                Assert.DoesNotThrow(() => admin.CreateCourse(course));

                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo($"Admin {admin.Name} created course: {course.CourseName}"));
            }
        }

        [Test]
        public void TestAdminModifyUser()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); 
                Assert.DoesNotThrow(() => admin.ModifyUser(user));

                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo($"Admin {admin.Name} modified user {user.Name}"));
            }
        }

        [Test]
        public void TestAdminViewReports()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Assert.DoesNotThrow(() => admin.ViewReports());

                var result = sw.ToString().Trim();
                Assert.That(result, Is.EqualTo($"Admin {admin.Name} viewed reports."));
            }
        }
    }
}