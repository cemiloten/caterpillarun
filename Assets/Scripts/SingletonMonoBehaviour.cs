using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance = null;

    public static T Instance {
        get {
            if (_instance == null) {
                Debug.LogWarning($"Instance of type {typeof(T)} is null.");
                return null;
            }

            return _instance;
        }
    }

    protected virtual void Awake() {
        if (_instance != null) {
            Debug.LogError($"Singleton instance of {typeof(T)} already exists, destroying old instance.");
            Destroy(_instance);
        }

        _instance = this as T;
    }
}