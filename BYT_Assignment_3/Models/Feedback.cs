namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Feedback
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalFeedbacks = 0;

        /// <summary>
        /// Gets or sets the total number of feedback entries.
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
        /// Gets a read-only list of all feedback entries.
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
        public int FeedbackID { get; private set; }

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

        private double averageRating;

        /// <summary>
        /// Gets or sets the average rating of the feedback.
        /// </summary>
        public double AverageRating
        {
            get => averageRating;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("AverageRating must be between 1 and 5.");
                averageRating = value;
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
                if(!string.IsNullOrEmpty(value) && value.Length > 500)
                    throw new ArgumentException("Comments length cannot exceed 500 characters.");
                comments = value;
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Feedback class with mandatory and optional attributes.
        /// </summary>
        public Feedback(int feedbackID, int customerID, int rating, DateTime dateTime, double averageRating, string? comments = null)
        {
            FeedbackID = feedbackID;
            CustomerID = customerID;
            Rating = rating;
            DateTime = dateTime;
            AverageRating = averageRating;
            Comments = comments;

            // Add to class extent
            feedbacks.Add(this);
            TotalFeedbacks = feedbacks.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Feedback() { }
    }
}