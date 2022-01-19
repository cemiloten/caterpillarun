using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> {
    private readonly int _size;

    private readonly List<T> _poolObjects;
    private readonly List<bool> _poolState;

    private int _nextIndex;

    private event Func<T> OnCreate;
    private event Action<T> OnGet;
    private event Action<T> OnReturn;

    public ObjectPool(
        int size,
        Func<T> onCreate,
        Action<T> onGet,
        Action<T> onReturn
    ) {
        _size = size;
        _nextIndex = 0;

        if (onCreate == null) {
            Debug.LogError("Cannot create pool if OnCreate callback is null.");
            return;
        }

        OnCreate = onCreate;

        if (onGet == null) {
            Debug.LogError("Cannot create pool if OnGet callback is null.");
            return;
        }

        OnGet = onGet;

        if (onReturn == null) {
            Debug.LogError("Cannot create pool if OnReturn callback is null.");
            return;
        }

        OnReturn = onReturn;

        _poolObjects = new List<T>(size);
        for (int i = 0; i < size; i++) {
            T obj = OnCreate();
            if (obj == null) {
                Debug.LogError("ObjectPool, got null object from OnCreate callback. Aborting pool creation");
                return;
            }

            _poolObjects.Add(obj);
        }

        _poolState = new List<bool>(size);
        for (int i = 0; i < size; i++) {
            _poolState.Add(false);
        }
    }

    private void IncrementIndex() {
        ++_nextIndex;
        if (_nextIndex > _size - 1) {
            _nextIndex = 0;
        }
    }

    private void DecrementIndex() {
        --_nextIndex;
        if (_nextIndex < 0) {
            _nextIndex = _size - 1;
        }
    }

    public T Get() {
        if (_nextIndex > _size - 1) {
            Debug.LogWarning("Object pool index has reached maximum capacity, recycling oldest pooled object.");
            _nextIndex = 0;
        }

        _poolState[_nextIndex] = true;
        T obj = _poolObjects[_nextIndex];

        OnGet?.Invoke(obj);

        IncrementIndex();

        return obj;
    }

    public void Release(T obj) {
        if (FindIndex(obj, out int index) == false) {
            Debug.LogError($"Did not find object {obj} in pool, so cannot release it.");
            return;
        }

        _poolState[index] = false;
        DecrementIndex();

        OnReturn?.Invoke(obj);
    }

    private bool FindIndex(T obj, out int index) {
        for (int i = 0; i < _poolObjects.Count; i++) {
            if (!obj.Equals(_poolObjects[i]))
                continue;

            index = i;
            return true;
        }

        index = -1;
        return false;
    }
}
