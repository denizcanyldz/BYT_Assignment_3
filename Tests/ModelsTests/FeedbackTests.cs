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
            var feedback = new Feedback(1, 101, 4, "Good service");
            Assert.That(feedback.FeedbackID, Is.EqualTo(1));
            Assert.That(feedback.CustomerID, Is.EqualTo(101));
            Assert.That(feedback.Rating, Is.EqualTo(4));
            Assert.That(feedback.Comments, Is.EqualTo("Good service"));
        }

        [Test]
        public void Feedback_ThrowsExceptionForNegativeTotalFeedbacks()
        {
            Assert.Throws<ArgumentException>(() => Feedback.TotalFeedbacks = -1);
        }

        [Test]
        public void Feedback_ThrowsExceptionForInvalidRating()
        {
            Assert.Throws<ArgumentException>(() => new Feedback(2, 101, 6));
            Assert.Throws<ArgumentException>(() => new Feedback(2, 101, 0));
        }

        [Test]
        public void Feedback_IsCorrectlySavedInExtent()
        {
            var feedback = new Feedback(2, 102, 5, "Excellent");
            Assert.That(Feedback.GetAll(), Contains.Item(feedback));
        }
    }
}