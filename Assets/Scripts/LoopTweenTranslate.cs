using DG.Tweening;
using UnityEngine;

public class LoopTweenTranslate : MonoBehaviour {
    [SerializeField] private Vector3 translation = Vector3.zero;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private int animationLoop = -1;
    [SerializeField] private Ease easing = Ease.InOutQuad;

    private void Start() {
        transform.DOLocalMove(translation, animationDuration)
            .SetEase(easing)
            .SetLoops(animationLoop, LoopType.Yoyo);
    }
}