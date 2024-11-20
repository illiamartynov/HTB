using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class LessonTests
    {
        private Lesson lesson;

        [SetUp]
        public void Setup()
        {
            Lesson.Extent.Clear();
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
        public void TestStartLesson()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                lesson.StartLesson();

                var output = sw.ToString().Trim();
                Assert.That(output, Is.EqualTo("Lesson Introduction to HTB started."));
            }
        }

        [Test]
        public void TestViewResources()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                lesson.ViewResources();

                var output = sw.ToString().Trim();
                Assert.That(output, Is.EqualTo("Viewing resources for lesson: Introduction to HTB"));
            }
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
