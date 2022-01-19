using System;
using DG.Tweening;
using OPX.Scripts;
using UnityEngine;

namespace UI {
    public class HomeMenu : MenuPanel {
        [SerializeField] private RectTransform handTutorialParent;
        [SerializeField] private RectTransform noAdsParent;
        [SerializeField] private float animationXOffset = 200f;
        [SerializeField] private float animationDuration = 1f;

        protected override GameState AssignGameStateType() => GameState.Home;

        private void Start() {
            handTutorialParent.DOAnchorPosX(animationXOffset, animationDuration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);

            // if (IAPManager.Instance.DidBuyNoAds()) {
            //     noAdsParent.gameObject.SetActive(false);
            // }
        }
    }
}
