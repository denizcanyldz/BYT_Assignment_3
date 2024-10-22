public class FeedbackPersistence : Persistence<Feedback>
{
    public static void SaveFeedbacks(string filePath)
    {
        Save(filePath, Feedback.feedbacks);
    }

    public static void LoadFeedbacks(string filePath)
    {
        Feedback.feedbacks = Load(filePath);
    }
}
