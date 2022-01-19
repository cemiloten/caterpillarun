using UI;
using UnityEngine;
using UnityEngine.UI;

public class OpenSettingsButton : MonoBehaviour {
    [SerializeField] private PopupMenu settingsMenu;
    
    private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings() {
        settingsMenu.Open();
    }
}