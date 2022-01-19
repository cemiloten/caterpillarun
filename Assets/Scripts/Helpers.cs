using DG.Tweening;
using UnityEngine;

public static class Helpers {
    public static void SetCanvasGroupValueImmediate(CanvasGroup canvasGroup, bool value) {
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
        canvasGroup.alpha = value ? 1f : 0f;
    }

    public static void SetCanvasGroupValue(CanvasGroup canvasGroup, bool activate, float transitionTime) {
        // Don't allow interaction during transition.
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(activate ? 1f : 0f, transitionTime)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => {
                if (activate) {
                    // Allow back interaction if we want to activate the CanvasGroup.
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }
            });
    }

    public static Quaternion Damp(Quaternion source, Quaternion target, float smoothing, float dt) {
        return Quaternion.LerpUnclamped(source, target, 1f - Mathf.Pow(smoothing, dt));
    }

    public static Vector3 Damp(Vector3 source, Vector3 target, float smoothing, float dt) {
        return Vector3.LerpUnclamped(source, target, 1f - Mathf.Pow(smoothing, dt));
    }
}

public static class ExtensionMethods {
    public static Vector3 ToVector3(this float f) => new Vector3(f, f, f);

    public static void SetPositionX(this Transform transform, float x) {
        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }

    public static void SetPositionY(this Transform transform, float y) {
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    public static void SetPositionZ(this Transform transform, float z) {
        Vector3 pos = transform.position;
        pos.z = z;
        transform.position = pos;
    }
}