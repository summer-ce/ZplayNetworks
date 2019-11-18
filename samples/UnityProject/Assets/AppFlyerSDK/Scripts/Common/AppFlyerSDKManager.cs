using System.Collections.Generic;
using Assets.ZplaySDK.Scripts.Services.Scriptable;
using Assets.ZplaySDK.Scripts.Extentions;

public class AppFlyerSDKManager : MonoSingleton<AppFlyerSDKManager>
{
    protected override void Awake()
    {
        base.Awake();
        gameObject.name = "AppsFlyerTrackerCallbacks";
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public void InitializeAppFlyer()
    {
        AppsFlyer.setAppsFlyerKey(GameSettings.AnalysisAppflyerDevKey);
#if UNITY_IPHONE
        AppsFlyer.setAppID(AnalysisSettings.AnalysisAppflyerIOSAppId);
        AppsFlyer.trackAppLaunch();
#elif UNITY_ANDROID
        AppsFlyer.setAppID(GameSettings.AnalysisAppflyerAndroidPackageName);
        AppsFlyer.init(GameSettings.AnalysisAppflyerDevKey, gameObject.name);
#endif
        gameObject.AddComponent<AppsFlyerTrackerCallbacks>();
    }
    public void AppFlyerLogEvent(string eventName, Dictionary<string, string> eventValues)
    {
        AppsFlyer.trackRichEvent(eventName,eventValues);
    }
}
