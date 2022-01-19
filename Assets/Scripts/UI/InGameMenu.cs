using TMPro;
using UnityEngine;

namespace UI {
    public class InGameMenu : MenuPanel {
        [SerializeField] private TextMeshProUGUI bodyCountText;

        protected override GameState AssignGameStateType() => GameState.InGame;

        private void OnEnable() {
            GameEvents.BodyCountChanged += OnBodyCountChanged;
        }

        private void OnDisable() {
            GameEvents.BodyCountChanged -= OnBodyCountChanged;
        }

        private void OnBodyCountChanged(int count) {
            UpdateText(count);
        }

        private void UpdateText(int bodyCount) {
            bodyCountText.text = bodyCount.ToString();
        }
    }
}
