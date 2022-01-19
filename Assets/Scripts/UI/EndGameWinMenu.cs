using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class EndGameWinMenu : MenuPanel {
        [SerializeField] private Button nextLevelButton = default;
        [SerializeField] private float continueButtonAnimationScale = 1.25f;
        [SerializeField] private float continueButtonAnimationDuration = 1f;
        
        [SerializeField] private TextMeshProUGUI bodyCountText;

        protected override GameState AssignGameStateType() => GameState.EndWin;

        private void Awake() {
            // Button animation.
            nextLevelButton.transform.DOScale(Vector3.one * continueButtonAnimationScale, continueButtonAnimationDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            nextLevelButton.onClick.AddListener(OnClickNextLevel);
        }

        protected override void OnActivate() {
            bodyCountText.text = PlayerController.BodyCount.ToString();
        }

        public void OnClickNextLevel() {
            GameEvents.OnClickedNextLevelButton();
        }
    }
}