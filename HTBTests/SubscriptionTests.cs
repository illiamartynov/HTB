using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class SubscriptionTests
    {
        private Subscription subscription;
        private IAccessType accessType;

        [SetUp]
        public void Setup()
        {
            Subscription.Extent.Clear();

            accessType = new Free(30); // Используем существующий класс Free
            subscription = new Subscription(
                subscriptionID: 1,
                startDate: DateTime.Now,
                endDate: DateTime.Now.AddMonths(1),
                type: SubscriptionType.Free,
                accessType: accessType
            );
        }

        [Test]
        public void TestSubscriptionCreation()
        {
            Assert.Multiple(() =>
            {
                Assert.That(subscription.SubscriptionID, Is.EqualTo(1));
                Assert.That(subscription.Type, Is.EqualTo(SubscriptionType.Free));
                Assert.That(subscription.AccessType, Is.EqualTo(accessType));
                Assert.That(subscription.StartDate.Date, Is.EqualTo(DateTime.Now.Date));
                Assert.That(subscription.EndDate.Date, Is.EqualTo(DateTime.Now.AddMonths(1).Date));
            });
        }

        [Test]
        public void TestShowSubscriptionInfo()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                subscription.ShowSubscriptionInfo();

                var output = sw.ToString().Trim();

                Assert.Multiple(() =>
                {
                    Assert.That(output, Does.Contain("Subscription ID: 1"));
                    Assert.That(output, Does.Contain("Type: Free"));
                    Assert.That(output, Does.Contain($"Access Details: {accessType.GetAccessDescription()}"));
                    Assert.That(output, Does.Contain($"Duration: {subscription.StartDate} - {subscription.EndDate}"));
                });
            }
        }

        [Test]
        public void TestSaveExtent()
        {
            var filename = "test_subscription_extent.json";
            Subscription.SaveExtent(filename);

            Assert.That(File.Exists(filename), Is.True);

            var content = File.ReadAllText(filename);
            Assert.Multiple(() =>
            {
                Assert.That(content, Does.Contain("\"SubscriptionID\": 1"));
                Assert.That(content, Does.Contain("\"Type\": \"Free\""));
            });

            File.Delete(filename);
        }

        [Test]
        public void TestLoadExtent()
        {
            var filename = "test_subscription_extent.json";
            Subscription.SaveExtent(filename);

            Subscription.Extent.Clear();
            Subscription.LoadExtent(filename);

            Assert.That(Subscription.Extent.Count, Is.EqualTo(1));
            var loadedSubscription = Subscription.Extent[0];

            Assert.Multiple(() =>
            {
                Assert.That(loadedSubscription.SubscriptionID, Is.EqualTo(subscription.SubscriptionID));
                Assert.That(loadedSubscription.Type, Is.EqualTo(subscription.Type));
                Assert.That(loadedSubscription.StartDate, Is.EqualTo(subscription.StartDate));
                Assert.That(loadedSubscription.EndDate, Is.EqualTo(subscription.EndDate));
                Assert.That(loadedSubscription.AccessType.GetAccessDescription(), Is.EqualTo(accessType.GetAccessDescription()));
            });

            File.Delete(filename);
        }
    }
}
