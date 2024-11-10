using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class FeedbackTests
    {
        [SetUp]
        public void SetUp()
        {
            Feedback.SetAll(new List<Feedback>());
        }

        [Test]
        public void Feedback_CreatesObjectCorrectly()
        {
            var feedback = new Feedback(1, 101, 5, DateTime.Now, 4.5, "Great service!");
            Assert.That(feedback.FeedbackID, Is.EqualTo(1));
            Assert.That(feedback.CustomerID, Is.EqualTo(101));
            Assert.That(feedback.Rating, Is.EqualTo(5));
            Assert.That(feedback.AverageRating, Is.EqualTo(4.5));
            Assert.That(feedback.Comments, Is.EqualTo("Great service!"));
        }

        [Test]
        public void Feedback_TotalFeedbacksCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Feedback.TotalFeedbacks = -1);
        }

        [Test]
        public void Feedback_RatingMustBeBetweenOneAndFive()
        {
            Assert.Throws<ArgumentException>(() => new Feedback(2, 102, 6, DateTime.Now, 4.0));
        }

        [Test]
        public void Feedback_DateTimeCannotBeInFuture()
        {
            Assert.Throws<ArgumentException>(() => new Feedback(3, 103, 4, DateTime.Now.AddDays(1), 4.0));
        }

        [Test]
        public void Feedback_IsCorrectlySavedInExtent()
        {
            var feedback = new Feedback(4, 104, 3, DateTime.Now, 3.5, "Average experience.");
            Assert.That(Feedback.GetAll(), Contains.Item(feedback));
        }
    }
}