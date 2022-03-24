using UnityEngine;

public enum GameState {
    Home,
    InGame,
    EndWin
}

public class GameManager : MonoBehaviour {
    public static GameState State { get; private set; }

    private void OnEnable() {
        GameEvents.FirstInput += OnFirstInput;
        GameEvents.ClickedNextLevelButton += OnClickedNextLevelButton ;
        GameEvents.GrewCocoonToMaxSize += OnGrewCocoonToMaxSize ;
    }

    private void OnDisable() {
        GameEvents.FirstInput -= OnFirstInput;
        GameEvents.ClickedNextLevelButton -= OnClickedNextLevelButton ;
        GameEvents.GrewCocoonToMaxSize -= OnGrewCocoonToMaxSize ;
    }

    private void OnFirstInput() {
        if (State != GameState.Home) {
            return;
        }

        SetState(GameState.InGame);
    }

    private void OnClickedNextLevelButton() {
        if (State != GameState.EndWin) {
            return;
        }

        SetState(GameState.Home);
    }

    private void OnGrewCocoonToMaxSize(int score) {
        if (State != GameState.InGame) {
            return;
        }

        SetState(GameState.EndWin);
    }

    private void Start() {
        SetState(GameState.Home);
    }

    private static void SetState(GameState state) {
        State = state;
        GameEvents.OnGameStateChanged(state);
    }
}
