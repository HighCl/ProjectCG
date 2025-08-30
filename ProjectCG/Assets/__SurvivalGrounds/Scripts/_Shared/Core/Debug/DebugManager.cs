using MinbGamesLib;

public class DebugManager : BaseDebugManager<DebugManager>
{
    protected override void LoadAsset()
    {
        _debugAsset = DebugAsset.Instance;
    }
}