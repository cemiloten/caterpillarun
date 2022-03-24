using System;

public static class GameEvents {
    public static event Action<GameState> GameStateChanged;
    public static void OnGameStateChanged(GameState state) => GameStateChanged?.Invoke(state);


    public static event Action FirstInput;
    public static void OnFirstInput() => FirstInput?.Invoke();


    public static event Action ReachedEndOfTree;
    public static void OnReachedEndOfTree() => ReachedEndOfTree?.Invoke();


    public static event Action ClickedNextLevelButton;
    public static void OnClickedNextLevelButton() => ClickedNextLevelButton?.Invoke();


    public static event Action HitObstacle;
    public static void OnHitObstacle() => HitObstacle?.Invoke();


    public static event Action GrewCocoonIncrement;
    public static void OnGrewCocoonIncrement() => GrewCocoonIncrement?.Invoke();

    public static event Action<int> GrewCocoonToMaxSize;
    public static void OnGrewCocoonToMaxSize(int score) => GrewCocoonToMaxSize?.Invoke(score);


    public static event Action<int> BodyCountChanged;
    public static void OnBodyCountChanged(int bodyCount) => BodyCountChanged?.Invoke(bodyCount);


    public static event Action<BonusObject> HitBonusObject;
    public static void OnHitBonusObject(BonusObject obj) => HitBonusObject?.Invoke(obj);


    public static event Action BuyNoAds;
    public static void OnBuyNoAds() => BuyNoAds?.Invoke();


    public static event Action ClickedPurchaseNoAds;
    public static void OnClickedPurchaseNoAds() => ClickedPurchaseNoAds?.Invoke();

    public static event Action ButterflyFinishedAppearing;
    public static void OnButterflyFinishedAppearing() => ButterflyFinishedAppearing?.Invoke();

    public static event Action JoystickDown;
    public static void OnJoystickDown() => JoystickDown?.Invoke();


    public static event Action JoystickUp;
    public static void OnJoystickUp() => JoystickUp?.Invoke();
}
