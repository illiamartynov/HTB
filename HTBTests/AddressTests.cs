using HTB;
using NUnit.Framework;
using System;

namespace HTBTests
{
    public class AddressTests
    {
        private Person _person;
        private Address _address;
        
        [SetUp]
        public void Setup()
        {
            Address.ClearAddresses();
        }

        [Test]
        public void AddAddress_ShouldAddAddressToPersonAndAddress()
        {
            // Act
            _person.AddAddress(_address);

            // Assert
            Assert.AreEqual(_address, _person.Address);
            Assert.AreEqual(_person, _address._person);
        }

        [Test]
        public void RemoveAddress_ShouldRemoveAddressFromPersonAndAddress()
        {
            // Arrange
            _person.AddAddress(_address);

            // Act
            _person.RemoveAddress(_address);

            // Assert
            Assert.IsNull(_person.Address);
            Assert.IsNull(_address._person);
        }

        [Test]
        public void UpdateAddress_ShouldUpdateBidirectionalReferences()
        {
            // Arrange
            var newAddress = Address.AddAddress("Canada", "Toronto", "King Street", 10);
            _person.AddAddress(_address);

            // Act
            _person.UpdateAddress(_address, newAddress);

            // Assert
            Assert.AreEqual(newAddress, _person.Address);
            Assert.AreEqual(_person, newAddress._person);
            Assert.IsNull(_address._person);
        }
    }
}
