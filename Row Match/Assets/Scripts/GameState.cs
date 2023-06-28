public enum State
{
    Launched,
    LevelFailed,
    LevelCompleted,
    LevelCompletedHighScore
}

public static class GameState
{
    public static State CurrentGameState = State.Launched;
    public static LevelInfo SelectedLevelInfo;

}
