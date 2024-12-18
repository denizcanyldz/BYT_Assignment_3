using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Feedback
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalFeedbacks = 0;

        /// <summary>
        /// Gets or sets the total number of feedbacks.
        /// </summary>
        public static int TotalFeedbacks
        {
            get => totalFeedbacks;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalFeedbacks cannot be negative.");
                totalFeedbacks = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Feedback> feedbacks = new List<Feedback>();

        /// <summary>
        /// Gets a read-only list of all feedbacks.
        /// </summary>
        public static IReadOnlyList<Feedback> GetAll()
        {
            return feedbacks.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire feedback list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Feedback> loadedFeedbacks)
        {
            feedbacks = loadedFeedbacks ?? new List<Feedback>();
            TotalFeedbacks = feedbacks.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int FeedbackID { get; set; }

        private string content;

        public string Content
        {
            get => content;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Content cannot be null or empty.");
                content = value;
            }
        }

        private DateTime feedbackDate;

        public DateTime FeedbackDate
        {
            get => feedbackDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("FeedbackDate cannot be in the future.");
                feedbackDate = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private int rating;

        public int Rating
        {
            get => rating;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("Rating must be between 1 and 5.");
                rating = value;
            }
        }

        private string? response;

        public string? Response
        {
            get => response;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 500)
                    throw new ArgumentException("Response length cannot exceed 500 characters.");
                response = value;
            }
        }

        // -------------------------------
        // Association Attributes
        // -------------------------------
        private Customer customer;

        [XmlIgnore]
        public Customer Customer
        {
            get => customer;
            private set
            {
                if (value == null)
                    throw new ArgumentException("Customer cannot be null.");
                customer = value;
            }
        }

        private Restaurant restaurant;

        [XmlIgnore]
        public Restaurant Restaurant
        {
            get => restaurant;
            private set
            {
                if (value == null)
                    throw new ArgumentException("Restaurant cannot be null.");
                restaurant = value;
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Feedback class with mandatory and optional attributes.
        /// </summary>
        public Feedback(int feedbackID, string content, DateTime feedbackDate, int rating, Restaurant restaurant, Customer customer, string? response = null)
        {
            FeedbackID = feedbackID;
            Content = content;
            FeedbackDate = feedbackDate;
            Rating = rating;
            Response = response;

            // Associate with Customer
            SetCustomer(customer);

            // Add to class extent
            feedbacks.Add(this);
            TotalFeedbacks = feedbacks.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Feedback()
        {
            // Initialize associations
        }

        // -------------------------------
        // Association Methods
        // -------------------------------

        /// <summary>
        /// Sets the Customer for the Feedback, maintaining bidirectional association.
        /// </summary>
        public void SetCustomer(Customer newCustomer)
        {
            if (newCustomer == null)
                throw new ArgumentNullException(nameof(newCustomer), "Customer cannot be null.");

            if (this.customer != null && this.customer != newCustomer)
            {
                // Remove from the old customer's feedbacks
                this.customer.RemoveFeedback(this);
            }

            this.customer = newCustomer;

            // Add this feedback to the new customer's feedbacks if not already present
            if (!newCustomer.Feedbacks.Contains(this))
            {
                newCustomer.AddFeedback(this);
            }
        }

        /// <summary>
        /// Removes the association with the current Customer, maintaining bidirectional consistency.
        /// </summary>
        public void RemoveCustomer()
        {
            if (this.customer != null)
            {
                var oldCustomer = this.customer;
                this.customer = null;

                // Remove this feedback from the old customer's feedbacks
                if (oldCustomer.Feedbacks.Contains(this))
                {
                    oldCustomer.RemoveFeedback(this);
                }
            }
        }


        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Feedback other)
            {
                return FeedbackID == other.FeedbackID &&
                       Content == other.Content &&
                       FeedbackDate == other.FeedbackDate &&
                       Rating == other.Rating &&
                       Response == other.Response &&
                       Customer.Equals(other.Customer) &&
                       Restaurant.Equals(other.Restaurant);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FeedbackID, Content, FeedbackDate, Rating, Response, Customer, Restaurant);
        }
    }
}
