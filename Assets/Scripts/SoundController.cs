using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour {
    [SerializeField] private AudioSource powerUp;
    [SerializeField] private AudioSource endLevelSuccess;
    [SerializeField] private AudioSource munch;
    [SerializeField] private AudioSource bonk;
    [SerializeField] private AudioSource pop;

    [SerializeField] private float munchPitchRange = 0.2f;
    [SerializeField] private float popPitchIncrement = 0.05f;

    private float _munchBasePitch = 1f;
    private float _popStartPitch = 1f;

    private void OnEnable() {
        GameEvents.HitBonusObject += OnHitBonusObject;
        GameEvents.HitObstacle += OnHitObstacle;
        GameEvents.GameStateChanged += OnGameStateChanged;
        GameEvents.GrewCocoonIncrement +=  OnGrewCocoonIncrement;
    }

    private void OnDisable() {
        GameEvents.HitBonusObject -= OnHitBonusObject;
        GameEvents.HitObstacle -= OnHitObstacle;
        GameEvents.GameStateChanged -= OnGameStateChanged;
        GameEvents.GrewCocoonIncrement -=  OnGrewCocoonIncrement;
    }

    private void Awake() {
        _munchBasePitch = munch.pitch;
        _popStartPitch = pop.pitch;
    }

    private void OnGameStateChanged(GameState state) {
        if (state == GameState.EndWin) {
            endLevelSuccess.Play();
        }
        else if (state == GameState.Home) {
            pop.pitch = _popStartPitch;
        }
    }

    private void OnHitBonusObject(BonusObject obj) {
        munch.pitch = _munchBasePitch + munchPitchRange * Random.Range(-1f, 1f);
        munch.Play();

        if (obj.type == BonusObjectType.Fever) {
            powerUp.Play();
        }
    }

    private void OnGrewCocoonIncrement() {
        pop.Play();
        pop.pitch += popPitchIncrement;
    }

    private void OnHitObstacle() {
        bonk.Play();
    }
}
