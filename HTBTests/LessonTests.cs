using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class LessonTests
    {
        private Lesson lesson;
        private Course course;

        [SetUp]
        public void Setup()
        {
            Lesson.Extent.Clear();

            course = new Course("OSINT Basics", "Beginner", new Free(30));
            lesson = new Lesson("Introduction to HTB", "This is an introductory lesson about Hack The Box.");
        }

        [Test]
        public void TestLessonCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(lesson.LessonTitle, Is.EqualTo("Introduction to HTB"));
                Assert.That(lesson.Content, Is.EqualTo("This is an introductory lesson about Hack The Box."));
            });
        }

        [Test]
        public void TestLessonAddedToExtent()
        {
            Assert.That(Lesson.Extent.Count, Is.EqualTo(1));
            Assert.That(Lesson.Extent.Contains(lesson), Is.True);
        }

        [Test]
        public void TestAssignToCourse()
        {
            lesson.AssignToCourse(course);

            Assert.Multiple(() =>
            {
                Assert.That(lesson.Course, Is.EqualTo(course));
                Assert.That(course.Lessons, Contains.Item(lesson));
            });
        }

        [Test]
        public void TestUnassignFromCourse()
        {
            lesson.AssignToCourse(course);
            lesson.UnassignFromCourse();

            Assert.Multiple(() =>
            {
                Assert.That(lesson.Course, Is.Null);
                Assert.That(course.Lessons, Does.Not.Contain(lesson));
            });
        }

        [Test]
        public void TestAddResource()
        {
            var resource = new Resource("Resource 1", "Video", "https://example.com/resource1", lesson);

            Assert.That(lesson.Resources, Contains.Item(resource));
        }

        [Test]
        public void TestRemoveResource()
        {
            var resource = new Resource("Resource 1", "Video", "https://example.com/resource1", lesson);
            lesson.RemoveResource(resource);

            Assert.That(lesson.Resources, Does.Not.Contain(resource));
        }

        [Test]
        public void TestDeleteLesson()
        {
            lesson.AssignToCourse(course);
            var resource = new Resource("Resource 1", "Video", "https://example.com/resource1", lesson);

            Lesson.DeleteLesson(lesson);

            Assert.Multiple(() =>
            {
                Assert.That(Lesson.Extent, Does.Not.Contain(lesson));
                Assert.That(course.Lessons, Does.Not.Contain(lesson));
                Assert.That(lesson.Resources, Is.Empty);
            });
        }

        [Test]
        public void TestSaveExtent()
        {
            var filename = "test_lesson_extent.json";
            Lesson.SaveExtent(filename);

            Assert.That(File.Exists(filename), Is.True);

            var content = File.ReadAllText(filename);
            Assert.That(content, Does.Contain("\"LessonTitle\":\"Introduction to HTB\""));
            Assert.That(content, Does.Contain("\"Content\":\"This is an introductory lesson about Hack The Box.\""));

            File.Delete(filename);
        }

        [Test]
        public void TestLoadExtent()
        {
            var filename = "test_lesson_extent.json";
            Lesson.SaveExtent(filename);

            Lesson.Extent.Clear();
            Lesson.LoadExtent(filename);

            Assert.That(Lesson.Extent.Count, Is.EqualTo(1));
            var loadedLesson = Lesson.Extent[0];

            Assert.Multiple(() =>
            {
                Assert.That(loadedLesson.LessonTitle, Is.EqualTo(lesson.LessonTitle));
                Assert.That(loadedLesson.Content, Is.EqualTo(lesson.Content));
            });

            File.Delete(filename);
        }
    }
}
