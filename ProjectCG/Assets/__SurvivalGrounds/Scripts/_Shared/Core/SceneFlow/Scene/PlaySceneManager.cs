using MinbGamesLib;
using UnityEngine;

public class PlaySceneManager : BaseSceneManager<PlaySceneManager>
{
    [SerializeField] private UIPlay _uiPlay;
    [SerializeField] private CinemachineController _cinemachineController;
    
    protected override void Init()
    {
        _uiPlay.Init();
        _uiPlay.Refresh();
        
        _cinemachineController.Init();
    }
}
