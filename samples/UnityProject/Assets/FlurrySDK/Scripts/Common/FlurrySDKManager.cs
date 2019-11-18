using System;
using System.Collections.Generic;
using FlurrySDK;
using Assets.ZplaySDK.Scripts.Services.Scriptable;
using Assets.ZplaySDK.Scripts.Extentions;
using Assets.ZplaySDK.Scripts.Services;

public class FlurrySDKManager : MonoSingleton<FlurrySDKManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public void InitializeFlurry()
    {
        new Flurry.Builder()
            .WithCrashReporting(true)
            .WithLogEnabled(true)
            .WithLogLevel(Flurry.LogLevel.VERBOSE)
            .WithMessaging(true)
#if UNITY_ANDROID
            .Build(GameSettings.AnalysisFlurryAndroidKey);
#elif UNITY_IPHONE || UNITY_IOS
        .Build(AnalysisSettings.AnalysisFlurryIOSKey);
#endif
    }

    public void SetFlurryLister()
    {
        Flurry.SetMessagingListener(new FlurryListener());
    }

    /// <summary>
    /// <param name="eventId">Event identifier.</param>
    /// <param name="timed">If set to <c>true</c> to log timed event.</param>
    /// </summary>
    public void FlurryLogEvent(string eventId, bool timed = false)
    {
        Flurry.LogEvent(eventId,timed);
    }

    /// <summary>
    /// Log a timed event with parameters.
    /// </summary>
    /// <returns>The event recording status.</returns>
    /// <param name="eventId">Event identifier.</param>
    /// <param name="parameters">Event parameters.</param>
    /// <param name="timed">If set to <c>true</c> to log timed event.</param>
    public void FlurryLogEvent(string eventId, Dictionary<string, string> parameters, bool timed = false)
    {
        Flurry.LogEvent(eventId, parameters, timed);
    }

    /// <summary>
    /// End a timed event.Only up to 10 unique parameters total can be passed for an event,
    /// including those passed when the event was initiated.
    /// </summary>
    /// <param name="eventId">Event identifier.</param>
    /// <param name="parameters">Event parameters.</param>
    public void FlurryEndTimedEvent(string eventId, Dictionary<string, string> parameters)
    {
        if (parameters != null && parameters.Count > 0)
            Flurry.EndTimedEvent(eventId, parameters);
        else
            Flurry.EndTimedEvent(eventId);
    }
}

public class FlurryListener : Flurry.IFlurryMessagingListener
{
    public bool OnNotificationReceived(Flurry.FlurryMessage message)
    {
        ZplayLogger.Log("Flurry Messaging Notification Received: " + message.Title);
        return false;
    }

    // If you would like to handle the notification yourself, return true to notify Flurry
    // you've handled it, and Flurry will not launch the app or "click_action" activity.
    public bool OnNotificationClicked(Flurry.FlurryMessage message)
    {
        ZplayLogger.Log("Flurry Messaging Notification Clicked: " + message.Title);
        return false;
    }

    public void OnNotificationCancelled(Flurry.FlurryMessage message)
    {
        ZplayLogger.Log("Flurry Messaging Notification Cancelled: " + message.Title);
    }

    public void OnTokenRefresh(string token)
    {
        ZplayLogger.Log("Flurry Messaging Token Refresh: " + token);
    }

    public void OnNonFlurryNotificationReceived(IDisposable nonFlurryMessage)
    {
        ZplayLogger.Log("Flurry Messaging Non-Flurry Notification.");
    }
}
