using DG.Tweening;
using MinbGamesLib;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;

public class WindowController : MonoBehaviour
{
    #region << =========== PROPERTIES =========== >>
 
    [field: SerializeField] public bool IsPlaying { get; private set; } = false;
    [SerializeField] GameObject _window;
    [SerializeField] SpriteRenderer _windowSpriteRenderer;
    [SerializeField] GameObject _person;

    #endregion

    #region << =========== MOVEMENT SETTINGS =========== >>

    [SerializeField, FoldoutGroup("Movement Settings")] Vector2 _moveTimeRange = new Vector2(1f, 3f);
    [SerializeField, FoldoutGroup("Movement Settings")] Vector2 _xRange = new Vector2(-0.75f, 0.75f);
    [SerializeField, FoldoutGroup("Movement Settings")] Vector2 _waitTimeRange = new Vector2(0.5f, 2f);

    #endregion

    #region << =========== HIT SETTINGS =========== >>

    [SerializeField, FoldoutGroup("Hit Settings")] Color _hitColor = Color.red;
    [SerializeField, FoldoutGroup("Hit Settings")] float _hitColorDuration = 0.3f;
    [SerializeField, FoldoutGroup("Hit Settings")] float _cameraShakeIntensity = 1f;
    [SerializeField, FoldoutGroup("Hit Settings")] float _cameraShakeDuration = 0.2f;
    [SerializeField, FoldoutGroup("Hit Settings")] float _personFadeOutDuration = 0.5f;

    #endregion

    private Tween _currentTween;
    private Color _originalColor;
    public bool IsHit { get; private set; } = false;

    void Start()
    {
        StartCoroutine(InitAsync());
    }

    private IEnumerator InitAsync()
    {
        yield return new WaitUntil(() => Global.IsInitialized);
        Init();
    }

    void Init()
    {
        _window = gameObject;
        _windowSpriteRenderer = _window.GetComponent<SpriteRenderer>();

        if (_windowSpriteRenderer != null)
        {
            _originalColor = _windowSpriteRenderer.color;
        }

        if (_person != null)
        {
            _person.transform.localPosition = GetRandomLocalPos();
        }

        float randomWaitTime = Random.Range(_waitTimeRange.x, _waitTimeRange.y);
        Global.Coroutine.Wait(randomWaitTime, () => StartRandomMovement(), this);
    }

    private void StartRandomMovement()
    {
        if (_person == null || IsHit) return;

        Vector3 targetLocalPos = GetRandomLocalPos();

        float moveTime = Random.Range(_moveTimeRange.x, _moveTimeRange.y);

        _currentTween = _person.transform.DOLocalMove(targetLocalPos, moveTime)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                if (IsHit) return;
                
                float waitTime = Random.Range(_waitTimeRange.x, _waitTimeRange.y);
                Global.Coroutine.Wait(waitTime, () =>
                {
                    if (this != null && !IsHit)
                    {
                        StartRandomMovement();
                    }
                }, this);
            });
    }

    private Vector3 GetRandomLocalPos()
    {
        float randomX = Random.Range(_xRange.x, _xRange.y);
        return new Vector3(randomX, _person.transform.localPosition.y, _person.transform.localPosition.z);
    }

    [Button("Hit")]
    public void Hit()
    {
        if (IsHit) return;
        
        IsHit = true;
        
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
        }

        if (_windowSpriteRenderer != null)
        {
            _windowSpriteRenderer.DOColor(_hitColor, _hitColorDuration);
        }

        HidePerson();

        if (Global.Cinemachine != null)
        {
            Global.Cinemachine.Shake(_cameraShakeIntensity, _cameraShakeDuration);
        }
    }

    [Button("Reset")]
    public void Reset()
    {
        IsHit = false;
        
        // 윈도우 색상 복원
        if (_windowSpriteRenderer != null)
        {
            _windowSpriteRenderer.color = _originalColor;
        }

        // 캐릭터 복원
        ShowPerson();

        // 이동 재시작
        float randomWaitTime = Random.Range(_waitTimeRange.x, _waitTimeRange.y);
        Global.Coroutine.Wait(randomWaitTime, () => StartRandomMovement(), this);
    }

    public void ShowPerson()
    {
        if (_person != null)
        {
            _person.SetActive(true);
            var personSpriteRenderer = _person.GetComponent<SpriteRenderer>();
            if (personSpriteRenderer != null)
            {
                personSpriteRenderer.color = new Color(personSpriteRenderer.color.r, personSpriteRenderer.color.g, personSpriteRenderer.color.b, 220f/255f);
            }
            
            _person.transform.localPosition = GetRandomLocalPos();
        }
    }

    public void HidePerson()
    {
        if (_person != null)
        {
            var personSpriteRenderer = _person.GetComponent<SpriteRenderer>();
            if (personSpriteRenderer != null)
            {
                personSpriteRenderer.DOFade(0f, _personFadeOutDuration)
                    .OnComplete(() => 
                    {
                        _person.SetActive(false);
                    });
            }
            else
            {
                // SpriteRenderer가 없으면 GameObject 자체를 비활성화
                Global.Coroutine.Wait(_personFadeOutDuration, () => 
                {
                    _person.SetActive(false);
                }, this);
            }
        }
    }

    [Button("Start Movement")]
    public void StartMovement()
    {
        if (_currentTween != null)
        {
            _currentTween.Kill();
        }
        StartRandomMovement();
    }

    [Button("Stop Movement")]
    public void StopMovement()
    {
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
        }
    }
}
