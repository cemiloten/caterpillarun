using UnityEngine;
using UnityEngine.UI;

public class NoAdsButton : MonoBehaviour {
    private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickedButton);
    }

    private void OnClickedButton() {
        GameEvents.OnClickedPurchaseNoAds();
    }
}
