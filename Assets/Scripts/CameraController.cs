using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : SingletonMonoBehaviour<CameraController> {
    [FormerlySerializedAs("virtualCamera")] [SerializeField]
    private CinemachineVirtualCamera caterpillarVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera butterflyVirtualCamera;


    [SerializeField] private float shakeAmplitude = 5f;
    [SerializeField] private float shakeTime = 0.1f;

    private bool _isShaking;
    private float _shakeTimer;
    private CinemachineBasicMultiChannelPerlin _perlin;

    public Camera Camera { get; private set; }


    private void OnEnable() {
        GameEvents.GameStateChanged += OnGameStateChanged;
        GameEvents.GrewCocoonToMaxSize += OnGrewCocoonToMaxSize;
        GameEvents.HitObstacle += OnHitObstacle;
    }

    private void OnDisable() {
        GameEvents.HitObstacle -= OnHitObstacle;
        GameEvents.GameStateChanged -= OnGameStateChanged;
        GameEvents.GrewCocoonToMaxSize -= OnGrewCocoonToMaxSize;
    }

    protected override void Awake() {
        base.Awake();
        Camera = GetComponent<Camera>();
        _perlin = caterpillarVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnGameStateChanged(GameState state) {
        if (state == GameState.Home) {
            caterpillarVirtualCamera.Priority = 10;
            butterflyVirtualCamera.Priority = 0;
        }
    }

    private void OnGrewCocoonToMaxSize(int score) {
        caterpillarVirtualCamera.Priority = 0;
        butterflyVirtualCamera.Priority = 10;
    }

    private void Update() {
        if (!_isShaking) {
            return;
        }

        _shakeTimer += Time.deltaTime;

        if (_shakeTimer > shakeTime) {
            _isShaking = false;
            _perlin.m_AmplitudeGain = 0f;
        }
    }

    private void OnHitObstacle() {
        CameraShake();
    }

    private void CameraShake() {
        _shakeTimer = 0f;
        _isShaking = true;
        _perlin.m_AmplitudeGain = shakeAmplitude;
    }
}
