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
            Staff.TotalStaff = 0;
        }

        [TearDown]
        public void TearDown()
        {
            // Reset static fields after each test
            Restaurant.SetAll(new List<Restaurant>());
            Restaurant.TotalRestaurants = 0;
            Staff.TotalStaff = 0;
        }

        [Test]
        public void Constructor_ShouldInitializeRestaurantCorrectly()
        {
            var menus = new List<Menu> { new Menu() };
            var openingHours = new Dictionary<string, string>
            {
                { "Monday", "9:00 AM - 6:00 PM" },
                { "Tuesday", "9:00 AM - 6:00 PM" }
            };

            var restaurant = new Restaurant(1, "Test Restaurant", "123 Test St.", "123-456-7890", menus, openingHours);

            Assert.That(restaurant.RestaurantId, Is.EqualTo(1));
            Assert.That(restaurant.Name, Is.EqualTo("Test Restaurant"));
            Assert.That(restaurant.Address, Is.EqualTo("123 Test St."));
            Assert.That(restaurant.ContactNumber, Is.EqualTo("123-456-7890"));
            Assert.That(restaurant.Menus.Count, Is.EqualTo(1));
            Assert.That(restaurant.OpeningHours.Count, Is.EqualTo(2));
            Assert.That(Restaurant.TotalRestaurants, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_ShouldSetOpeningHoursToEmpty_WhenNotProvided()
        {
            var menus = new List<Menu> { new Menu() };
            var restaurant = new Restaurant(2, "Another Restaurant", "456 Another St.", "987-654-3210", menus);

            Assert.That(restaurant.OpeningHours.Count, Is.EqualTo(0));
        }

        [Test]
        public void SetAll_ShouldUpdateRestaurantListCorrectly()
        {
            var menus = new List<Menu> { new Menu() };
            var restaurant1 = new Restaurant(3, "Restaurant One", "789 Restaurant St.", "123-789-4560", menus);
            var restaurant2 = new Restaurant(4, "Restaurant Two", "101 Restaurant Ave.", "987-123-4567", menus);

            var newRestaurants = new List<Restaurant> { restaurant1, restaurant2 };
            Restaurant.SetAll(newRestaurants);

            var allRestaurants = Restaurant.GetAll();

            Assert.That(allRestaurants.Count, Is.EqualTo(2));
            Assert.Contains(restaurant1, (System.Collections.ICollection)allRestaurants);
            Assert.Contains(restaurant2, (System.Collections.ICollection)allRestaurants);
            Assert.That(Restaurant.TotalRestaurants, Is.EqualTo(2));
        }

        [Test]
        public void SetAll_ShouldSetEmptyList_WhenNullIsPassed()
        {
            Restaurant.SetAll(null);

            var allRestaurants = Restaurant.GetAll();

            Assert.That(allRestaurants.Count, Is.EqualTo(0));
            Assert.That(Restaurant.TotalRestaurants, Is.EqualTo(0));
        }

        [Test]
        public void TotalRestaurants_ShouldThrowException_WhenSetToNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => Restaurant.TotalRestaurants = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalWaiterBartenders cannot be negative."));
        }

        [Test]
        public void GetAll_ShouldReturnAllRestaurants()
        {
            var menus = new List<Menu> { new Menu() };
            var restaurant1 = new Restaurant(5, "Restaurant Three", "111 Third St.", "555-123-4567", menus);
            var restaurant2 = new Restaurant(6, "Restaurant Four", "222 Fourth Ave.", "555-987-6543", menus);

            var allRestaurants = Restaurant.GetAll();

            Assert.That(allRestaurants.Count, Is.EqualTo(2));
            Assert.Contains(restaurant1, (System.Collections.ICollection)allRestaurants);
            Assert.Contains(restaurant2, (System.Collections.ICollection)allRestaurants);
        }


        [Test]
        public void TotalRestaurants_ShouldUpdateAfterRestaurantAdded()
        {
            var menus = new List<Menu> { new Menu() };
            var restaurant1 = new Restaurant(9, "Restaurant Five", "444 Fifth St.", "555-111-2233", menus);
            var restaurant2 = new Restaurant(10, "Restaurant Six", "555 Sixth St.", "555-222-3344", menus);

            Assert.That(Restaurant.TotalRestaurants, Is.EqualTo(2));
        }
    }
}