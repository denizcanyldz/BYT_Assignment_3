using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Gets the total number of feedback entries.
        /// </summary>
        public static int TotalFeedbacks
        {
            get => totalFeedbacks;
            private set
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
        /// Gets a read-only list of all feedback entries.
        /// </summary>
        public static IReadOnlyList<Feedback> GetAll()
        {
            return feedbacks.AsReadOnly();
        }

        private static void ValidateFeedback(Feedback feedback)
        {
            if (feedback.CustomerID <= 0)
                throw new ArgumentException("CustomerID must be positive.");
    
            if (feedback.Rating < 1 || feedback.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");
    
            if (feedback.DateTime > DateTime.Now)
                throw new ArgumentException("DateTime cannot be in the future.");
    
            if (feedback.Comments != null)
            {
                if (string.IsNullOrWhiteSpace(feedback.Comments))
                    throw new ArgumentException("Comments cannot be empty or whitespace.");
                if (feedback.Comments.Length > 500)
                    throw new ArgumentException("Comments length cannot exceed 500 characters.");
            }
        }

        /// <summary>
        /// Sets the entire feedback list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Feedback> loadedFeedbacks)
        {
            feedbacks = loadedFeedbacks ?? new List<Feedback>();
            TotalFeedbacks = 0; 

            foreach (var feedback in feedbacks.ToList()) // Use a copy to allow safe removal
            {
                try
                {
                    ValidateFeedback(feedback);
                    TotalFeedbacks++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding Feedback (FeedbackID: {feedback.FeedbackID}): {ex.Message}");
                    feedbacks.Remove(feedback); // Remove invalid feedback
                    // Alternatively, rethrow or handle as per application requirements
                    throw;
                }
            }
        }


        /// <summary>
        /// Gets the average rating from all feedback entries.
        /// </summary>
        public static double AverageRating
        {
            get
            {
                if (feedbacks.Count == 0)
                    return 0.0; 

                return feedbacks.Average(fb => fb.Rating);
            }
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int FeedbackID { get; set; }

        private int customerID;

        public int CustomerID
        {
            get => customerID;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("CustomerID must be positive.");
                customerID = value;
            }
        }

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

        private DateTime dateTime;

        /// <summary>
        /// Gets or sets the date and time of the feedback.
        /// </summary>
        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("DateTime cannot be in the future.");
                dateTime = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? comments;

        public string? Comments
        {
            get => comments;
            set
            {
                if (value != null)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Comments cannot be empty or whitespace.");
                    if (value.Length > 500)
                        throw new ArgumentException("Comments length cannot exceed 500 characters.");
                }
                comments = value;
            }
        }


        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Feedback class with mandatory and optional attributes.
        /// </summary>
        public Feedback(int feedbackID, int customerID, int rating, DateTime dateTime, string? comments = null)
        {
            FeedbackID = feedbackID;
            CustomerID = customerID;
            Rating = rating;
            DateTime = dateTime;
            Comments = comments;

            // Add to class extent
            feedbacks.Add(this);
            TotalFeedbacks = feedbacks.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Feedback() { }

        /// <summary>
        /// Determines whether the specified object is equal to the current Feedback.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Feedback other)
            {
                return FeedbackID == other.FeedbackID &&
                       CustomerID == other.CustomerID &&
                       Rating == other.Rating &&
                       DateTime == other.DateTime &&
                       Comments == other.Comments;
            }
            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(FeedbackID, CustomerID, Rating, DateTime, Comments);
        }


        private List<Customer> _customers = new List<Customer>();

        /// <summary>
        /// Relation implementation
        /// </summary>

        public void AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentException("Feedback cannot be null.");

            if (_customers.Contains(customer))
                return;

            _customers.Add(customer);
            if (!customer.GetFeedbacks().Contains(this))
            {
                customer.AddFeedback(this);
            }
        }

        public void ModifyCustomer(Customer oldCustomer, Customer newCustomer)
        {
            if (oldCustomer == null || newCustomer == null)
                throw new ArgumentException("Customer cannot be null.");

            int index = _customers.IndexOf(oldCustomer);
            if (index == -1)
                throw new ArgumentException("Customer not found.");

            _customers[index] = newCustomer;

            // Update reverse relationship
            if (oldCustomer.GetFeedbacks().Contains(this))
            {
                oldCustomer.RemoveFeedback(this);
            }

            if (!newCustomer.GetFeedbacks().Contains(this))
            {
                newCustomer.AddFeedback(this);
            }
        }

        public void RemoveCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentException("Customer cannot be null");

            if (!_customers.Contains(customer))
                throw new KeyNotFoundException("The specified customer is not associated with this feedback.");

            _customers.Remove(customer);

            if (customer.GetFeedbacks().Contains(this))
            {
                customer.RemoveFeedback(this);
            }
        }


        public IReadOnlyList<Customer> GetCustomers()
        {
            return _customers.AsReadOnly();
        }
    }
}
