using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Canvas))]
    public abstract class PopupMenu : MonoBehaviour {
        [SerializeField] private bool openAtStart;
        [SerializeField] private float openCloseDuration = 0.2f;
        [SerializeField] private Ease easing;
        [SerializeField] private Vector3 closedScale = new Vector3(1f, 0f, 1f);
        [SerializeField] private Vector3 openedScale = Vector3.one;
        [SerializeField] private Button[] closeButtons;

        private Canvas _canvas;

        public bool IsOpen { get; private set; }

        protected abstract void OnPopupOpenStart();
        protected abstract void OnPopupOpenEnd();

        protected abstract void OnPopupCloseStart();
        protected abstract void OnPopupCloseEnd();

        protected virtual void Awake() {
            _canvas = GetComponent<Canvas>();
            if (_canvas == null) {
                Debug.LogError("PopupPage ERROR: Couldn't get the Canvas component.");
            }

            if (closeButtons == null || closeButtons.Length < 1) {
                Debug.LogError("PopupPage ERROR: No close button is assigned in inspector.");
                return;
            }

            foreach (Button closeButton in closeButtons) {
                if (closeButton == null) {
                    Debug.LogError("PopupPage ERROR: A close button list element in the inspector is null. "
                                   + "Aborting close buttons listener setting.");
                    return;
                }

                closeButton.onClick.AddListener(OnClickedCloseButton);
            }

            if (openAtStart == false) {
                CloseImmediate();
            }
        }


        public void Open() {
            if (IsOpen)
                return;

            _OnPopupOpenStart();
            IsOpen = true;
        }

        private void OnClickedCloseButton() {
            Close();
        }

        public void Close() {
            if (IsOpen == false)
                return;

            _OnPopupCloseStart();
            IsOpen = false;
        }

        private void _OnPopupOpenStart() {
            _canvas.enabled = true;
            transform.DOScale(openedScale, openCloseDuration)
                .SetEase(easing)
                .OnComplete(_OnPopupOpenEnd);

            OnPopupOpenStart();
        }

        private void _OnPopupCloseStart() {
            transform.DOScale(closedScale, openCloseDuration)
                .SetEase(easing)
                .OnComplete(_OnPopupCloseEnd);

            OnPopupCloseStart();
        }

        private void _OnPopupOpenEnd() {
            OnPopupOpenEnd();
        }

        private void _OnPopupCloseEnd() {
            _canvas.enabled = false;
            OnPopupCloseEnd();
        }

        private void CloseImmediate() {
            _canvas.enabled = false;
            transform.localScale = closedScale;
            IsOpen = false;
        }
    }
}