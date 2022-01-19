using UnityEngine;

namespace OPX.Scripts {
    // public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {
    public class AdManager : MonoBehaviour {
//         [SerializeField] public bool testMode = true;
//         [SerializeField] public bool noAdsDebug = true;
//         [SerializeField] private double minSecondsBetweenAds = 30f;
//
//         private const string GameIdAndroid = "4512594";
//         private const string GameIdIOS = "4512594";
//
//         private const string InterstitialIdAndroid = "Interstitial_Android";
//         private const string InterstitialIdIOS = "Interstitial_iOS";
//
//         private const string BannerIdAndroid = "Banner_Android";
//         private const string BannerIdIOS = "Banner_iOS";
//
//         private string _gameId;
//         private string _bannerId;
//         private string _interstitialId;
//
//         private DateTime _lastInterstitialShowEndTime;
//
//         private void OnEnable() {
//             GameEvents.GameStateChanged += OnGameStateChanged;
//         }
//
//         private void OnDisable() {
//             GameEvents.GameStateChanged -= OnGameStateChanged;
//         }
//
//         private void Awake() {
// #if UNITY_IOS
//             _gameId = GameIdIOS;
//             _interstitialId = InterstitialIdIOS;
//             _bannerId = BannerIdIOS;
// #else
//             _gameId = GameIdAndroid;
//             _interstitialId = InterstitialIdAndroid;
//             _bannerId = BannerIdAndroid;
// #endif
//
//             // Start ad timer from the start of the game since no previous ad was shown.
//             _lastInterstitialShowEndTime = DateTime.UtcNow;
//
//             Advertisement.Initialize(_gameId, testMode, this);
//         }
//
//         private void Start() {
//             StartCoroutine(ShowBannerWhenReady());
//         }
//
//         public void OnInitializationComplete() {
//             Debug.Log("Ads initialization complete");
//         }
//
//         public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
//             Debug.Log($"Ads initialization failed: {error}, {message}");
//         }
//
//         private void OnGameStateChanged(GameState newState) {
//             if (newState == GameState.EndWin) {
//                 Debug.Log($"Trying to load ad '{_interstitialId}'.");
//                 Advertisement.Load(_interstitialId, loadListener: this);
//             }
//
//             if (newState == GameState.Home) {
//                 if (Advertisement.isInitialized == false) {
//                     return;
//                 }
//
//                 if (IAPManager.Instance.DidBuyNoAds())
//                     return;
//
//                 if (noAdsDebug)
//                     return;
//
//                 double seconds = SecondsSinceLastInterstitialEnded();
//                 if (seconds < minSecondsBetweenAds) {
//                     Debug.Log($"Not showing inter. Time since last one ended: {seconds:F1} seconds.");
//                     return;
//                 }
//
//                 Advertisement.Show(_interstitialId, showListener: this);
//             }
//         }
//
//         private double SecondsSinceLastInterstitialEnded() {
//             return (DateTime.UtcNow - _lastInterstitialShowEndTime).TotalSeconds;
//         }
//
//         public void OnUnityAdsAdLoaded(string placementId) {
//             Debug.Log($"Ad '{placementId}' was loaded during Game State '{GameManager.State}'.");
//         }
//
//         public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
//             Debug.Log($"Ad '{placementId}'  failed to load. {error}, {message}");
//         }
//
//         private IEnumerator ShowBannerWhenReady() {
//             Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
//
//             while (Advertisement.Banner.isLoaded == false) {
//                 Debug.Log("Trying to load banner");
//                 Advertisement.Banner.Load(_bannerId);
//                 yield return new WaitForSeconds(0.5f);
//             }
//
//             Debug.Log("Trying to show banner");
//
//             Advertisement.Banner.Show(_bannerId);
//         }
//
//         public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
//             Debug.LogError($"Failed show of ad {placementId}, error: {error}, message: {message}");
//         }
//
//         public void OnUnityAdsShowStart(string placementId) {
//             Debug.Log($"Started showing ad {placementId}");
//         }
//
//         public void OnUnityAdsShowClick(string placementId) {
//             Debug.Log($"Trying to showing ad {placementId}");
//         }
//
//         public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
//             Debug.Log($"Ad show completion state {showCompletionState}");
//
//             switch (showCompletionState) {
//                 case UnityAdsShowCompletionState.SKIPPED:
//                 case UnityAdsShowCompletionState.COMPLETED:
//                     if (placementId == _interstitialId) {
//                         _lastInterstitialShowEndTime = DateTime.UtcNow;
//                     }
//
//                     break;
//             }
//         }
    }
}
