using System;
using UnityEngine;

public enum GameplayParameterType {
    MovementSpeed,
    RotationSpeed,
}

public class GameplayParameterController : SingletonMonoBehaviour<GameplayParameterController> {
    public float MovementSpeed {
        get => PlayerPrefs.GetFloat("MovementSpeed", defaultValue: 5f);
        set => PlayerPrefs.SetFloat("MovementSpeed", value);
    }

    public float RotationSpeed {
        get => PlayerPrefs.GetFloat("RotationSpeed ", defaultValue: 90f);
        set => PlayerPrefs.SetFloat("RotationSpeed ", value);
    }

    private void Start() {
        PlayerController.Instance.movementSpeed = MovementSpeed;
        PlayerController.Instance.rotationSensitivity = RotationSpeed;
    }

    public float GetValue(GameplayParameterType parameterType) {
        switch (parameterType) {
            case GameplayParameterType.MovementSpeed:
                return MovementSpeed;
            case GameplayParameterType.RotationSpeed:
                return RotationSpeed;
            default:
                throw new ArgumentOutOfRangeException(nameof(parameterType), parameterType, null);
        }
    }

    public void OnParameterChanged(GameplayParameterType parameterType, float value) {
        switch (parameterType) {
            case GameplayParameterType.MovementSpeed:
                MovementSpeed = value;
                PlayerController.Instance.movementSpeed = value;
                break;

            case GameplayParameterType.RotationSpeed:
                RotationSpeed = value;
                PlayerController.Instance.rotationSensitivity = value;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(parameterType), parameterType, null);
        }
    }
}