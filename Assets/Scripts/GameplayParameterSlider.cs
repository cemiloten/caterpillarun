using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayParameterSlider : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI minValueText;
    [SerializeField] private TextMeshProUGUI maxValueText;
    [SerializeField] private TextMeshProUGUI currentValueText;

    [SerializeField] private GameplayParameterType parameterType;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    private void Awake() {
        minValueText.text = minValue.ToString(CultureInfo.InvariantCulture);
        maxValueText.text = maxValue.ToString(CultureInfo.InvariantCulture);

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void Start() {
        float value = GameplayParameterController.Instance.GetValue(parameterType);
        slider.value = value;
        UpdateCurrentValueText(value);
    }

    private void OnValueChanged(float value) {
        UpdateCurrentValueText(value);
        GameplayParameterController.Instance.OnParameterChanged(parameterType, value);
    }

    private void UpdateCurrentValueText(float value) {
        currentValueText.text = value.ToString(CultureInfo.InvariantCulture);
    }
}