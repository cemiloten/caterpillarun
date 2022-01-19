using System;
using UnityEngine;
// using UnityEngine.Purchasing;

namespace OPX.Scripts {
    // public class IAPManager : SingletonMonoBehaviour<IAPManager>, IStoreListener {
    public class IAPManager : SingletonMonoBehaviour<IAPManager>{
        // private IStoreController _storeController;
        // private IExtensionProvider _extensionProvider;
        //
        // private const string DisableAdsProductID = "com.opixel.caterpillarun.noads";
        //
        // private static bool DidBuyNoAdsPlayerPref {
        //     get => PlayerPrefs.GetInt("DidBuyNoAds", defaultValue: 0) == 1;
        //     set => PlayerPrefs.SetInt("DidBuyNoAds", value ? 1 : 0);
        // }
        //
        // private void OnEnable() {
        //     GameEvents.ClickedPurchaseNoAds += OnClickedPurchaseNoAds;
        // }
        //
        // private void OnDisable() {
        //     GameEvents.ClickedPurchaseNoAds -= OnClickedPurchaseNoAds;
        // }
        //
        // private void Start() {
        //     if (_storeController == null) {
        //         InitializePurchasing();
        //     }
        // }
        //
        // public void InitializePurchasing() {
        //     if (IsInitialized())
        //         return;
        //
        //     var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //     builder.AddProduct(DisableAdsProductID, ProductType.NonConsumable);
        //     UnityPurchasing.Initialize(this, builder);
        // }
        //
        // private bool IsInitialized() {
        //     return _storeController != null && _extensionProvider != null;
        // }
        //
        // public void OnClickedPurchaseNoAds() {
        //     if (!IsInitialized())
        //         return;
        //
        //     Product noAdsProduct = _storeController.products.WithID(DisableAdsProductID);
        //     if (noAdsProduct != null && noAdsProduct.availableToPurchase) {
        //         _storeController.InitiatePurchase(DisableAdsProductID);
        //     }
        // }
        //
        // public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        //     _storeController = controller;
        //     _extensionProvider = extensions;
        //     RestorePurchases();
        //     UpdatePlayerPref();
        // }
        //
        // private void UpdatePlayerPref() {
        //     bool result = false;
        //
        //     Product product = _storeController.products.WithID(DisableAdsProductID);
        //     if (product == null) {
        //         Debug.LogError($"ERROR: Product with ID {DisableAdsProductID} does not exist.");
        //         result = false;
        //     }
        //     else {
        //         result = product.hasReceipt;
        //     }
        //
        //     DidBuyNoAdsPlayerPref = result;
        // }
        //
        // public void OnInitializeFailed(InitializationFailureReason error) {
        //     Debug.LogError("IAP Initialize failed:" + error);
        // }
        //
        // public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e) {
        //     if (string.Equals(e.purchasedProduct.definition.id, DisableAdsProductID, StringComparison.Ordinal)) {
        //         Debug.Log($"ProcessPurchase: PASS. Product: '{e.purchasedProduct.definition.id}'");
        //         DidBuyNoAdsPlayerPref = true;
        //         GameEvents.OnBuyNoAds();
        //     }
        //
        //     return PurchaseProcessingResult.Complete;
        // }
        //
        // public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
        //     Debug.Log(
        //         $"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
        // }
        //
        // public void RestorePurchases() {
        //     Debug.Log("Trying to restore purchases.");
        //
        //     if (!IsInitialized()) {
        //         Debug.LogError("RestorePurchases FAILED. Not initialized");
        //         return;
        //     }
        //
        //     if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) {
        //         Debug.Log("RestorePurchases started...");
        //
        //         var apple = _extensionProvider.GetExtension<IAppleExtensions>();
        //         apple.RestoreTransactions((result) => {
        //             Debug.Log(
        //                 $"RestorePurchases continuing: {result}. If no further messages, no purchases available to restore.");
        //         });
        //     }
        //     else {
        //         Debug.Log($"RestorePurchases FAIL. Not supported on this platform. Current = {Application.platform}");
        //     }
        // }
        //
        // public bool DidBuyNoAds() {
        //     if (!IsInitialized()) {
        //         return DidBuyNoAdsPlayerPref;
        //     }
        //
        //     Product product = _storeController.products.WithID(DisableAdsProductID);
        //     if (product == null) {
        //         Debug.LogError($"ERROR: Product with ID {DisableAdsProductID} does not exist.");
        //         return false;
        //     }
        //
        //     return product.hasReceipt;
        // }
    }
}
