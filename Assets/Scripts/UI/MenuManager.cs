using System.Collections;
using UnityEngine;

namespace UI {
    public class MenuManager : MonoBehaviour {
        private MenuPanel[] _panels;

        private void Awake() {
            _panels = GetComponentsInChildren<MenuPanel>();
            foreach (MenuPanel menuPanel in _panels) {
                menuPanel.Initialize();
                menuPanel.SetValueImmediate(false);
            }
        }

        private void OnEnable() {
            GameEvents.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable() {
            GameEvents.GameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newState) {
            SetPanel(newState);
        }

        private void SetPanel(GameState state) {
            for (int i = 0; i < _panels.Length; ++i) {
                MenuPanel menuPanel = _panels[i];
                menuPanel.SetValue(menuPanel.State == state);
            }
        }
    }
}