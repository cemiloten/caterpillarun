using UnityEngine;
using UnityEngine.Serialization;

public class EffectsManager : SingletonMonoBehaviour<EffectsManager> {
    [SerializeField] private ParticleSystem bodyLostFx;
    [SerializeField] private ParticleSystem cocoonExplode;
    [FormerlySerializedAs("fruitExplode")] [SerializeField]
    private ParticleSystem eatFX;
    [SerializeField] private ParticleSystem confetti;

    private ObjectPool<ParticleSystem> _bodyLostFXs;

    private void OnEnable() {
        GameEvents.GameStateChanged += OnGameStateChanged;
        GameEvents.HitBonusObject += OnHitBonusObject;
        GameEvents.GrewCocoonToMaxSize += OnGrewCocoonToMaxSize;
    }

    private void OnDisable() {
        GameEvents.GameStateChanged -= OnGameStateChanged;
        GameEvents.HitBonusObject -= OnHitBonusObject;
        GameEvents.GrewCocoonToMaxSize -= OnGrewCocoonToMaxSize;
    }

    protected override void Awake() {
        base.Awake();
        _bodyLostFXs = new ObjectPool<ParticleSystem>(
            16,
            CreatePooledObject,
            OnTakeFromPool,
            OnReturnedToPool);
    }

    private void OnGameStateChanged(GameState state) {
        if (state == GameState.InGame) {
            confetti.Stop();
        }
    }

    private void OnHitBonusObject(BonusObject obj) {
        // eatFX.transform.position = obj.transform.position;
        eatFX.Play();
    }

    private void OnGrewCocoonToMaxSize() {
        confetti.Play();
    }

    private ParticleSystem CreatePooledObject() {
        return Instantiate(bodyLostFx);
    }

    private void OnTakeFromPool(ParticleSystem ps) {
    }

    private void OnReturnedToPool(ParticleSystem ps) {
    }

    private void OnDestroyPooledObject(ParticleSystem ps) {
        Destroy(ps.gameObject);
    }

    public void CocoonExplode(Transform cocoonTransform) {
        cocoonExplode.transform.position = cocoonTransform.position;
        cocoonExplode.Play();
    }

    public void LostBodyFeedback(Vector3 position) {
        ParticleSystem ps = _bodyLostFXs.Get();
        ps.transform.position = position;
        ps.Play();
    }
}
