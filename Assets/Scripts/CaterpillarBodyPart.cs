using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class CaterpillarBodyPart : MonoBehaviour {
    [SerializeField] public Transform target;
    [SerializeField] public Transform targetTail;
    [SerializeField] private Transform tail;

    public Transform Tail => tail;

    private bool _hasTween;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

    public bool Active { get; set; }

    public void Initialize(Transform target, Transform targetTail) {
        this.target = target;
        this.targetTail = targetTail;

        Transform trs = transform;
        trs.position = targetTail.position;
        transform.LookAt(target, target.up);
    }

    public void DoUpdate(float followSpeed, float lookAtSpeed) {
        if (Active == false) {
            return;
        }

        Transform trs = transform;
        Vector3 transformPos = trs.position;
        Vector3 targetTailPos = targetTail.position;

        UpdateLookAt(transformPos, target.position, lookAtSpeed);

        trs.position = Helpers.Damp(transformPos, targetTailPos, followSpeed, Time.deltaTime);
    }


    public void ScaleAnimation(float scaleValue, float scaleTime) {
        if (_hasTween) {
            _tween.Restart();
            return;
        }

        _hasTween = true;
        _tween = transform.DOScale(Vector3.one * scaleValue, scaleTime)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => { _hasTween = false; });
    }

    public void UpdateLookAt(Vector3 transformPosition, Vector3 targetPosition, float lookAtSpeed) {
        transform.LookAt(target, target.up);
    }
}
