using HTB;

namespace HTBTests
{
    using NUnit.Framework;
    using System;

    public class AddressTests
    {
        [SetUp]
        public void Setup()
        {
            Address.Addresses.Clear();  
        }

        [Test]
        public void TestAddressCreation()
        {
            var address = new Address("USA", "New York", "5th Avenue", 101);

            Assert.That(address.Country, Is.EqualTo("USA"));
            Assert.That(address.City, Is.EqualTo("New York"));
            Assert.That(address.Street, Is.EqualTo("5th Avenue"));
            Assert.That(address.Number, Is.EqualTo(101));
        }

        [Test]
        public void TestAddressAddedToList()
        {
            var address1 = new Address("USA", "New York", "5th Avenue", 101);
            var address2 = new Address("Canada", "Toronto", "Queen St", 22);

            Assert.That(Address.Addresses.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestViewAllAddresses()
        {
            var address1 = new Address("USA", "New York", "5th Avenue", 101);
            var address2 = new Address("Canada", "Toronto", "Queen St", 22);

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);
                Address.ViewAllAddresses();
                var result = sw.ToString().Trim();

                Assert.That(result, Does.Contain("5th Avenue 101, New York, USA"));
                Assert.That(result, Does.Contain("Queen St 22, Toronto, Canada"));
            }
        }
    }
}