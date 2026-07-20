using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public enum GameState
    {
        Gameplay,       // regular mode
        ReadingNote,    // for note interaction UI
        Paused          // future use 
    }

    public GameState CurrentState { get; private set; } = GameState.Gameplay;

    public bool GameplayLocked => CurrentState != GameState.Gameplay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetState(GameState state)
    {
        CurrentState = state;
    }

    public bool IsState(GameState state)
    {
        return CurrentState == state;
    }
}