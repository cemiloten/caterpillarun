using UnityEngine;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class MenuPanel : MonoBehaviour {
        [SerializeField] private float transitionTime = 0.5f;

        private CanvasGroup _canvasGroup;

        public GameState State { get; private set; }
        public bool IsActive { get; private set; } = true; // True because all items are visible at start.

        protected abstract GameState AssignGameStateType();

        protected virtual void OnActivate() {
        }

        protected virtual void OnDeactivate() {
        }

        public void Initialize() {
            if (!TryGetComponent(out _canvasGroup)) {
                Debug.LogError("Couldn't get CanvasGroup component");
            }

            State = AssignGameStateType();
        }

        public void SetValueImmediate(bool value) {
            if (IsActive == value)
                return;

            Helpers.SetCanvasGroupValueImmediate(_canvasGroup, value);

            if (value) {
                OnActivate();
            }
            else {
                OnDeactivate();
            }

            IsActive = value;
        }

        public void SetValue(bool value) {
            if (IsActive == value)
                return;

            Helpers.SetCanvasGroupValue(_canvasGroup, value, transitionTime);

            if (value) {
                OnActivate();
            }
            else {
                OnDeactivate();
            }

            IsActive = value;
        }
    }
}