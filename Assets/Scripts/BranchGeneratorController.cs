using System.Collections.Generic;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;
using UnityEngine;

public class BranchGeneratorController : MonoBehaviour {
    [SerializeField] private CurvyGenerator generator;
    [SerializeField] private BuildVolumeSpots buildVolumeSpots;

    private readonly List<BonusObject> _bonusObjects = new List<BonusObject>();

    private void OnEnable() {
        GameEvents.GameStateChanged += OnGameStateChanged;
        GameEvents.HitBonusObject += OnHitBonusObject;
    }

    private void OnDisable() {
        GameEvents.GameStateChanged -= OnGameStateChanged;
        GameEvents.HitBonusObject -= OnHitBonusObject;
    }

    private void OnGameStateChanged(GameState state) {
        if (state == GameState.Home) {
            RandomizeBranch();

            // Reinitialize bons objects.
            for (int i = _bonusObjects.Count - 1; i >= 0; i--) {
                BonusObject bonusObject = _bonusObjects[i];
                bonusObject.Enable();
                _bonusObjects.Remove(bonusObject);
            }
        }
    }

    private void OnHitBonusObject(BonusObject bonusObject) {
        _bonusObjects.Add(bonusObject);
    }

    private void RandomizeBranch() {
        buildVolumeSpots.Seed = Random.Range(int.MinValue, int.MaxValue);
        generator.Refresh();
    }
}
