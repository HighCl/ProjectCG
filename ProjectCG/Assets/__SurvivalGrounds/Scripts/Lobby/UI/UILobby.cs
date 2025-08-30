using MinbGamesLib;
using UnityEngine;

public class UILobby : UIScene
{
    [SerializeField] private ButtonAtom _startBtn;

    protected override void OnInit()
    {
        _startBtn.AddListener(() =>
        {
            Global.Scene.LoadScene(SceneType.Play.GetSceneName(), (int)GlobalType.TransitionDim);
        });
    }
    protected override void OnRefresh() { }
}