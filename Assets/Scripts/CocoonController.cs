using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class CocoonController : MonoBehaviour {
    [SerializeField] private Transform modelParent;
    [SerializeField] private float scaleAddition = 1f;
    
    private bool _hasTween;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

    public Transform CocoonTransform => modelParent.transform;

    public void ScaleUp() {
       Vector3 scl = modelParent.localScale;
       modelParent.localScale = scl + scaleAddition.ToVector3();
    }

    public void Appear(Vector3 position, Transform lookAtTarget) {
        Transform trs = modelParent.transform;
        trs.position = position;
        trs.LookAt(lookAtTarget);
    }

    public void Initialize() {
        modelParent.localScale = Vector3.one;
        modelParent.position = new Vector3(0f, -10f, 0f);
    }

    public void Disappear() {
        modelParent.transform.localScale = Vector3.zero;
    }
}