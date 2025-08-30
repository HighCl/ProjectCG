using MinbGamesLib;
using UnityEngine;

public class LobbySceneManager : BaseSceneManager<LobbySceneManager>
{
    [SerializeField] private UILobby _uiLobby;
    
    protected override void Init()
    {
        _uiLobby.Init();
        _uiLobby.Refresh();
    }
}
