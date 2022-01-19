using System.ComponentModel;
using OPX.Scripts;
using UnityEngine;

public partial class SROptions {
    [NumberRange(0, 15)]
    [Category("Gameplay")]
    public float MovementSpeed {
        get => Object.FindObjectOfType<PlayerController>().movementSpeed;
        set => Object.FindObjectOfType<PlayerController>().movementSpeed = value;
    }

    [NumberRange(0, 360)]
    [Category("Gameplay")]
    public float RotationSpeed {
        get => Object.FindObjectOfType<PlayerController>().rotationSensitivity;
        set => Object.FindObjectOfType<PlayerController>().rotationSensitivity = value;
    }

    [Category("Gameplay")]
    public float FeverTime {
        get => Object.FindObjectOfType<PlayerController>().feverTime;
        set => Object.FindObjectOfType<PlayerController>().feverTime = value;
    }

    [Category("Gameplay")]
    public float FeverSpeedMultiplier {
        get => Object.FindObjectOfType<PlayerController>().feverSpeedMultiplier;
        set => Object.FindObjectOfType<PlayerController>().feverSpeedMultiplier = value;
    }

    [Category("Gameplay")]
    public int FeverBodyPartAmount {
        get => Object.FindObjectOfType<PlayerController>().feverBodyPartAmount;
        set => Object.FindObjectOfType<PlayerController>().feverBodyPartAmount = value;
    }


    [Category("Gameplay")]
    public float HeadAngleRange {
        get => Object.FindObjectOfType<PlayerController>().headAngleRange;
        set => Object.FindObjectOfType<PlayerController>().headAngleRange = value;
    }


    // [Category("Ads")]
    // public bool TestMode {
    //     get => Object.FindObjectOfType<AdManager>().testMode;
    //     set => Object.FindObjectOfType<AdManager>().testMode = value;
    // }
    //
    // [Category("Ads")]
    // public bool NoAds {
    //     get => Object.FindObjectOfType<AdManager>().noAdsDebug;
    //     set => Object.FindObjectOfType<AdManager>().noAdsDebug = value;
    // }
}
