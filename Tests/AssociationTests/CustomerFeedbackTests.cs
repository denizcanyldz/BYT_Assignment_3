using System;
using NUnit.Framework;
using BYT_Assignment_3.Models;

namespace Tests
{
    [TestFixture]
    public class CustomerFeedbackTests
    {
        // Test for adding feedback to a customer
        [Test]
        public void AddFeedback_AddsFeedbackCorrectly()
        {
            var customer = new Customer(1, "Michael");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            customer.AddFeedback(feedback);

            Assert.That(customer.GetFeedbacks().Contains(feedback), Is.True);
            Assert.That(feedback.GetCustomers().Contains(customer), Is.True);
        }


        // Test for adding null feedback
        [Test]
        public void AddFeedback_ThrowsExceptionForNullFeedback()
        {
            var customer = new Customer(1, "Michael");
            Assert.Throws<ArgumentException>(() => customer.AddFeedback(null));
        }

        // Test for modifying feedback for a customer
        [Test]
        public void ModifyFeedback_ModifiesFeedbackCorrectly()
        {
            var customer = new Customer(1, "Michael");
            var oldFeedback = new Feedback(1, 1, 3, DateTime.Now);
            var newFeedback = new Feedback(2, 1, 5, DateTime.Now);

            customer.AddFeedback(oldFeedback);
            customer.ModifyFeedback(oldFeedback, newFeedback);

            Assert.That(customer.GetFeedbacks().Contains(newFeedback), Is.True);
            Assert.That(customer.GetFeedbacks().Contains(oldFeedback), Is.False);
        }

        // Test for modifying feedback that doesn't exist
        [Test]
        public void ModifyFeedback_ThrowsExceptionForNonExistingFeedback()
        {
            var customer = new Customer(1, "Michael");
            var oldFeedback = new Feedback(1, 1, 3, DateTime.Now);
            var newFeedback = new Feedback(2, 1, 5, DateTime.Now);

            Assert.Throws<ArgumentException>(() => customer.ModifyFeedback(oldFeedback, newFeedback));
        }

        // Test for modifying feedback with null values
        [Test]
        public void ModifyFeedback_ThrowsExceptionForNullFeedback()
        {
            var customer = new Customer(1, "Michael");
            var feedback = new Feedback(1, 1, 3, DateTime.Now);

            customer.AddFeedback(feedback);
            Assert.Throws<ArgumentException>(() => customer.ModifyFeedback(feedback, null));
            Assert.Throws<ArgumentException>(() => customer.ModifyFeedback(null, feedback));
        }

        // Test for removing feedback from a customer
        [Test]
        public void RemoveFeedback_RemovesFeedbackCorrectly()
        {
            var customer = new Customer(1, "Michael");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            customer.AddFeedback(feedback);
            customer.RemoveFeedback(feedback);

            Assert.That(customer.GetFeedbacks().Contains(feedback), Is.False);
            Assert.That(feedback.GetCustomers().Contains(customer), Is.False);
        }

        // Test for removing feedback that doesn't exist
        [Test]
        public void RemoveFeedback_ThrowsExceptionForNonExistingFeedback()
        {
            var customer = new Customer(1, "Michael");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            Assert.Throws<KeyNotFoundException>(() => customer.RemoveFeedback(feedback));
        }

        // Test for removing null feedback
        [Test]
        public void RemoveFeedback_ThrowsExceptionForNullFeedback()
        {
            var customer = new Customer(1, "Michael");
            Assert.Throws<ArgumentException>(() => customer.RemoveFeedback(null));
        }

        // Similar tests for Feedback class to test AddCustomer, ModifyCustomer, and RemoveCustomer
        [Test]
        public void AddCustomer_AddsCustomerCorrectly()
        {
            var customer = new Customer(1, "Michael");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            feedback.AddCustomer(customer);

            Assert.That(feedback.GetCustomers().Contains(customer), Is.True);
            Assert.That(customer.GetFeedbacks().Contains(feedback), Is.True);
        }


        [Test]
        public void AddCustomer_ThrowsExceptionForNullCustomer()
        {
            var feedback = new Feedback(1, 1, 5, DateTime.Now);
            Assert.Throws<ArgumentException>(() => feedback.AddCustomer(null));
        }

        [Test]
        public void ModifyCustomer_ModifiesCustomerCorrectly()
        {
            var oldCustomer = new Customer(1, "Michael");
            var newCustomer = new Customer(2, "Sarah");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            feedback.AddCustomer(oldCustomer);
            feedback.ModifyCustomer(oldCustomer, newCustomer);

            Assert.That(feedback.GetCustomers().Contains(newCustomer), Is.True);
            Assert.That(feedback.GetCustomers().Contains(oldCustomer), Is.False);
        }

        [Test]
        public void ModifyCustomer_ThrowsExceptionForNonExistingCustomer()
        {
            var oldCustomer = new Customer(1, "Michael");
            var newCustomer = new Customer(2, "Sarah");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            Assert.Throws<ArgumentException>(() => feedback.ModifyCustomer(oldCustomer, newCustomer));
        }

        [Test]
        public void ModifyCustomer_ThrowsExceptionForNullCustomer()
        {
            var customer = new Customer(1, "Michael");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            feedback.AddCustomer(customer);
            Assert.Throws<ArgumentException>(() => feedback.ModifyCustomer(customer, null));
            Assert.Throws<ArgumentException>(() => feedback.ModifyCustomer(null, customer));
        }

        [Test]
        public void RemoveCustomer_RemovesCustomerCorrectly()
        {
            var customer = new Customer(1, "Michael");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            feedback.AddCustomer(customer);
            feedback.RemoveCustomer(customer);

            Assert.That(feedback.GetCustomers().Contains(customer), Is.False);
            Assert.That(customer.GetFeedbacks().Contains(feedback), Is.False);
        }

        [Test]
        public void RemoveCustomer_ThrowsExceptionForNonExistingCustomer()
        {
            var customer = new Customer(1, "Michael");
            var feedback = new Feedback(1, 1, 5, DateTime.Now);

            Assert.Throws<KeyNotFoundException>(() => feedback.RemoveCustomer(customer));
        }

        [Test]
        public void RemoveCustomer_ThrowsExceptionForNullCustomer()
        {
            var feedback = new Feedback(1, 1, 5, DateTime.Now);
            Assert.Throws<ArgumentException>(() => feedback.RemoveCustomer(null));
        }
    }
}
