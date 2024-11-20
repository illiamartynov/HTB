using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class CompletenessLevelTests
    {
        private CompletenessLevel completenessLevel;

        [SetUp]
        public void Setup()
        {
            completenessLevel = new CompletenessLevel(50, DateTime.Now);
        }

        [Test]
        public void TestCompletenessLevelCreation()
        {
            var startDate = DateTime.Now;
            var completeness = new CompletenessLevel(80, startDate);

            Assert.Multiple(() =>
            {
                Assert.That(completeness.CompletenessPercentage, Is.EqualTo(80));
                Assert.That(completeness.StartDate, Is.EqualTo(startDate));
            });
        }

        [Test]
        public void TestInvalidCompletenessLevelCreation()
        {
            Assert.Throws<ArgumentException>(() => new CompletenessLevel(-10, DateTime.Now));
            Assert.Throws<ArgumentException>(() => new CompletenessLevel(110, DateTime.Now));
        }

        [Test]
        public void TestUpdateCompleteness_ValidValue()
        {
            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                completenessLevel.UpdateCompleteness(70);
                var output = sw.ToString().Trim();

                Assert.Multiple(() =>
                {
                    Assert.That(completenessLevel.CompletenessPercentage, Is.EqualTo(70));
                    Assert.That(output, Does.Contain("Completeness updated to 70%"));
                });
            }
        }

        [Test]
        public void TestUpdateCompleteness_InvalidValue()
        {
            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                completenessLevel.UpdateCompleteness(150);
                completenessLevel.UpdateCompleteness(-20);

                var output = sw.ToString().Trim();

                Assert.Multiple(() =>
                {
                    Assert.That(completenessLevel.CompletenessPercentage, Is.EqualTo(50)); 
                    Assert.That(output, Does.Contain("Invalid percentage value. Must be between 0 and 100."));
                });
            }
        }


        [Test]
        public void TestCompletenessPercentage_SetValidValue()
        {
            completenessLevel.CompletenessPercentage = 85;

            Assert.That(completenessLevel.CompletenessPercentage, Is.EqualTo(85));
        }

        [Test]
        public void TestCompletenessPercentage_SetInvalidValue()
        {
            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                completenessLevel.CompletenessPercentage = 150;
                completenessLevel.CompletenessPercentage = -30;

                var output = sw.ToString().Trim();

                Assert.That(completenessLevel.CompletenessPercentage, Is.EqualTo(50));
                Assert.That(output, Does.Contain("Invalid percentage value. Must be between 0 and 100."));
            }
        }

        [Test]
        public void TestStartDate_SetValue()
        {
            var newDate = DateTime.Now.AddDays(-10);
            completenessLevel.StartDate = newDate;

            Assert.That(completenessLevel.StartDate, Is.EqualTo(newDate));
        }
    }
}
