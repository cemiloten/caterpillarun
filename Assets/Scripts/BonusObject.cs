using DG.Tweening;
using UnityEngine;

public enum BonusObjectType {
    Classic,
    Fever
}

public class BonusObject : MonoBehaviour {
    [SerializeField] private Transform container;
    [SerializeField] private Transform objectTransform;
    [SerializeField] public BonusObjectType type;
    
    [SerializeField] private Vector3 translation = Vector3.zero;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private int animationLoop = -1;
    [SerializeField] private Ease easing = Ease.InOutQuad;
    
    [SerializeField] private Vector3 rotation = Vector3.zero;
    [SerializeField] private float rotationDuration = 1f;

    private void Update() {
        objectTransform.Rotate(0f, rotationDuration * Time.deltaTime, 0f, Space.Self);
    }

    public void Disable() {
        container.gameObject.SetActive(false);
    }

    public void Enable() {
        container.gameObject.SetActive(true);
    }
}