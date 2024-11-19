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
            
            // Ensure the test file does not exist before each test
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
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
            var feedback = new Feedback(1, 1001, 5, DateTime.Now.AddDays(-1), "Excellent service!");

            // Act & Assert
            Assert.AreEqual(1, feedback.FeedbackID, "FeedbackID should be set correctly.");
            Assert.AreEqual(1001, feedback.CustomerID, "CustomerID should be set correctly.");
            Assert.AreEqual(5, feedback.Rating, "Rating should be set correctly.");
            Assert.LessOrEqual(feedback.DateTime, DateTime.Now, "DateTime should not be in the future.");
            Assert.AreEqual("Excellent service!", feedback.Comments, "Comments should be set correctly.");
        }

        /// <summary>
        /// Tests that creating a Feedback with an invalid CustomerID throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_InvalidCustomerID_ShouldThrowException()
        {
            // Arrange
            var invalidCustomerID = 0;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Feedback(2, invalidCustomerID, 4, DateTime.Now.AddDays(-2), "Good."));
            Assert.AreEqual("CustomerID must be positive.", ex.Message, "Exception message should indicate invalid CustomerID.");
        }

        /// <summary>
        /// Tests that creating a Feedback with an invalid Rating throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_InvalidRating_ShouldThrowException()
        {
            // Arrange
            var invalidRatings = new List<int> { 0, 6 };

            foreach (var invalidRating in invalidRatings)
            {
                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => new Feedback(3, 1002, invalidRating, DateTime.Now.AddDays(-3), "Average."));
                Assert.AreEqual("Rating must be between 1 and 5.", ex.Message, "Exception message should indicate invalid Rating.");
            }
        }

        /// <summary>
        /// Tests that creating a Feedback with a future DateTime throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_FutureDateTime_ShouldThrowException()
        {
            // Arrange
            var futureDateTime = DateTime.Now.AddDays(1);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Feedback(4, 1003, 3, futureDateTime, "Okay."));
            Assert.AreEqual("DateTime cannot be in the future.", ex.Message, "Exception message should indicate invalid DateTime.");
        }

        /// <summary>
        /// Tests that creating a Feedback with Comments exceeding 500 characters throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_ExceedsCommentsLength_ShouldThrowException()
        {
            // Arrange
            var longComments = new string('A', 501); // 501 characters

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Feedback(5, 1004, 4, DateTime.Now.AddDays(-4), longComments));
            Assert.AreEqual("Comments length cannot exceed 500 characters.", ex.Message, "Exception message should indicate excessive Comments length.");
        }

        /// <summary>
        /// Tests that creating a Feedback with empty or whitespace Comments throws an ArgumentException.
        /// </summary>
        [Test]
        public void Feedback_EmptyOrWhitespaceComments_ShouldThrowException()
        {
            // Arrange
            var invalidCommentsList = new List<string?> { "", "   ", "\t", "\n" };

            foreach (var invalidComments in invalidCommentsList)
            {
                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => new Feedback(6, 1005, 2, DateTime.Now.AddDays(-5), invalidComments));
                Assert.AreEqual("Comments cannot be empty or whitespace.", ex.Message, "Exception message should indicate invalid Comments.");
            }
        }

        /// <summary>
        /// Tests that adding a Feedback to the class extent increases the TotalFeedbacks count.
        /// </summary>
        [Test]
        public void Feedback_AddingFeedback_ShouldIncreaseTotalFeedbacks()
        {
            // Arrange
            var initialCount = Feedback.TotalFeedbacks;

            var feedback = new Feedback(7, 1006, 5, DateTime.Now.AddDays(-6), "Outstanding!");

            // Act & Assert
            Assert.AreEqual(initialCount + 1, Feedback.TotalFeedbacks, "TotalFeedbacks should increment by one.");
            Assert.Contains(feedback, (System.Collections.ICollection)Feedback.GetAll(), "Feedback should be added to the extent.");
        }

        /// <summary>
        /// Tests that the AverageRating property calculates correctly.
        /// </summary>
        [Test]
        public void Feedback_AverageRating_ShouldCalculateCorrectly()
        {
            // Arrange
            var feedback1 = new Feedback(8, 1007, 4, DateTime.Now.AddDays(-7), "Good.");
            var feedback2 = new Feedback(9, 1008, 5, DateTime.Now.AddDays(-8), "Excellent.");
            var feedback3 = new Feedback(10, 1009, 3, DateTime.Now.AddDays(-9), "Average.");

            var expectedAverage = (4 + 5 + 3) / 3.0;

            // Act
            var actualAverage = Feedback.AverageRating;

            // Assert
            Assert.AreEqual(expectedAverage, actualAverage, "AverageRating should be calculated correctly.");
        }

        /// <summary>
        /// Tests that setting the Feedback extent to null initializes it as an empty list.
        /// </summary>
        [Test]
        public void SetAll_Null_ShouldInitializeEmptyExtent()
        {
            // Act
            Feedback.SetAll(null);

            // Assert
            Assert.AreEqual(0, Feedback.TotalFeedbacks, "TotalFeedbacks should be 0 after setting null.");
            Assert.IsEmpty(Feedback.GetAll(), "Feedback extent should be empty after setting null.");
        }

        /// <summary>
        /// Tests that the Feedback's Equals and GetHashCode methods function correctly.
        /// </summary>
        [Test]
        public void Feedback_EqualsAndHashCode_ShouldFunctionCorrectly()
        {
            // Arrange
            var fixedDate = new DateTime(2023, 10, 10, 12, 0, 0); // Fixed DateTime
            var feedback1 = new Feedback(14, 1013, 5, fixedDate, "Outstanding!");
            var feedback2 = new Feedback(14, 1013, 5, fixedDate, "Outstanding!");
            var feedback3 = new Feedback(15, 1014, 3, fixedDate.AddDays(1), "Average."); // Different DateTime

            // Act & Assert
            Assert.IsTrue(feedback1.Equals(feedback2), "Feedbacks with identical properties should be equal.");
            Assert.AreEqual(feedback1.GetHashCode(), feedback2.GetHashCode(), "HashCodes of identical feedbacks should be equal.");

            Assert.IsFalse(feedback1.Equals(feedback3), "Feedbacks with different properties should not be equal.");
            Assert.AreNotEqual(feedback1.GetHashCode(), feedback3.GetHashCode(), "HashCodes of different feedbacks should not be equal.");
        }


        /// <summary>
        /// Tests that the AverageRating property returns 0 when there are no feedback entries.
        /// </summary>
        [Test]
        public void AverageRating_NoFeedbacks_ShouldReturnZero()
        {
            // Arrange
            // No feedback entries added

            // Act
            var average = Feedback.AverageRating;

            // Assert
            Assert.AreEqual(0.0, average, "AverageRating should be 0 when there are no feedbacks.");
        }

        /// <summary>
        /// Tests that the AverageRating property calculates correctly with multiple feedback entries.
        /// </summary>
        [Test]
        public void AverageRating_WithFeedbacks_ShouldCalculateCorrectly()
        {
            // Arrange
            var feedback1 = new Feedback(16, 1015, 4, DateTime.Now.AddDays(-12), "Good.");
            var feedback2 = new Feedback(17, 1016, 5, DateTime.Now.AddDays(-13), "Excellent.");
            var feedback3 = new Feedback(18, 1017, 3, DateTime.Now.AddDays(-14), "Average.");

            var expectedAverage = (4 + 5 + 3) / 3.0;

            // Act
            var actualAverage = Feedback.AverageRating;

            // Assert
            Assert.AreEqual(expectedAverage, actualAverage, "AverageRating should be calculated correctly.");
        }

        /// <summary>
        /// Tests that adding a Feedback with Comments set to null does not throw an exception.
        /// </summary>
        [Test]
        public void Feedback_NullComments_ShouldAddSuccessfully()
        {
            // Arrange
            var feedback = new Feedback(19, 1018, 4, DateTime.Now.AddDays(-15), null);

            // Act & Assert
            Assert.AreEqual(null, feedback.Comments, "Comments should be null.");
            Assert.AreEqual(1, Feedback.TotalFeedbacks, "TotalFeedbacks should be incremented.");
            CollectionAssert.Contains(Feedback.GetAll(), feedback, "Feedback should be added to the extent.");
        }

        #endregion

        // ... (Other tests for different classes can be placed here)
    }
}
