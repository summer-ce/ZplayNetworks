using System.Collections.Generic;
using Assets.ZplaySDK.Scripts.Extentions;
using Assets.ZplaySDK.Scripts.Services.Scriptable;

public class TalkingDataSDKManager : MonoSingleton<TalkingDataSDKManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public void InitializeTalkingData()
    {
        TalkingDataGA.OnStart(GameSettings.AnalysisTalkingDataAppId, GameSettings.AnalysisTalkingDataChannelId);
    }
    public void TalkingDataLogEvent(string eventId,Dictionary<string,object> parameters)
    {
        TalkingDataGA.OnEvent(eventId,parameters);
    }
}
