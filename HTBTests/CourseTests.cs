using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class CourseTests
    {
        private Course course;
        private Person person;

        [SetUp]
        public void Setup()
        {
            Course.ClearExtent();
            Person.ClearExtent();

            var accessType = new Free(30);
            course = new Course("Nmap Basics", "Intermediate", accessType);

            var address = Address.AddAddress("USA", "New York", "5th Avenue", 10);
            var profile = new Profile(500, "Intermediate", null);
            var rank = new Rank(1, null, new Leaderboard());
            var completenessLevel = new CompletenessLevel(80, DateTime.Now, null, course);

            person = Person.AddPerson(
                email: "user@example.com",
                name: "UserTest",
                password: "password",
                registrationDate: DateTime.Now,
                birthDate: DateTime.Now.AddYears(-20),
                isActive: true,
                balance: 500,
                profile: profile,
                address: address,
                rank: rank,
                completenessLevel: completenessLevel,
                subscription: null
            );
        }

        [Test]
        public void TestCourseCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(course.CourseName, Is.EqualTo("Nmap Basics"));
                Assert.That(course.DifficultyLevel, Is.EqualTo("Intermediate"));
                Assert.That(course.AccessType.GetAccessDescription(), Is.EqualTo("Free access for 30 days."));
            });
        }

        [Test]
        public void TestAddLessonToCourse()
        {
            var lesson = new Lesson("Port Scanning Basics", "Learn how to scan ports using Nmap");
            course.AddLesson(lesson);

            Assert.Multiple(() =>
            {
                Assert.That(course.Lessons, Contains.Item(lesson));
                Assert.That(lesson.Course, Is.EqualTo(course));
            });
        }

        [Test]
        public void TestRemoveLessonFromCourse()
        {
            var lesson = new Lesson("Port Scanning Basics", "Learn how to scan ports using Nmap");
            course.AddLesson(lesson);
            course.RemoveLesson(lesson);

            Assert.Multiple(() =>
            {
                Assert.That(course.Lessons, Does.Not.Contain(lesson));
                Assert.That(lesson.Course, Is.Null);
            });
        }

        [Test]
        public void TestClearLessonsFromCourse()
        {
            var lesson1 = new Lesson("Port Scanning Basics", "Learn how to scan ports using Nmap");
            var lesson2 = new Lesson("Service Detection", "Understand how to identify running services");
            course.AddLesson(lesson1);
            course.AddLesson(lesson2);

            course.ClearLessons();

            Assert.That(course.Lessons, Is.Empty);
        }

        [Test]
        public void TestDeleteCourse()
        {
            Course.DeleteCourse(course);

            Assert.That(Course.Extent, Does.Not.Contain(course));
        }
    }
}
