using DG.Tweening;
using MinbGamesLib;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Linq;

public class ProductionManager : SingletonWithMono<ProductionManager>
{
    #region << =========== VOLUME =========== >>

    [SerializeField, FoldoutGroup("Volume Settings")] float _volumeWeight = 0.3f;
    [SerializeField, FoldoutGroup("Volume Settings")] float _volumeDuration = 5f;
    [SerializeField, FoldoutGroup("Volume Settings")] Volume _volume;

    #endregion

    #region << =========== CAMERA =========== >>

    [SerializeField, FoldoutGroup("Camera Settings")] float _targetOrthographicSize = 18f;
    [SerializeField, FoldoutGroup("Camera Settings")] float _cameraAnimationDuration = 3f;
    [SerializeField, FoldoutGroup("Camera Settings")] CinemachineCamera _cinemachineCamera;

    #endregion

    #region << =========== WINDOW =========== >>

    [SerializeField, FoldoutGroup("Window Settings")] public List<WindowController> _activeWindows = new List<WindowController>();
    [SerializeField, FoldoutGroup("Window Settings")] float _hitInterval = 2f;

    #endregion

    #region << =========== RESET =========== >>

    [SerializeField, FoldoutGroup("Reset Settings")] float _originalVolumeWeight;
    [SerializeField, FoldoutGroup("Reset Settings")] float _originalOrthographicSize;
    [SerializeField, FoldoutGroup("Reset Settings")] float _resetDuration = 2f;

    #endregion

    private CoroutineHandle _hitCoroutine;

    void Start()
    {
        var volumeObj = FindAnyObjectByType<UnityEngine.Rendering.Volume>();
        if (volumeObj != null)
        {
            _volume = volumeObj.GetComponent<Volume>();
            _originalVolumeWeight = _volume.weight;
        }

        // Virtual Camera 찾기
        if (_cinemachineCamera == null)
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            if (brain != null && brain.ActiveVirtualCamera != null)
            {
                var activeCamera = brain.ActiveVirtualCamera as Component;
                if (activeCamera != null)
                {
                    _cinemachineCamera = activeCamera.GetComponent<CinemachineCamera>();
                }
            }
        }

        // 초기 Orthographic Size 저장
        if (_cinemachineCamera != null)
        {
            _originalOrthographicSize = _cinemachineCamera.Lens.OrthographicSize;
        }

        FindWindowControllers();
    }

    private void FindWindowControllers()
    {
        _activeWindows = FindObjectsByType<WindowController>(FindObjectsSortMode.None).ToList();
    }

    [Button]
    void StartProduction()
    {
        DebugHelper.Log(DebugType.System, "StartProduction");

        if (_volume != null)
        {
            DOTween.To(() => _volume.weight, x => _volume.weight = x, 1f, _volumeDuration).From(_volumeWeight);
        }

        if (_cinemachineCamera != null)
        {
            DOTween.To(() => _cinemachineCamera.Lens.OrthographicSize,
                      x => _cinemachineCamera.Lens.OrthographicSize = x,
                      _targetOrthographicSize, _cameraAnimationDuration).From(_originalOrthographicSize);
        }

        StartRandomHitSequence();
    }

    private void StartRandomHitSequence()
    {
        WindowController PlayingWindow = _activeWindows.Where(w => w != null && w.IsPlaying).ToList().First();
        PlayingWindow.HidePerson();
        Global.Audio.PlaySFX("Reload");

        _hitCoroutine?.Stop();
        _hitCoroutine = Global.Coroutine.Interval(_hitInterval, () =>
        {
            HitRandomWindow();
        }, -1, this.gameObject);
    }

    private void HitRandomWindow()
    {
        // hit되지 않은 WindowController들 필터링
        var availableWindows = _activeWindows.Where(w => w != null && !w.IsPlaying && !w.IsHit).ToList();

        if (availableWindows.Count == 0)
        {
            DebugHelper.Log(DebugType.System, "모든 Window가 이미 Hit되었습니다!");
            return;
        }

        int randomIndex = Random.Range(0, availableWindows.Count);
        WindowController selectedWindow = availableWindows[randomIndex];

        if (selectedWindow != null)
        {
            DebugHelper.Log(DebugType.System, $"Window Hit: {selectedWindow.name}");
            Global.Audio.PlaySFX("Shoot");
            selectedWindow.Hit();
        }
    }

    [Button]
    void ResetProduction()
    {
        DebugHelper.Log(DebugType.System, "ResetProduction");

        // 진행 중인 모든 DOTween 중단
        DOTween.KillAll();

        // Random Hit Sequence 중단
        _hitCoroutine?.Stop();

        // Volume 효과 즉시 초기화
        if (_volume != null)
        {
            _volume.weight = _originalVolumeWeight;
        }

        // Camera Orthographic Size 효과 즉시 초기화
        if (_cinemachineCamera != null)
        {
            _cinemachineCamera.Lens.OrthographicSize = _originalOrthographicSize;
        }

        // 모든 Window 리셋
        ResetAllWindows();
    }

    void StopRandomHit()
    {
        _hitCoroutine?.Stop();
        DebugHelper.Log(DebugType.System, "Random Hit Sequence Stopped");
    }

    void ResetAllWindows()
    {
        foreach (var window in _activeWindows)
        {
            if (window != null)
            {
                window.Reset();
            }
        }
        DebugHelper.Log(DebugType.System, "All Windows Reset");
    }
}
