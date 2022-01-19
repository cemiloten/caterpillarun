using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class CaterpillarController : MonoBehaviour {
    [SerializeField] private CaterpillarBodyPart bodyPrefab;
    [SerializeField] private Transform head;
    [SerializeField] private Transform tail;
    [SerializeField] private Transform targetTransform;

    [SerializeField] private float eatScaleValue = 1.25f;
    [SerializeField] private float eatScaleTime = 0.2f;
    [SerializeField] private float eatDelayBetweenBodyParts = 0.1f;

    [SerializeField] private float hitScaleValue = 1.25f;
    [SerializeField] private float hitScaleTime = 0.2f;
    [SerializeField] private float hitDelayBetweenBodyParts = 0.1f;


    [SerializeField] private int maxBodyCount = 64;

    private List<CaterpillarBodyPart> _bodyParts;
    private ObjectPool<CaterpillarBodyPart> _bodyPartsPool;

    private Collider _lastObstacle;
    private int _feverBodyPartAmount;

    private bool _hasHeadTween;
    private TweenerCore<Vector3, Vector3, VectorOptions> _headTween;

    public int BodyCount => _bodyParts.Count;

    private void OnTakeFromPool(CaterpillarBodyPart bodyPart) {
        _bodyParts.Add(bodyPart);
        bodyPart.Active = true;
    }

    private void OnReturnedToPool(CaterpillarBodyPart bodyPart) {
        Transform trs = bodyPart.transform;
        trs.position = new Vector3(0f, -20f, -20f);

        bodyPart.Active = false;
        bodyPart.target = null;
        bodyPart.targetTail = null;

        _bodyParts.Remove(bodyPart);
    }

    public void Initialize(int bodyCountAtStart, int feverBodyPartCount) {
        _feverBodyPartAmount = feverBodyPartCount;

        if (_bodyPartsPool == null) {
            // First call.
            _bodyPartsPool = new ObjectPool<CaterpillarBodyPart>(
                maxBodyCount,
                OnCreatePoolObject,
                OnTakeFromPool,
                OnReturnedToPool
            );

            _bodyParts = new List<CaterpillarBodyPart>(maxBodyCount);
        }
        else {
            ReleaseAllBodyParts();
        }

        Appear();

        for (int i = 0; i < bodyCountAtStart; i++) {
            AddBodyPart();
        }

        GameEvents.OnBodyCountChanged(BodyCount);
    }

    private CaterpillarBodyPart OnCreatePoolObject() {
        return Instantiate(bodyPrefab);
    }

    private void ReleaseAllBodyParts() {
        for (int i = BodyCount - 1; i >= 0; i--) {
            CaterpillarBodyPart bodyPart = _bodyParts[i];
            if (bodyPart.Active) {
                _bodyPartsPool.Release(bodyPart);
            }
        }
    }

    public void Appear() {
        head.localScale = Vector3.one;
    }

    public void Disappear() {
        if (_hasHeadTween || _headTween != null) {
            _headTween.Kill();
        }

        head.localScale = Vector3.zero;
    }

    public void DoUpdate(float caterpillarBodyFollowSpeed, float caterpillarLookAtSpeed) {
        for (int i = 0; i < BodyCount; i++) {
            CaterpillarBodyPart caterpillarBodyPart = _bodyParts[i];
            caterpillarBodyPart.DoUpdate(caterpillarBodyFollowSpeed, caterpillarLookAtSpeed);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bonus")) {
            var bonusObject = other.GetComponentInParent<BonusObject>();
            bonusObject.Disable();

            if (bonusObject.type == BonusObjectType.Classic) {
                AddBodyPart();
            }
            else if (bonusObject.type == BonusObjectType.Fever) {
                for (int i = 0; i < _feverBodyPartAmount; i++) {
                    AddBodyPart();
                }
            }

            GameEvents.OnBodyCountChanged(BodyCount);
            GameEvents.OnHitBonusObject(bonusObject);
            StartCoroutine(EatAnimation(BodyCount));
        }
        else if (other.CompareTag("Obstacle")) {
            if (other == _lastObstacle) {
                return;
            }

            _lastObstacle = other;

            OnHitObstacle();

            GameEvents.OnHitObstacle();
        }
    }

    private void AddBodyPart() {
        Transform targetBody;
        Transform targetTail;

        if (BodyCount == 0) {
            targetBody = targetTransform;
            targetTail = tail;
        }
        else {
            CaterpillarBodyPart last = _bodyParts.Last();
            targetBody = last.transform;
            targetTail = last.Tail;
        }

        CaterpillarBodyPart body = _bodyPartsPool.Get();
        body.Initialize(targetBody, targetTail);
    }

    private void OnHitObstacle() {
        int count = BodyCount;
        if (count > 0) {
            int lastIndex = count / 2;

            for (int i = BodyCount - 1; i >= lastIndex; i--) {
                CaterpillarBodyPart bodyPart = _bodyParts[i];

                EffectsManager.Instance.LostBodyFeedback(bodyPart.transform.position);

                _bodyPartsPool.Release(bodyPart);
                _bodyParts.Remove(bodyPart);
            }
        }

        GameEvents.OnBodyCountChanged(BodyCount);
        StartCoroutine(HitObstacleAnimation(BodyCount));
    }

    private IEnumerator HitObstacleAnimation(int count) {
        if (_hasHeadTween) {
            _headTween.Restart();
        }
        else {
            // Head animation
            _hasHeadTween = true;
            _headTween = head.DOScale(Vector3.one * hitScaleValue, hitScaleTime)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => { _hasHeadTween = false; });
        }

        yield return new WaitForSeconds(hitDelayBetweenBodyParts);

        // Body animations
        for (int i = 0; i < BodyCount; i++) {
            if (i >= count) {
                break;
            }

            CaterpillarBodyPart body = _bodyParts[i];
            body.ScaleAnimation(hitScaleValue, hitScaleTime);
            yield return new WaitForSeconds(hitDelayBetweenBodyParts);
        }
    }

    private IEnumerator EatAnimation(int count) {
        if (_hasHeadTween) {
            _headTween.Restart();
        }
        else {
            // Head animation
            _hasHeadTween = true;
            _headTween = head.DOScale(Vector3.one * eatScaleValue, eatScaleTime)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => { _hasHeadTween = false; });
        }

        yield return new WaitForSeconds(eatDelayBetweenBodyParts);

        // Body animations
        for (int i = 0; i < BodyCount; i++) {
            if (i >= count) {
                break;
            }

            CaterpillarBodyPart body = _bodyParts[i];
            body.ScaleAnimation(eatScaleValue, eatScaleTime);
            yield return new WaitForSeconds(eatDelayBetweenBodyParts);
        }
    }

    public void MoveTowardsCocoon() {
        CaterpillarBodyPart bodyPart = _bodyParts.Last();
        _bodyPartsPool.Release(bodyPart);
    }

    public void ShowHead() {
        head.localScale = Vector3.one;
    }

    public void HideHead() {
        head.localScale = Vector3.zero;
    }
}
