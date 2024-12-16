using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class RestaurantStaffTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset the static extents before each test 
            // so we start clean every time.
            Restaurant.SetAll(new List<Restaurant>());
            Staff.SetAll(new List<Staff>());
        }

        [Test]
        public void AddStaff_AssociationIsLinkedBothWays()
        {
            // Arrange
            var restaurant = new Restaurant(
                restaurantId: 1,
                name: "Testaurant",
                address: "123 Food Street",
                contactNumber: "555-1234",
                menus: new List<Menu>()
            );

            // Create a Staff subclass instance (e.g., a Waiter or just a test Staff)
            var someStaff = new MockStaff(101, "Alice", "111-2222");

            // Act
            restaurant.AddStaff(someStaff);

            // Assert
            Assert.That(restaurant.GetStaff(), Contains.Item(someStaff),
                "Restaurant should contain the newly added staff.");
            Assert.That(someStaff.Restaurant, Is.EqualTo(restaurant),
                "Staff object should reference the correct Restaurant.");
        }

        [Test]
        public void RemoveStaff_AssociationIsUnlinkedBothWays()
        {
            // Arrange
            var restaurant = new Restaurant(
                restaurantId: 2,
                name: "Some Diner",
                address: "456 Another St",
                contactNumber: "555-5678",
                menus: new List<Menu>()
            );
            var staffToRemove = new MockStaff(202, "Bob", "222-3333");

            // Initialize association
            restaurant.AddStaff(staffToRemove);

            // Act
            restaurant.RemoveStaff(staffToRemove);

            // Assert
            Assert.That(restaurant.GetStaff(), Does.Not.Contain(staffToRemove),
                "Staff should no longer be in the Restaurant's list after removal.");
            Assert.That(staffToRemove.Restaurant, Is.Null,
                "Staff's Restaurant reference should be null after removal.");
        }

         [Test]
        public void StaffCanExistIndependently_AggregationNotComposition()
        {
            // Arrange/Act: Create a Staff member with no Restaurant
            var loneStaff = new MockStaff(1001, "Indy Staff", "555-0000");

            // Assert
            Assert.That(loneStaff.Restaurant, Is.Null,
                "Staff should not be forced to belong to a Restaurant.");
            Assert.That(Staff.GetAll(), Contains.Item(loneStaff),
                "Staff should exist in the global Staff extent even without a Restaurant.");
        }

        [Test]
        public void RemoveRestaurant_StaffStillExists()
        {
            // Arrange
            var restaurant = new Restaurant(2, "Temp Resto", "456 Temp Ave", "555-4567", new List<Menu>());
            var staffMember = new MockStaff(2002, "Temp Staff", "555-2002");
            restaurant.AddStaff(staffMember);

            // Act: Remove the restaurant  
           
            var allRestaurants = new List<Restaurant>(Restaurant.GetAll());
            allRestaurants.Remove(restaurant);
            Restaurant.SetAll(allRestaurants);

            // Assert
            // Now "restaurant" is no longer in the Restaurant extent,
            
            Assert.That(Restaurant.GetAll(), Does.Not.Contain(restaurant),
                "Restaurant no longer in the global extent.");
            Assert.That(Staff.GetAll(), Contains.Item(staffMember),
                "Staff remains in the Staff extent, proving it wasn't destroyed.");
        
        }

        
    



       [Test]
public void UpdateStaff_ReassignsStaffToNewRestaurant()
{
    // Arrange
    var restaurantA = new Restaurant(3, "Resto A", "123 A Street", "555-AAAA", new List<Menu>());
    var restaurantB = new Restaurant(4, "Resto B", "456 B Avenue", "555-BBBB", new List<Menu>());

    var staffMember = new MockStaff(303, "Charlie", "333-4444");
    restaurantA.AddStaff(staffMember);

    // Act: Move staff from restaurantA to restaurantB
    
    restaurantA.RemoveStaff(staffMember);
    restaurantB.AddStaff(staffMember);

    // Assert
    Assert.That(restaurantA.GetStaff(), Is.Empty,
        "After reassignment, Restaurant A should no longer contain the staff.");
    Assert.That(restaurantB.GetStaff(), Contains.Item(staffMember),
        "Restaurant B should now contain the staff.");
    Assert.That(staffMember.Restaurant, Is.EqualTo(restaurantB),
        "Staff's Restaurant reference should be updated to Restaurant B.");
}


        [Test]
        public void AddStaff_NullShouldThrowException()
        {
            // Arrange
            var restaurant = new Restaurant(
                restaurantId: 5,
                name: "Null Test Resto",
                address: "123 Nowhere",
                contactNumber: "555-9999",
                menus: new List<Menu>()
            );

            // Act & Assert
            Assert.Throws<ArgumentException>(() => restaurant.AddStaff(null),
                "Adding null Staff should throw an ArgumentException.");
        }

      
        [Test]
        public void TotalStaff_ReflectsCorrectCount()
        {
            // Arrange
            var restaurant = new Restaurant(
                restaurantId: 7,
                name: "Counting Resto",
                address: "123 Count Lane",
                contactNumber: "555-COUNT",
                menus: new List<Menu>()
            );

            var staffA = new MockStaff(701, "Eve", "701-0001");
            var staffB = new MockStaff(702, "Frank", "702-0002");

            // Act
            restaurant.AddStaff(staffA);
            restaurant.AddStaff(staffB);

            // Assert
            // Staff.TotalStaff should reflect the total staff *across all restaurants*.
            Assert.That(Staff.TotalStaff, Is.EqualTo(2),
                "TotalStaff should match the total number of Staff created/added in the system.");

            // Also check the Restaurant's staff count 
            Assert.That(restaurant.GetStaff().Count, Is.EqualTo(2),
                "Restaurant's staff list should contain two staff members.");
        }
    }

    /// <summary>
    /// A quick mock subclass of Staff for testing 
    /// </summary>
    public class MockStaff : Staff
    {
        public MockStaff(int staffID, string name, string contactNumber = null)
            : base(staffID, name, contactNumber)
        {
        }

        // Parameterless constructor needed for serialization or reflection, possibly
        public MockStaff() : base() { }
    }
}
