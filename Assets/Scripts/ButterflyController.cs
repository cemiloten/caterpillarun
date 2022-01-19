using DG.Tweening;
using UnityEngine;

internal class ButterflyController : MonoBehaviour {
    [SerializeField] private Transform butterflyParent;
    [SerializeField] private Transform butterflyModel;
    [SerializeField] private Transform wingR;
    [SerializeField] private Transform wingL;

    [SerializeField] private float rotationRange = 60f;
    [SerializeField] private float rotationTime = 0.2f;
    [SerializeField] private Ease rotationEase = Ease.InOutQuad;

    [SerializeField] private Vector3 movementOffset;
    [SerializeField] private float movementTime = 0.5f;
    [SerializeField] private Ease movementEase = Ease.InOutQuad;

    [SerializeField] private Ease appearEase = Ease.InOutQuad;
    [SerializeField] private float appearTimePerIncrement = 0.1f;
    [SerializeField] private float scalePerIncrement = 0.1f;
    [SerializeField] private float initialPositionOffset= 1f;

    private void Start() {
        wingR.DOLocalRotate(new Vector3(0f, rotationRange, 0f), rotationTime)
            .SetEase(rotationEase)
            .SetLoops(-1, LoopType.Yoyo);

        wingL.DOLocalRotate(new Vector3(0f, -rotationRange, 0f), rotationTime)
            .SetEase(rotationEase)
            .SetLoops(-1, LoopType.Yoyo);

        butterflyModel.DOLocalMove(movementOffset, movementTime)
            .SetEase(movementEase)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Initialize() {
        butterflyParent.localScale = Vector3.zero;
        butterflyParent.position = new Vector3(-10f, -10f, 0f);
    }

    public void Appear(Transform cocoonTransform, int bodyCount) {
        Vector3 pos = cocoonTransform.position;
        butterflyParent.position = pos - cocoonTransform.forward * initialPositionOffset;

        butterflyParent.LookAt(pos + cocoonTransform.forward * -10f);

        float appearTime = appearTimePerIncrement * bodyCount;

        butterflyParent.DOBlendableLocalMoveBy(butterflyParent.forward * bodyCount, appearTime);

        butterflyParent.DOScale(
                Vector3.one + scalePerIncrement.ToVector3() * bodyCount,
                appearTime)
            .SetEase(appearEase);
    }
}
