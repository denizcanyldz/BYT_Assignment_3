using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.AssociationTests
{
    [TestFixture]
    public class StaffAndRestaurantTests
    {
        private Restaurant _restaurant;
        private Waiter _waiter;
        private Chef _chef;
        private Bartender _bartender;

        [SetUp]
        public void SetUp()
        {
            _restaurant = new Restaurant(1, "Test Restaurant", "123 Test Street", "123456789", new List<Menu>());
            _waiter = new Waiter(1, "Waiter John");
            _chef = new Chef(2, "Chef Marie", new List<string?> { "Italian Cuisine" });
            _bartender = new Bartender(3, "Bartender Lucy");
        }

        [Test]
        public void AddRestaurant_AssociatesStaffWithRestaurant()
        {
            _waiter.AddRestaurant(_restaurant);
            Assert.AreEqual(_restaurant, _waiter.GetRestaurant());
            Assert.That(_restaurant.GetStaff().Contains(_waiter));
        }

        [Test]
        public void AddRestaurant_DoesNotReAddExistingRestaurant()
        {
            _waiter.AddRestaurant(_restaurant);
            _waiter.AddRestaurant(_restaurant);
            Assert.AreEqual(1, _restaurant.GetStaff().Count);
        }

        [Test]
        public void AddRestaurant_ThrowsException_WhenRestaurantIsNull()
        {
            Assert.Throws<ArgumentException>(() => _waiter.AddRestaurant(null));
        }

        [Test]
        public void RemoveRest_RemovesStaffFromRestaurant()
        {
            _waiter.AddRestaurant(_restaurant);
            _waiter.RemoveRest(_restaurant);
            Assert.IsNull(_waiter.GetRestaurant());
            Assert.IsFalse(_restaurant.GetStaff().Contains(_waiter));
        }

        [Test]
        public void RemoveRest_ThrowsException_WhenRestaurantIsNotAssociated()
        {
            var anotherRestaurant = new Restaurant(2, "Another Restaurant", "456 Another Street", "987654321", new List<Menu>());
            Assert.Throws<KeyNotFoundException>(() => _waiter.RemoveRest(anotherRestaurant));
        }

        [Test]
        public void ModifyRestaurant_ChangesAssociatedRestaurant()
        {
            var newRestaurant = new Restaurant(3, "New Restaurant", "789 New Street", "112233445", new List<Menu>());
            _chef.AddRestaurant(_restaurant);
            _chef.ModifyRestaurant(newRestaurant);

            Assert.AreEqual(newRestaurant, _chef.GetRestaurant());
            Assert.IsFalse(_restaurant.GetStaff().Contains(_chef));
            Assert.IsTrue(newRestaurant.GetStaff().Contains(_chef));
        }

        [Test]
        public void ModifyRestaurant_ThrowsException_WhenNewRestaurantIsNull()
        {
            Assert.Throws<ArgumentException>(() => _chef.ModifyRestaurant(null));
        }

        [Test]
        public void AddStaff_AssociatesStaffWithRestaurant()
        {
            _restaurant.AddStaff(_bartender);
            Assert.That(_restaurant.GetStaff().Contains(_bartender));
            Assert.AreEqual(_restaurant, _bartender.GetRestaurant());
        }

        [Test]
        public void AddStaff_DoesNotReAddExistingStaff()
        {
            _restaurant.AddStaff(_bartender);
            _restaurant.AddStaff(_bartender);
            Assert.AreEqual(1, _restaurant.GetStaff().Count);
        }

        [Test]
        public void AddStaff_ThrowsException_WhenStaffIsNull()
        {
            Assert.Throws<ArgumentException>(() => _restaurant.AddStaff(null));
        }

        [Test]
        public void RemoveStaff_RemovesAssociationWithRestaurant()
        {
            _restaurant.AddStaff(_chef);
            _restaurant.RemoveStaff(_chef);

            Assert.IsFalse(_restaurant.GetStaff().Contains(_chef));
            Assert.IsNull(_chef.GetRestaurant());
        }

        [Test]
        public void RemoveStaff_ThrowsException_WhenStaffNotAssociated()
        {
            Assert.Throws<KeyNotFoundException>(() => _restaurant.RemoveStaff(_waiter));
        }

        [Test]
        public void ModifyStaff_ReplacesOldStaffWithNew()
        {
            var newWaiter = new Waiter(4, "Waiter Tom");
            _restaurant.AddStaff(_waiter);
            _restaurant.ModifyStaff(_waiter, newWaiter);

            Assert.IsFalse(_restaurant.GetStaff().Contains(_waiter));
            Assert.IsTrue(_restaurant.GetStaff().Contains(newWaiter));
        }

        [Test]
        public void ModifyStaff_ThrowsException_WhenOldStaffNotAssociated()
        {
            var newWaiter = new Waiter(4, "Waiter Tom");
            Assert.Throws<InvalidOperationException>(() => _restaurant.ModifyStaff(_waiter, newWaiter));
        }
    }
}
