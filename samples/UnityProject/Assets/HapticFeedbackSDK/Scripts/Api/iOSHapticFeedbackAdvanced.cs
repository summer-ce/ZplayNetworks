/// <summary>
/// If you use the advanced mode you have to prepare the feedback generator you are about to use
/// This can decrease the latency but is barely noticable. If you do not use advanced mode
/// the feedback generator is prepared and triggered at the same time
/// </summary>
public class iOSHapticFeedbackAdvanced : iOSHapticFeedback
{

    protected void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Triggers one of the haptic feedbacks available on iOS.
    /// Warning: You are using the advanced mode. Make sure to prepare every feedback type before you trigger it.
    /// </summary>
    /// <param name="feedbackType">Feedback type.</param>
    public void Trigger(FeedbackType feedbackType)
    {
        TriggerFeedbackGenerator((int)feedbackType, true);
    }



    public void InstantiateFeedbackGenerator(FeedbackType feedbackType)
    {
        base.InstantiateFeedbackGenerator((int)feedbackType);
    }

    public void PrepareFeedbackGenerator(FeedbackType feedbackType)
    {
        base.PrepareFeedbackGenerator((int)feedbackType);
    }

    public void TriggerFeedbackGenerator(FeedbackType feedbackType)
    {
        Trigger(feedbackType);
    }

    public void ReleaseFeedbackGenerator(FeedbackType feedbackType)
    {
        base.ReleaseFeedbackGenerator((int)feedbackType);
    }
}