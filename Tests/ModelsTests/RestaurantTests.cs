using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class RestaurantTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset static fields before each test to ensure test isolation
            Restaurant.SetAll(new List<Restaurant>());
            Restaurant.TotalRestaurants = 0;
            Staff.SetAll(new List<Staff>());
            Staff.TotalStaff = 0;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset static fields after each test
            Restaurant.SetAll(new List<Restaurant>());
            Restaurant.TotalRestaurants = 0;
            Staff.SetAll(new List<Staff>());
            Staff.TotalStaff = 0;
        }

        [Test]
        public void Name_ShouldThrowException_WhenNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Restaurant(1, null, "221B Baker Street", "020-7946-0958", new List<Menu>(), null));
            Assert.That(ex.Message, Is.EqualTo("Name cannot be null or empty."));
        }

        [Test]
        public void Address_ShouldThrowException_WhenNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Restaurant(1, "The Crown", null, "020-7946-0958", new List<Menu>(), null));
            Assert.That(ex.Message, Is.EqualTo("Address cannot be null or empty."));
        }

        [Test]
        public void ContactNumber_ShouldThrowException_WhenNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Restaurant(1, "The Crown", "221B Baker Street", null, new List<Menu>(), null));
            Assert.That(ex.Message, Is.EqualTo("ContactNumber cannot be null or empty."));
        }

        [Test]
        public void Constructor_ShouldInitializeRestaurantCorrectly()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(1, menuItems) };
            var openingHours = new Dictionary<string, string>
            {
                { "Monday", "9:00 AM - 6:00 PM" },
                { "Tuesday", "9:00 AM - 6:00 PM" }
            };

            // Act
            var restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", menus, openingHours);

            // Assert
            Assert.AreEqual(1, restaurant.RestaurantId, "RestaurantId should be set correctly.");
            Assert.AreEqual("The Crown", restaurant.Name, "Name should be set correctly.");
            Assert.AreEqual("221B Baker Street", restaurant.Address, "Address should be set correctly.");
            Assert.AreEqual("020-7946-0958", restaurant.ContactNumber, "ContactNumber should be set correctly.");
            Assert.AreEqual(menus, restaurant.Menus, "Menus should be assigned correctly.");
            Assert.AreEqual(2, restaurant.OpeningHours.Count, "OpeningHours should be assigned correctly.");
            Assert.AreEqual(1, Restaurant.TotalRestaurants, "TotalRestaurants should increment to 1 after creating one restaurant.");
            CollectionAssert.Contains(Restaurant.GetAll(), restaurant, "The created restaurant should be in the restaurants extent.");
        }

        [Test]
        public void Constructor_ShouldSetOpeningHoursToEmpty_WhenNotProvided()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(2, menuItems) };
            var restaurant = new Restaurant(2, "The Royal", "10 Downing Street", "020-1234-5678", menus);

            // Assert
            Assert.AreEqual(0, restaurant.OpeningHours.Count, "OpeningHours should be empty when not provided.");
        }

        [Test]
        public void SetAll_WithLoadedRestaurants_ShouldSetExtentCorrectly()
        {
            // Arrange
            var menuItems1 = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menuItems2 = new List<MenuItem> { new MenuItem(2, "Shepherd's Pie", 12.0, 700, 10.0, 20) };
            var menus1 = new List<Menu> { new Menu(1, menuItems1) };
            var menus2 = new List<Menu> { new Menu(2, menuItems2) };
            var restaurant1 = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", menus1, null);
            var restaurant2 = new Restaurant(2, "The Royal", "10 Downing Street", "020-1234-5678", menus2, null);

            var loadedRestaurants = new List<Restaurant> { restaurant1, restaurant2 };

            // Act
            Restaurant.SetAll(loadedRestaurants);

            // Assert
            Assert.AreEqual(2, Restaurant.TotalRestaurants, "TotalRestaurants should reflect the loaded restaurants.");
            CollectionAssert.AreEquivalent(loadedRestaurants, Restaurant.GetAll(), "Restaurants extent should match the loaded restaurants.");
        }

        [Test]
        public void SetAll_ShouldSetEmptyList_WhenNullIsPassed()
        {
            // Act
            Restaurant.SetAll(new List<Restaurant>());

            // Assert
            var allRestaurants = Restaurant.GetAll();

            Assert.AreEqual(0, allRestaurants.Count, "Restaurants extent should be empty.");
            Assert.AreEqual(0, Restaurant.TotalRestaurants, "TotalRestaurants should be 0.");
        }

        [Test]
        public void TotalRestaurants_ShouldThrowException_WhenSetToNegative()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Restaurant.TotalRestaurants = -1);
            Assert.AreEqual("TotalRestaurants cannot be negative.", ex.Message,
                "Exception message should indicate that TotalRestaurants cannot be negative.");
        }

        [Test]
        public void GetAll_ShouldReturnAllRestaurants()
        {
            // Arrange
            var menuItems1 = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menuItems2 = new List<MenuItem> { new MenuItem(2, "Shepherd's Pie", 12.0, 700, 10.0, 20) };
            var menus1 = new List<Menu> { new Menu(3, menuItems1) };
            var menus2 = new List<Menu> { new Menu(4, menuItems2) };
            var restaurant1 = new Restaurant(3, "The Crown", "221B Baker Street", "020-7946-0958", menus1, null);
            var restaurant2 = new Restaurant(4, "The Royal", "10 Downing Street", "020-1234-5678", menus2, null);

            // Act
            var allRestaurants = Restaurant.GetAll();

            // Assert
            Assert.AreEqual(2, allRestaurants.Count, "GetAll should return all created restaurants.");
            CollectionAssert.Contains(allRestaurants, restaurant1, "First restaurant should be in the restaurants extent.");
            CollectionAssert.Contains(allRestaurants, restaurant2, "Second restaurant should be in the restaurants extent.");
        }

        [Test]
        public void TotalRestaurants_ShouldUpdateAfterRestaurantAdded()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(5, menuItems) };
            var restaurant1 = new Restaurant(5, "The Crown", "221B Baker Street", "020-7946-0958", menus, null);
            var restaurant2 = new Restaurant(6, "The Royal", "10 Downing Street", "020-1234-5678", menus, null);

            // Act & Assert
            Assert.AreEqual(2, Restaurant.TotalRestaurants, "TotalRestaurants should increment correctly after adding restaurants.");
        }
    }
}