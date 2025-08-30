using MinbGamesLib;

public class GameManager : BaseGameManager
{
    protected override void AddManagers()
    {
        _managerList.Add(EventManager.Instance);
        _managerList.Add(ResourceManager.Instance);
        _managerList.Add(TableManager.Instance);
        _managerList.Add(LocalizationManager.Instance);
        _managerList.Add(PopupManager.Instance);
        _managerList.Add(UIManager.Instance);

        _managerList.Add(ObjectPoolManager.Instance);
        _managerList.Add(ObjectManager.Instance);
        _managerList.Add(AudioManager.Instance);
        _managerList.Add(SceneFlowManager.Instance);
        _managerList.Add(GameStateManager.Instance);
    }

    protected override void PreLoadManagers()
    {
        DebugManager.Instance.Init();
        CoroutineManager.Instance.Init();
        
#if UNITY_EDITOR
        DebugDrawManager.Instance.Init();
        GizmoDrawManager.Instance.Init();
        HandleDrawManager.Instance.Init();
#endif
    }

    protected override void OnInit()
    {
    }
}
