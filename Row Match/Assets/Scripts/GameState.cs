public enum State
{
    Launched,
    LevelFailed,
    LevelCompleted
    
}

public static class GameState
{
    public static State CurrentGameState = State.Launched;
    public static LevelInfo SelectedLevelInfo;

}
