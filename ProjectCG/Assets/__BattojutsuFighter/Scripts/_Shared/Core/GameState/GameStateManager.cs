using MinbGamesLib;

public class GameStateManager : BaseGameStateManager<GameStateManager>
{
    protected override void AddGameStates()
    {
        
    }

    public void ChangeState(GameState gameState)
    {
        DebugHelper.Log(DebugType.System, $"🟠 ---- [GameState] changed to: {gameState}");
        ChangeState((int)gameState);
    }
}