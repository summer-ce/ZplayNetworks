#if UNITY_PURCHASING
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using Assets.ZplaySDK.Scripts.Extentions;
using Assets.ZplaySDK.Scripts.Services;

namespace Assets.ZplayServices.Scripts.Purchaser
{
    public class SubscriptionController : MonoSingleton<SubscriptionController>
    {
        protected override void Awake()
        {
            base.Awake();
        }
        public void CheckSubscription(String subscriptionId,Action<Boolean> subscriptionCallback)
        {
            var subscriptionIdInfo = Retrive(subscriptionId);
            if (subscriptionIdInfo == null)
            {
                subscriptionCallback(false);
                return;
            }
            if (subscriptionIdInfo.isSubscribed() == Result.True)//Has subscribed to
            {
                if(subscriptionIdInfo.isExpired() == Result.True)//Subscription expired
                {
                    ZplayLogger.Log(String.Format("Subscription expired: '{0}'", subscriptionId));
                    subscriptionCallback(false);
                }
                else if (subscriptionIdInfo.isExpired() == Result.False)//Unexpired subscription
                {
                    ZplayLogger.Log(String.Format("Subscription not expired: '{0}'", subscriptionId));
                    subscriptionCallback(true);
                }
            }
            else
            {
                ZplayLogger.Log(String.Format("You have not purchased a subscription: '{0}'", subscriptionId));
                subscriptionCallback(false);
            }
        }

        private SubscriptionInfo Retrive(String subscriptionId)
        {
            if (!PurchaserManager.Instance.IsInitialized())
            {
                return null;
            }
            var subscriptionproduct = PurchaserManager.StoreController.products.all.FirstOrDefault(product => product.definition.type == ProductType.Subscription && product.definition.id == subscriptionId);
            if (subscriptionproduct == null)
            {
                Debug.LogError(String.Format("IAP Failed to finf aa subscription{0}!", subscriptionId));
                return null;
            }
            if (subscriptionproduct.receipt == null || !checkIfProductIsAvailableForSubscriptionManager(subscriptionproduct.receipt))
            {
                Debug.LogError(String.Format("IAP subscriptionproduct.receipt is null And checkIfProductIsAvailableForSubscriptionManager is ture === {0}{1}", subscriptionproduct.receipt, !checkIfProductIsAvailableForSubscriptionManager(subscriptionproduct.receipt)));
                return null;
            }
            var introductoryInfoDict = PurchaserManager.StoreExtensionProvider.GetExtension<IAppleExtensions>().GetIntroductoryPriceDictionary();
            var introJson = introductoryInfoDict == null || !introductoryInfoDict.ContainsKey(subscriptionproduct.definition.storeSpecificId) ? null : introductoryInfoDict[subscriptionproduct.definition.storeSpecificId];
            try
            {
                return new SubscriptionManager(subscriptionproduct, introJson).getSubscriptionInfo();
            }
            catch (Exception e)
            {
                Debug.LogError("IAP Exception received while getting SubscriptionInfo:" + e.Message);
                return null;
            }
        }

        private Boolean checkIfProductIsAvailableForSubscriptionManager(string receipt)
        {
            try
            {
                var receiptWrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
                if (!receiptWrapper.ContainsKey("Store") || !receiptWrapper.ContainsKey("Payload"))
                {
                    Debug.Log("IAP The product receipt does not contain enough information");
                    return false;
                }
                var store = (string)receiptWrapper["Store"];
                var payload = (string)receiptWrapper["Payload"];
                Debug.Log(String.Format("IAP store and payload === {0}{1}", store, payload));
                if (payload != null)
                {
                    switch (store)
                    {
                        case GooglePlay.Name:
                            {
                                var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                                if (!payload_wrapper.ContainsKey("json"))
                                {
                                    Debug.Log("IAP The product receipt does not contain enough information, the 'json' field is missing");
                                    return false;
                                }
                                var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                                if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload"))
                                {
                                    Debug.Log("IAP The product receipt does not contain enough information, the 'developerPayload' field is missing");
                                    return false;
                                }
                                var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                                var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                                if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial"))
                                {
                                    Debug.Log("IAP The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                                    return false;
                                }
                                return true;
                            }
                        case AppleAppStore.Name:
                        case AmazonApps.Name:
                        case MacAppStore.Name:
                            {
                                return true;
                            }
                        default:
                            {
                                return false;
                            }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError("IAP Exception received while checking subscription availability:" + e.Message);
                return false;
            }
        }
    }
}
#endif
