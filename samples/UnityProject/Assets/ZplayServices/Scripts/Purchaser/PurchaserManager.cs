#if UNITY_PURCHASING
using System;
using UnityEngine;
using UnityEngine.Purchasing;
using Assets.ZplaySDK.Scripts.Extentions;
using Assets.ZplaySDK.Scripts.Services.Scriptable;
using Assets.ZplaySDK.Scripts.Services;

namespace Assets.ZplayServices.Scripts.Purchaser
{
    public class PurchaserManager : MonoSingleton<PurchaserManager>, IStoreListener
    {
        //The object associated with the IAP component, m_Controller, holds the item information
        public static IStoreController StoreController { get; private set; }
        public static IExtensionProvider StoreExtensionProvider { get; private set; }
        private String _inAppSource;

        // The call back
        public static Action<Boolean> Initialized;
        public static event Action<String> PurchaseStarted;
        public static event Action<Product, String> PurchaseSucceeded;
        public static event Action<Product, PurchaseFailureReason> PurchaseFailed;
        protected override void Awake()
        {
            base.Awake();
        }
        public void Initialize()
        {
            if (StoreController == null)
            {
                InitializePurchasing();
            }
        }

        private void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }
            var module = StandardPurchasingModule.Instance();
            var builder = ConfigurationBuilder.Instance(module);

            var consumableList = GameSettings.ConsumableList;
            var nonConsumagleList = GameSettings.NonConsumagleList;
            var subscriptionList = GameSettings.SubscriptionList;

            if (consumableList != null && consumableList.Count > 0)
            {
                foreach (var consumableId in consumableList)
                {
                    builder.AddProduct(consumableId, ProductType.Consumable);
                }
            }
            if (nonConsumagleList != null && nonConsumagleList.Count > 0)
            {
                foreach (var nonConsumagleId in nonConsumagleList)
                {
                    builder.AddProduct(nonConsumagleId, ProductType.NonConsumable);
                }
            }
            if (subscriptionList != null && subscriptionList.Count > 0)
            {
                foreach (var subscriptionId in subscriptionList)
                {
                    builder.AddProduct(subscriptionId, ProductType.Subscription);
                }
            }

            // start initalized
            UnityPurchasing.Initialize(this, builder);
        }


        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            ZplayLogger.Log("IAP Initialization successful");
            StoreController = controller;
            StoreExtensionProvider = extensions;
            if (Initialized != null) Initialized(true);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            ZplayLogger.Log("OnInitializeFailed InitializationFailureReason:" + error);
            if (Initialized != null) Initialized(false);
        }
        public Boolean IsInitialized()
        {
            return StoreController != null && StoreExtensionProvider != null;
        }
        public void BuyProductId(String productId, String source = "Shop") //source should be provided in a better way
        {
            _inAppSource = source;
            if (IsInitialized())
            {
                var product = StoreController.products.WithStoreSpecificID(productId);
                if (product != null)
                {
                    if (product.availableToPurchase)
                    {
                        ZplayLogger.Log(String.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                        if (PurchaseStarted != null) PurchaseStarted(productId);
                        StoreController.InitiatePurchase(product);
                    }
                    else
                    {
                        ZplayLogger.LogError(String.Format("Unable to purchase product with id '{0}'(product is unavailable for purchase)", productId));
                    }
                }
                else
                {
                    ZplayLogger.LogError(String.Format("Unable to purchase product with is '{0}'(product no found)", productId));
                }
            }
            else
            {
                ZplayLogger.Log("Unable to purchase product (system isn't initialized)");
            }
        }

        public Product OnProductPrice(string productId)
        {
            if (IsInitialized())
            {
                var product = StoreController.products.WithID(productId);
                if (product != null)
                {
                    return product;
                }
                else
                {
                    ZplayLogger.LogError(String.Format("Unable to purchase product with is '{0}'(product no found)", productId));
                    return null;
                }
            }
            else
            {
                ZplayLogger.Log("Unable to purchase product (system isn't initialized)");
                return null;
            }
        }

        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases(String productId, String source = "Shop")
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                ZplayLogger.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }
            _inAppSource = source;
            // If we are running on an Apple device ... 
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                ZplayLogger.Log("RestorePurchases started ...");
                if (PurchaseStarted != null) PurchaseStarted(productId);
                // Fetch the Apple store-specific subsystem.
                var apple = StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) =>
                {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                ZplayLogger.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                ZplayLogger.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            ZplayLogger.Log(String.Format("ProcessPurchase: PASS. Product: '{0}'',{1}'", args.purchasedProduct.definition.id, args.purchasedProduct.definition.type));
            try
            {
                if (PurchaseSucceeded != null) PurchaseSucceeded(args.purchasedProduct, _inAppSource);
            }
            catch (Exception e)
            {
                ZplayLogger.LogError("Exception: " + e.Message + "\n" + e.StackTrace);
            }
            _inAppSource = null;
            return PurchaseProcessingResult.Complete;
        }
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            ZplayLogger.Log(String.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
            if (PurchaseFailed != null) PurchaseFailed(product, failureReason);
            _inAppSource = null;
        }
    }
}
#endif
