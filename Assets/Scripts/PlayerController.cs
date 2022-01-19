using System.Collections;
using UnityEngine;

public enum PlayState {
    Climbing,
    Cocoon
}

public class PlayerController : SingletonMonoBehaviour<PlayerController> {
    [SerializeField] private CaterpillarController caterpillarController;
    [SerializeField] private CocoonController cocoonController;
    [SerializeField] private ButterflyController butterflyController;

    [SerializeField] private Transform caterpillarBase;
    [SerializeField] private Transform caterpillarHeadTransform;
    [SerializeField] private Transform caterpillarParentTransform;

    // [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private CustomJoystick joystick;

    [Header("Level")]
    [SerializeField] private float endLevelHeight = 90f;

    [Header("Branch movement")]
    [SerializeField] public float movementSpeed = 10f;
    [SerializeField] private float onHitWaitTime = 1f;

    [Header("Fever")]
    [SerializeField] public float feverTime = 2f;
    [SerializeField] public float feverSpeedMultiplier = 2f;
    [SerializeField] public int feverBodyPartAmount = 5;

    [Header("Rotation around branch")]
    [SerializeField] public float rotationSensitivity = 10f;

    [Header("Head turning")]
    [SerializeField] public float headAngleRange = 90f;

    [Header("Body")]
    [SerializeField] public int bodyCountAtStart = 3;
    [SerializeField] private float caterpillarBodyFollowSpeed = 8f;
    [SerializeField] private float caterpillarLookAtSpeed;

    [Header("Cocoon")]
    [SerializeField] private float cocoonPositionOffset = 0.25f;
    [SerializeField] private float delayBetweenTwoCocoonGrows = 0.05f;

    private PlayState _playState;

    public static int BodyCount;
    private int _currentBodyCount;

    private bool _fingerDown;

    private Quaternion _minHeadRotation;
    private Quaternion _maxHeadRotation;
    private bool _shouldMove;

    private bool _fever;
    private float _feverTimer;

    private void OnEnable() {
        GameEvents.JoystickDown += OnFingerDown;
        GameEvents.JoystickUp -= OnFingerUp;
        GameEvents.GameStateChanged += OnGameStateChanged;
        GameEvents.HitBonusObject += OnHitBonusObject;
        GameEvents.HitObstacle += OnHitObstacle;
    }

    private void OnDisable() {
        GameEvents.JoystickDown += OnFingerDown;
        GameEvents.JoystickUp -= OnFingerUp;
        GameEvents.GameStateChanged -= OnGameStateChanged;
        GameEvents.HitObstacle -= OnHitObstacle;
        GameEvents.HitBonusObject -= OnHitBonusObject;
    }

    protected override void Awake() {
        base.Awake();
        _minHeadRotation = Quaternion.Euler(0f, -headAngleRange, 0f);
        _maxHeadRotation = Quaternion.Euler(0f, headAngleRange, 0f);
    }

    private void OnFingerDown() {
        // if (Advertisement.isShowing)
        // return;

        if (GameManager.State == GameState.Home) {
            GameEvents.OnFirstInput();
        }

        _fingerDown = true;
    }

    private void OnFingerUp() {
        _fingerDown = false;
    }

    private void OnGameStateChanged(GameState state) {
        if (state == GameState.Home) {
            Initialize();
        }
    }

    private void OnHitObstacle() {
        _shouldMove = false;
        StartCoroutine(WaitToMove());
    }

    private void OnHitBonusObject(BonusObject bonusObject) {
        if (bonusObject.type == BonusObjectType.Fever) {
            _fever = true;
            _feverTimer = feverTime;
        }
    }

    private IEnumerator WaitToMove() {
        yield return new WaitForSeconds(onHitWaitTime);
        _shouldMove = true;
    }


    private void Initialize() {
        caterpillarBase.transform.position = Vector3.zero;

        caterpillarController.Initialize(bodyCountAtStart, feverBodyPartAmount);
        cocoonController.Initialize();
        butterflyController.Initialize();

        _shouldMove = true;
        _fever = false;
        _feverTimer = 0f;
        _playState = PlayState.Climbing;
    }

    private void Update() {
        if (GameManager.State != GameState.InGame)
            return;

        if (caterpillarBase.position.y > endLevelHeight && _playState == PlayState.Climbing) {
            InitializeCocoonState();
            GameEvents.OnReachedEndOfTree();
        }

        if (!_shouldMove)
            return;

        if (_playState != PlayState.Climbing)
            return;

        // Update fever timer
        if (_fever) {
            _feverTimer -= Time.deltaTime;

            if (_feverTimer < 0f) {
                _fever = false;
            }
        }

        // Update body
        caterpillarController.DoUpdate(caterpillarBodyFollowSpeed, caterpillarLookAtSpeed);

        // Move up
        float speed = movementSpeed * Time.deltaTime;
        if (_fever) {
            speed *= feverSpeedMultiplier;
        }

        caterpillarBase.Translate(Vector3.up * speed);

        // Move left/right
        float x = joystick.Horizontal;
        UpdateCaterpillarRotation(-x, caterpillarParentTransform);
        UpdateHeadRotation(x, caterpillarHeadTransform);
    }

    private void InitializeCocoonState() {
        _playState = PlayState.Cocoon;

        Transform trs = caterpillarController.transform;
        cocoonController.Appear(trs.position + trs.forward * cocoonPositionOffset, caterpillarBase);

        caterpillarController.Disappear();

        BodyCount = caterpillarController.BodyCount;
        _currentBodyCount = BodyCount;

        StartCoroutine(GrowCocoonAuto());
    }

    private IEnumerator GrowCocoonAuto() {
        for (int i = 0; i < BodyCount; i++) {
            GrowCocoon();
            yield return new WaitForSeconds(delayBetweenTwoCocoonGrows);
        }

        GrewCocoonToMaxSize();
    }

    private void UpdateHeadRotation(float joystickX, Transform headTransform) {
        float x = joystickX * 0.5f + 0.5f; // Rescale value from [-1, 1] to [0, 1]
        headTransform.localRotation = Quaternion.Lerp(_minHeadRotation, _maxHeadRotation, x);
    }

    private void UpdateCaterpillarRotation(float fingerDelta, Transform parentTransform) {
        float delta = fingerDelta * rotationSensitivity * Time.deltaTime;
        parentTransform.Rotate(Vector3.up, delta);
    }

    private void GrowCocoon() {
        if (_currentBodyCount <= 0) {
            GrewCocoonToMaxSize();
            return;
        }

        cocoonController.ScaleUp();
        caterpillarController.MoveTowardsCocoon();

        --_currentBodyCount;

        GameEvents.OnGrewCocoonIncrement();
        GameEvents.OnBodyCountChanged(_currentBodyCount);
    }

    private void GrewCocoonToMaxSize() {
        EffectsManager.Instance.CocoonExplode(cocoonController.CocoonTransform);

        cocoonController.Disappear();
        butterflyController.Appear(cocoonController.CocoonTransform, BodyCount);

        GameEvents.OnGrewCocoonToMaxSize();
    }
}
