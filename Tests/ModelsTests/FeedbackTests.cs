using NUnit.Framework;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using BYT_Assignment_3.Persistences;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class FeedbackTests
    {
        // Path to the test XML file used for serialization/deserialization
        private const string TestFilePath = "test_feedbacks.xml";

        /// <summary>
        /// Runs before each test to ensure a clean environment.
        /// Resets all class extents and deletes the test XML file if it exists.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Reset all class extents to ensure no residual data from previous tests
            Feedback.SetAll(new List<Feedback>());
            Customer.SetAll(new List<Customer>());
            Restaurant.SetAll(new List<Restaurant>());
        }

        /// <summary>
        /// Runs after each test to clean up the test environment.
        /// Deletes the test XML file and resets all class extents.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Clean up the test file after each test
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            // Reset class extents after each test
            SetUp();
        }

        #region Feedback Class Tests

        /// <summary>
        /// Tests that a Feedback object is created correctly with all mandatory attributes.
        /// </summary>
        [Test]
        public void Feedback_CreatesObjectCorrectly_WithMandatoryAttributes()
        {
            // Arrange
            var restaurant = new Restaurant(1, "Gourmet Hub", "123 Culinary St.", "555-1234");
            var customer = new Customer(1, "Emily Clark", "555-2468");

            // Act
            var feedback = new Feedback(
                feedbackID: 1,
                content: "Excellent service!",
                feedbackDate: DateTime.Now.AddDays(-1),
                rating: 5,
                restaurant: restaurant,
                customer: customer,
                response: "Thank you for your feedback!"
            );

            // Assert
            Assert.AreEqual(1, feedback.FeedbackID, "FeedbackID should be set correctly.");
            Assert.AreEqual("Excellent service!", feedback.Content, "Content should be set correctly.");
            Assert.AreEqual(5, feedback.Rating, "Rating should be set correctly.");
            Assert.LessOrEqual(feedback.FeedbackDate, DateTime.Now, "FeedbackDate should not be in the future.");
            Assert.AreEqual("Thank you for your feedback!", feedback.Response, "Response should be set correctly.");
            Assert.AreEqual(customer, feedback.Customer, "Feedback's Customer should be correctly set.");
            Assert.AreEqual(restaurant, feedback.Restaurant, "Feedback's Restaurant should be correctly set.");
        }

        /// <summary>
        /// Tests that creating a Feedback with an invalid Rating throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_InvalidRating_ShouldThrowException()
        {
            // Arrange
            var restaurant = new Restaurant(2, "Spice Symphony", "789 Aroma Blvd.", "555-1357");
            var customer = new Customer(2, "Michael Scott", "555-9753");

            // Act & Assert
            var exLow = Assert.Throws<ArgumentException>(() =>
                new Feedback(2, "Poor service.", DateTime.Now.AddDays(-2), 0, restaurant, customer));
            Assert.That(exLow.Message, Is.EqualTo("Rating must be between 1 and 5."));

            var exHigh = Assert.Throws<ArgumentException>(() =>
                new Feedback(3, "Outstanding!", DateTime.Now.AddDays(-3), 6, restaurant, customer));
            Assert.That(exHigh.Message, Is.EqualTo("Rating must be between 1 and 5."));
        }

        /// <summary>
        /// Tests that creating a Feedback with a future FeedbackDate throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_FutureFeedbackDate_ShouldThrowException()
        {
            // Arrange
            var restaurant = new Restaurant(3, "Herbal Haven", "321 Greenway Rd.", "555-8642");
            var customer = new Customer(3, "Dwight Schrute", "555-1122");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new Feedback(4, "Great ambiance.", DateTime.Now.AddDays(1), 4, restaurant, customer)
            );
            Assert.That(ex.Message, Is.EqualTo("FeedbackDate cannot be in the future."));
        }

        /// <summary>
        /// Tests that creating a Feedback with empty or whitespace Content throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_EmptyOrWhitespaceContent_ShouldThrowException()
        {
            // Arrange
            var restaurant = new Restaurant(4, "The Culinary Spot", "456 Flavor Ave.", "555-6789");
            var customer = new Customer(4, "John Doe", "555-6789");

            var invalidContents = new List<string> { "", "   ", "\t", "\n" };

            foreach (var invalidContent in invalidContents)
            {
                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() =>
                    new Feedback(5, invalidContent, DateTime.Now.AddDays(-4), 3, restaurant, customer)
                );
                Assert.That(ex.Message, Is.EqualTo("Content cannot be null or empty."));
            }
        }

        /// <summary>
        /// Tests that creating a Feedback with Content exceeding 500 characters throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_ExceedsContentLength_ShouldThrowException()
        {
            // Arrange
            var restaurant = new Restaurant(5, "Flavor Fiesta", "789 Spice St.", "555-2468");
            var customer = new Customer(5, "Jim Halpert", "555-1357");

            var longContent = new string('A', 501); // 501 characters

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new Feedback(6, longContent, DateTime.Now.AddDays(-5), 4, restaurant, customer)
            );
            Assert.That(ex.Message, Is.EqualTo("Content length cannot exceed 500 characters."));
        }

        /// <summary>
        /// Tests that adding a Feedback increases the TotalFeedbacks count and includes it in the extent.
        /// </summary>
        [Test]
        public void Feedback_AddingFeedback_ShouldIncreaseTotalFeedbacks()
        {
            // Arrange
            var restaurant = new Restaurant(6, "Spice Symphony", "789 Aroma Blvd.", "555-1357");
            var customer = new Customer(6, "Pam Beesly", "555-8642");

            var initialCount = Feedback.TotalFeedbacks;

            var feedback = new Feedback(
                feedbackID: 7,
                content: "Loved the desserts!",
                feedbackDate: DateTime.Now.AddDays(-6),
                rating: 5,
                restaurant: restaurant,
                customer: customer,
                response: "Thank you! We're glad you enjoyed our desserts."
            );

            // Act & Assert
            Assert.AreEqual(initialCount + 1, Feedback.TotalFeedbacks, "TotalFeedbacks should increment by one.");
            CollectionAssert.Contains(Feedback.GetAll(), feedback, "Feedback should be added to the extent.");
        }


        /// <summary>
        /// Tests that updating a Feedback replaces the old one with the new one correctly.
        /// </summary>
        [Test]
        public void Feedback_UpdateFeedback_ShouldReplaceOldWithNew()
        {
            // Arrange
            var restaurant = new Restaurant(8, "Herbal Haven", "321 Greenway Rd.", "555-8642");
            var customer = new Customer(8, "Kevin Malone", "555-1122");

            var feedbackOld = new Feedback(
                feedbackID: 9,
                content: "Decent experience.",
                feedbackDate: DateTime.Now.AddDays(-8),
                rating: 3,
                restaurant: restaurant,
                customer: customer,
                response: null
            );

            var feedbackNew = new Feedback(
                feedbackID: 10,
                content: "Improved service!",
                feedbackDate: DateTime.Now.AddDays(-9),
                rating: 4,
                restaurant: restaurant,
                customer: customer,
                response: "Thank you for noticing the improvements!"
            );

            // Act
            customer.UpdateFeedback(feedbackOld, feedbackNew);

            // Assert
            CollectionAssert.DoesNotContain(customer.Feedbacks, feedbackOld,
                "Old Feedback should be removed from Customer's Feedbacks.");
            CollectionAssert.Contains(customer.Feedbacks, feedbackNew,
                "New Feedback should be added to Customer's Feedbacks.");
            Assert.AreEqual(customer, feedbackNew.Customer, "New Feedback's Customer should be correctly set.");
            Assert.AreEqual(restaurant, feedbackNew.Restaurant, "New Feedback's Restaurant should be correctly set.");
        }

        /// <summary>
        /// Tests that adding a duplicate Feedback throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_AddDuplicateFeedback_ShouldThrowException()
        {
            // Arrange
            var restaurant = new Restaurant(9, "Flavor Fiesta", "789 Spice St.", "555-2468");
            var customer = new Customer(9, "Stanley Hudson", "555-1357");

            var feedback = new Feedback(
                feedbackID: 11,
                content: "Loved the appetizers!",
                feedbackDate: DateTime.Now.AddDays(-10),
                rating: 5,
                restaurant: restaurant,
                customer: customer,
                response: "Glad you enjoyed them!"
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => customer.AddFeedback(feedback));
            Assert.That(ex.Message, Is.EqualTo("Feedback already exists in the Customer."));
        }

        /// <summary>
        /// Tests that removing a non-existent Feedback throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_RemoveNonExistentFeedback_ShouldThrowException()
        {
            // Arrange
            var restaurant = new Restaurant(10, "Spice Symphony", "789 Aroma Blvd.", "555-1357");
            var customer = new Customer(10, "Toby Flenderson", "555-8642");

            var feedback = new Feedback(
                feedbackID: 12,
                content: "Good ambiance.",
                feedbackDate: DateTime.Now.AddDays(-11),
                rating: 4,
                restaurant: restaurant,
                customer: customer,
                response: null
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => customer.RemoveFeedback(feedback));
            Assert.That(ex.Message, Is.EqualTo("Feedback not found in the Customer."));
        }

        /// <summary>
        /// Tests that a Feedback associates correctly with a Customer.
        /// </summary>
        [Test]
        public void Feedback_AssociatesWithCustomer_Correctly()
        {
            // Arrange
            var restaurant = new Restaurant(11, "Gourmet Hub", "123 Culinary St.", "555-1234");
            var customer = new Customer(11, "Kelly Kapoor", "555-6789");

            var feedback = new Feedback(
                feedbackID: 13,
                content: "Exceptional desserts!",
                feedbackDate: DateTime.Now.AddDays(-12),
                rating: 5,
                restaurant: restaurant,
                customer: customer,
                response: "Thank you! We're delighted you enjoyed them."
            );

            // Act & Assert
            Assert.Contains(feedback, (System.Collections.ICollection)customer.Feedbacks,
                "Feedback should be in Customer's Feedbacks list.");
            Assert.AreEqual(customer, feedback.Customer, "Feedback's Customer should be correctly set.");
        }

       

        /// <summary>
        /// Tests that setting a Feedback's Customer updates the association correctly.
        /// </summary>
        [Test]
        public void Feedback_SetCustomer_ShouldUpdateAssociation()
        {
            // Arrange
            var restaurant = new Restaurant(13, "Herbal Haven", "321 Greenway Rd.", "555-8642");
            var customer1 = new Customer(13, "Meredith Palmer", "555-1122");
            var customer2 = new Customer(14, "Oscar Martinez", "555-3344");

            var feedback = new Feedback(
                feedbackID: 15,
                content: "Loved the herbal teas!",
                feedbackDate: DateTime.Now.AddDays(-14),
                rating: 5,
                restaurant: restaurant,
                customer: customer1,
                response: "Thank you! We're happy you enjoyed our teas."
            );

            // Act
            feedback.SetCustomer(customer2);

            // Assert
            Assert.IsFalse(customer1.Feedbacks.Contains(feedback),
                "Feedback should be removed from the old Customer's Feedbacks.");
            Assert.IsTrue(customer2.Feedbacks.Contains(feedback),
                "Feedback should be added to the new Customer's Feedbacks.");
            Assert.AreEqual(customer2, feedback.Customer, "Feedback's Customer should be updated to the new Customer.");
        }

        

        #endregion
    }
}
