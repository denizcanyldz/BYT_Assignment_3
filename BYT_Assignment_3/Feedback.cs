public class Feedback
{
    public static List<Feedback> feedbacks = new List<Feedback>();

    public int FeedbackID{get;set;}
    public Customer Customer{get;set;}
    public int Rating{get;set;}
    public string Comments{get;set;}
    public DateTime Date{get;set;}

    public static double AverageRating(){
        if(feedbacks.Count == 0){
            return 0;
        }
        return feedbacks.Average(f => f.Rating);
    }
    public Feedback(int feedbackID, Customer customer, int rating, string comments, DateTime date)
    {
        FeedbackID = feedbackID;
        Customer = customer;
        Rating = rating;
        Comments = comments;
        Date = date;

        feedbacks.Add(this);
    }
}
