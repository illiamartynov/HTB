using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;
    using System.IO;

    public class AddressTests
    {
        [SetUp]
        public void Setup()
        {
            Address.ClearAddresses();
        }

        [Test]
        public void TestAddressCreation()
        {
            var address = new Address("USA", "New York", "5th Avenue", 101);

            Assert.Multiple(() =>
            {
                Assert.That(address.Country, Is.EqualTo("USA"));
                Assert.That(address.City, Is.EqualTo("New York"));
                Assert.That(address.Street, Is.EqualTo("5th Avenue"));
                Assert.That(address.Number, Is.EqualTo(101));
            });
        }

        [Test]
        public void TestAddressAddedToList()
        {
            new Address("USA", "New York", "5th Avenue", 101);
            new Address("Canada", "Toronto", "Queen St", 22);

            Assert.That(Address.Addresses.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestViewAllAddresses()
        {
            new Address("USA", "New York", "5th Avenue", 101);
            new Address("Canada", "Toronto", "Queen St", 22);

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Address.ViewAllAddresses();
                var result = sw.ToString().Trim();

                Assert.Multiple(() =>
                {
                    Assert.That(result, Does.Contain("5th Avenue 101, New York, USA"));
                    Assert.That(result, Does.Contain("Queen St 22, Toronto, Canada"));
                });
            }
        }
    }
}