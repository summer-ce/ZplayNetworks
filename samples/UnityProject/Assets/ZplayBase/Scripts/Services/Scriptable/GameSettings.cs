using Assets.Scripts.Extentions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ZplaySDK.Scripts.Services.Scriptable
{
    [ExecuteInEditMode]
    public class GameSettings : SingletonResourcesAsset<GameSettings>
    {
        //The following is the three-party statistical id or key parameter
        [Header("Flurry SDK")]
        [SerializeField]
        private string FLURRY_ANDROID_KEY;
        [SerializeField]
        private string FLURRY_IOS_KEY;
        [Header("AppFlyer")]
        [SerializeField]
        private string APPFLYER_DEV_KEY;
        [SerializeField]
        private string APPFLYER_IOS_APP_ID;
        [SerializeField]
        private string APPFLYER_ANDROID_PACKAGE_NAME;
        [Header("TalkingData")]
        [SerializeField]
        private string TALKINGDATA_APP_ID;
        [SerializeField]
        private string TALKINGDATA_CHANNEL_ID = "your_channel_id";

        //The following are payment list parameters, Consumable: Consumables List, NonConsumable: Non-Consumables List, Subscription Subscription List
        [Header("Consumable")]
        [SerializeField]
        private List<string> consumableList;
        [Header("NonConsumable")]
        [SerializeField]
        private List<string> nonConsumagleList;
        [Header("Subscription")]
        [SerializeField]
        private List<string> subscriptionList;

        public static string AnalysisFlurryAndroidKey
        {
            get { return Instance.FLURRY_ANDROID_KEY; }
        }
        public static string AnalysisFlurryIOSKey
        {
            get { return Instance.FLURRY_IOS_KEY; }
        }
        public static string AnalysisAppflyerDevKey
        {
            get { return Instance.APPFLYER_DEV_KEY; }
        }
        public static string AnalysisAppflyerIOSAppId
        {
            get { return Instance.APPFLYER_IOS_APP_ID; }
        }
        public static string AnalysisAppflyerAndroidPackageName
        {
            get { return Instance.APPFLYER_ANDROID_PACKAGE_NAME; }
        }
        public static string AnalysisTalkingDataAppId
        {
            get { return Instance.TALKINGDATA_APP_ID; }
        }
        public static string AnalysisTalkingDataChannelId
        {
            get { return Instance.TALKINGDATA_CHANNEL_ID; }
        }

        public static List<string> ConsumableList
        {
            get { return Instance.consumableList; }
        }
        public static List<string> NonConsumagleList
        {
            get { return Instance.nonConsumagleList; }
        }
        public static List<string> SubscriptionList
        {
            get { return Instance.subscriptionList; }
        }
    }
}
