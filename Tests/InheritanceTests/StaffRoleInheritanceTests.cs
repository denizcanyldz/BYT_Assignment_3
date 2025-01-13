using NUnit.Framework;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;

namespace Tests.InheritanceTests
{
    [TestFixture]
    public class StaffRoleInheritanceTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset all specialized extents (so each test starts fresh)
            Bartender.SetAll(new List<Bartender>());
            Chef.SetAll(new List<Chef>());
            Waiter.SetAll(new List<Waiter>());
            WaiterBartender.SetAll(new List<WaiterBartender>());
            Manager.SetAll(new List<Manager>());
            
            // reset the base Staff extent
            Staff.SetAll(new List<Staff>());
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up again after each test
            Bartender.SetAll(new List<Bartender>());
            Chef.SetAll(new List<Chef>());
            Waiter.SetAll(new List<Waiter>());
            WaiterBartender.SetAll(new List<WaiterBartender>());
            Manager.SetAll(new List<Manager>());
            Staff.SetAll(new List<Staff>());
        }

        [Test]
        public void WaiterBartender_InheritsBartender_And_ImplementsIWaiter()
        {
            // Arrange & Act
            var wb = new WaiterBartender(staffID: 100, name: "MultiRoleStaff", bonus: 500);

            // Assert
            Assert.That(wb, Is.InstanceOf<Bartender>(), 
                "WaiterBartender should inherit from Bartender.");
            Assert.That(wb, Is.InstanceOf<Staff>(), 
                "WaiterBartender should inherit from the base Staff class as well.");

            // The WaiterBartender must also implement IWaiter
            Assert.That(wb, Is.InstanceOf<IWaiter>(), 
                "WaiterBartender should implement IWaiter interface.");

            // Confirm that the role is set as WaiterBartender
            Assert.That(wb.CurrentRole, Is.EqualTo(StaffRole.WaiterBartender), 
                "CurrentRole should be WaiterBartender for WaiterBartender instances.");

            // Also confirm we can call Bartender methods:
            Assert.DoesNotThrow(() => wb.MixDrink(), 
                "WaiterBartender should be able to use Bartender method MixDrink() without error.");

            // Cast to IWaiter and test Waiter-like methods:
            var asWaiter = (IWaiter)wb;
            Assert.DoesNotThrow(() => asWaiter.TakeOrder(), 
                "WaiterBartender should be able to call IWaiter.TakeOrder() without throwing.");
            Assert.DoesNotThrow(() => asWaiter.ServeOrder(), 
                "WaiterBartender should be able to call IWaiter.ServeOrder() without throwing.");
            Assert.DoesNotThrow(() => asWaiter.ProcessPayment(),
                "WaiterBartender should be able to call IWaiter.ProcessPayment() without throwing.");
        }

        [Test]
        public void Staff_SwitchRole_DisjointTest()
        {
            // Arrange
            var waiter = new Waiter(staffID: 10, name: "Alice", initialTips: 50);
            var bartender = new Bartender(staffID: 20, name: "Bob");

            // Act
            // Suppose we want to switch "waiter" to a new "bartender" role:
            var newBartenderRole = new Bartender(staffID: 10, name: "Alice-NowBartender");

            // Copy relevant data from Waiter -> newBartenderRole by calling SwitchRole:
            waiter.SwitchRole(newBartenderRole);

            

            // Assert
            Assert.That(newBartenderRole.Name, Is.EqualTo("Alice"), 
                "Name should be carried over to the new role.");
            Assert.That(newBartenderRole.ContactNumber, Is.EqualTo(waiter.ContactNumber),
                "ContactNumber should be copied to the new role if it existed.");

            // newBartenderRole's staffID matches the old one:
            Assert.That(newBartenderRole.StaffID, Is.EqualTo(10),
                "StaffID should remain consistent during the switch.");

            // Confirm newBartenderRole is indeed recognized as a Bartender
            Assert.That(newBartenderRole.CurrentRole, Is.EqualTo(StaffRole.Bartender),
                "The new role should be recognized as Bartender.");

           
        }

        [Test]
        public void Staff_CannotBeTwoRolesAtOnce_SimpleCheck()
        {
            

            var waiter = new Waiter(staffID: 33, name: "John the Waiter");


            Assert.Throws<InvalidCastException>(() =>
            {
                // Force an invalid cast
                var forcedChef = (Chef)(object)waiter;
            }, 
            "A Staff object that is specifically a Waiter cannot be cast to Chef if we truly have disjoint roles.");

            // If the user wants to *become* a Chef, they'd have to do SwitchRole() 
            // to a new Chef instance instead, not cast the same object to Chef.
        }

        [Test]
        public void WaiterBartender_ShouldStoreTips_FromIWaiterInterface()
        {
            // Arrange
            var wb = new WaiterBartender(staffID: 202, name: "Hybrid Staff");

            // Act
            var iwaiter = (IWaiter)wb;
            iwaiter.TipsCollected = 123.45;

            // Assert
            Assert.That(() => iwaiter.TipsCollected, Is.EqualTo(123.45),
                "WaiterBartender should store tips properly via IWaiter interface property.");

            // Also confirm it doesn't overlap with the Bartender's extents or fields incorrectly
            Assert.That(WaiterBartender.GetAll().Count, Is.EqualTo(1),
                "There should be exactly one WaiterBartender in the extent.");
            Assert.That(wb.StaffID, Is.EqualTo(202),
                "StaffID was set correctly on WaiterBartender.");
        }
    }
}
