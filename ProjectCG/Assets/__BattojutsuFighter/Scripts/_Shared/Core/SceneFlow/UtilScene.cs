public static class UtilScene
{
    public static string GetSceneName(this SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.PreLoading:
                return "PreLoadingScene";
            case SceneType.Title:
                return "TitleScene";
            case SceneType.Lobby:
                return "LobbyScene";
            case SceneType.Play:
                return "PlayScene";
            default:
                return "PlayScene";
        }
    }
}
