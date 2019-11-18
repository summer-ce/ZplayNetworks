using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.ZplaySDK.Scripts.Services;
using UnityEngine.UI;
using System;

public class SampleDemo : MonoBehaviour
{
    [SerializeField]
    private GameObject panel_1;
    [SerializeField]
    private GameObject panel_2;
    [SerializeField]
    private String _timerFormat;
    [SerializeField]
    private Text unbiasedTimeText;

    private Boolean isOpenfeedback;
    private void Start()
    {
        // ZplayLogger.SetDebug(true);
        InitializationStatistics();
        Debug.Log(string.Format("{0}{1}", "Zplay", "===iii==="));
    }

    // Update is called once per frame
    private void Update()
    {
        CheckUnbiasedTime();
    }
    private void OnGUI()
    {
        if (isOpenfeedback)
        {
            for (int i = 0; i < 7; i++)
            {
                if (GUI.Button(new Rect(0, i * 60, 300, 50), "Trigger " + (FeedbackType)i))
                {
                    HapticFeedbackManager.Instance.HapticFeedbackTrigger((FeedbackType)i);
                }
            }
        }
    }
    private void InitializationStatistics()
    {
        FlurrySDKManager.Instance.InitializeFlurry();
        AppFlyerSDKManager.Instance.InitializeAppFlyer();
        TalkingDataSDKManager.Instance.InitializeTalkingData();
        FlurrySDKManager.Instance.SetFlurryLister();

        StatisticEvent();
    }
    private void StatisticEvent()
    {
        string key = "Statistic_Event";
        Dictionary<string, string> dic = new Dictionary<string, string>
        {
            {"Eevnt","Value"}
        };
        FlurrySDKManager.Instance.FlurryLogEvent(key, dic);
        AppFlyerSDKManager.Instance.AppFlyerLogEvent(key, dic);
        TalkingDataSDKManager.Instance.TalkingDataLogEvent(key, new Dictionary<string, object> { { "Eevnt", "Value" } });
    }

    public void CheckOnePermissions()
    {
#if UNITY_ANDROID
        if (CheckPermissions.Check_Read_Phone_State_Permission)
        {
            RuntimePermissionsHandler.ShowAppSettings();
        }
        else
        {
            RuntimePermissionsHandler.GrantPermission(AndroidPermission.READ_PHONE_STATE, (permission, result) =>
            {
                if (permission == AndroidPermission.READ_PHONE_STATE && result)
                {
                    ZplayLogger.Log("Check One Permissions Phone authorization successful");
                }
            });
        }
#endif
    }
    public void CheckTwoPermissions()
    {
#if UNITY_ANDROID
        if (CheckPermissions.Check_Write_External_Storage_Prmission && CheckPermissions.Check_Access_Coarse_Location)
        {
            RuntimePermissionsHandler.ShowAppSettings();
        }
        else
        {
            AndroidPermission[] permissionArr = { AndroidPermission.WRITE_EXTERNAL_STORAGE, AndroidPermission.ACCESS_COARSE_LOCATION };
            RuntimePermissionsHandler.GrantPermissions(permissionArr, result =>
            {
                if (result)
                {
                    ZplayLogger.Log("Check Two Permissions Phone authorization successful");
                }
            });
        }
#endif
    }

    public void AddUnbiasedTime()
    {
        GameData.SampleDemo.NextTime = UnbiasedTime.Instance.Now().AddSeconds(18000f);
        GameData.SampleDemo.OpenTime = true;
        unbiasedTimeText.gameObject.SetActive(true);
    }
    private void CheckUnbiasedTime()
    {
        if (GameData.SampleDemo.OpenTime)
        {
            var delta = GameData.SampleDemo.NextTime - UnbiasedTime.Instance.Now();
            unbiasedTimeText.text = String.Format(_timerFormat, delta.Hours, delta.Minutes, delta.Seconds);
            if (delta.TotalSeconds <= 0)
            {
                GameData.SampleDemo.OpenTime = false;
                unbiasedTimeText.gameObject.SetActive(false);
            }
        }
    }
    public void OpenFeedback()
    {
        panel_1.SetActive(false);
        panel_2.SetActive(true);
        isOpenfeedback = true;
    }
    public void CloseFeedback()
    {
        panel_1.SetActive(true);
        panel_2.SetActive(false);
        isOpenfeedback = false;
    }
}
