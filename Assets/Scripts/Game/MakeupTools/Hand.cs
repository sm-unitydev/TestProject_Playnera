using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour, IDraggable
{
    public enum State
    { 
        Initial,
        Idle,
        WithTool
    }
   
    [SerializeField]
    private RectTransform _toolSlot;//Слот для инструментов (кисти, крем)
    [SerializeField]
    private float _dragSpeed = 10f;
    [SerializeField]
    private float _moveStartDragDuration = 0.4f;
    [SerializeField]
    private float _moveTakeToolDuration = 0.4f;
    [SerializeField]
    private float _movePutToolDuration = 0.4f;
    [SerializeField]
    private float _moveToColorPickDuration = 0.4f;
    [SerializeField]
    private float _moveInitialPosDuration = 0.5f;
    [SerializeField]
    private float _fixPosBeforeAnimDuration = 0.2f;
    [SerializeField]
    private Ease _moveEasing = Ease.OutQuad;

    public State CurrentState => _state;
    public bool IsBusy => _animate || (_tween != null && _tween.active);

    private RectTransform _rectTransform;
    private Animator _animator;

    private Vector2 _initialPos;
    private Vector2 _toolSlotOffset;

    private State _state;
    
    private bool _needDrag;
    private Vector2 _dragTargetPos;

    private bool _needAnimOffset;
    private Vector2 _animOffset;

    private Tween _tween;
    private bool _animate;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _initialPos = _rectTransform.position;
        _rectTransform.anchorMax = _rectTransform.anchorMin = Vector2.one * 0.5f;//for animations
        _rectTransform.position = _initialPos;
        _toolSlotOffset = _toolSlot.position - _rectTransform.position;       
    }

    public void MoveToStartDragPos(MakeupTool mTool)
    {
        KillTweens();

        _tween = _rectTransform.DOMove((Vector2)mTool.StartDragPoint.position - _toolSlotOffset, _moveStartDragDuration).SetEase(_moveEasing);
        _tween.onComplete += SetIdleState;
    }

    public void MoveToTakeTool(MakeupTool mTool)
    {
        KillTweens();

        _tween = _rectTransform.DOMove(mTool.SlotPos - _toolSlotOffset, _moveTakeToolDuration).SetEase(_moveEasing);
        _tween.onComplete += SetIdleState;
    }

    public void MoveToPutTool(MakeupTool mTool)
    {
        KillTweens();

        _tween = _rectTransform.DOMove(mTool.SlotPos - _toolSlotOffset, _movePutToolDuration).SetEase(_moveEasing);
        _tween.onComplete += SetIdleState;
    }

    public void MoveToColorPick(MakeupColorPick mColorPick)
    {
        KillTweens();

        var mTool = mColorPick.MakeupTool;
        Vector2 targetOffset = mTool.TargetPoint.position - mTool.transform.position;
        var targetPos = (Vector2)mColorPick.transform.position - _toolSlotOffset - targetOffset;

        _tween = _rectTransform.DOMove(targetPos, _moveToColorPickDuration).SetEase(_moveEasing);
        _tween.onComplete += SetIdleState;
    }

    public void MoveToInitialPos()
    {
        KillTweens();

        _tween = _rectTransform.DOMove(_initialPos, _moveInitialPosDuration).SetEase(_moveEasing);
        _tween.onComplete += SetInitialState;
    }

    public void TakeTool(MakeupTool mTool)
    {
        _needDrag = false;
        mTool.transform.SetParent(_toolSlot);
        _state = State.WithTool;
    }

    public void UseTool(MakeupTool mTool)
    {
        _needDrag = false;

        switch (mTool)
        {
            case Cream:
                StartCoroutine(PlayAnim(Constants.UseCreamAnimationHash));
                break;

            case Brush:
                if(mTool.MakeupType == MakeupType.Eyeshadow)
                    StartCoroutine(PlayAnim(Constants.UseEyeShadowAnimationHash));
                else
                    StartCoroutine(PlayAnim(Constants.UseBlushAnimationHash));               
                break;

            case Lipstick:
                StartCoroutine(PlayAnim(Constants.UseLipstickAnimationHash));
                break;
        }
    }

    public void TakeColor(MakeupColorPick colorPick)
    {
        _needDrag = false;

        StartCoroutine(PlayAnimWithOffset(Constants.ColorPickAnimationHash, colorPick));
    }

    private IEnumerator PlayAnim(int animhash)
    {
        _animate = true;

        var startPos = _rectTransform.position;

        _animator.enabled = true;
        _animator.Play(animhash, 0);
        _animator.Update(0f);
        _animator.enabled = false;

        var dstPos = _rectTransform.position;
        _rectTransform.position = startPos;

        //move from curr pos to anim start pos
        var tween = _rectTransform.DOMove(dstPos, _fixPosBeforeAnimDuration);
        while (tween.active)
            yield return null;

        _animator.enabled = true;

        while (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animhash)
            yield return null;

        _animator.enabled = false;
        _animate = false;
    }

    private IEnumerator PlayAnimWithOffset(int animHash, MakeupColorPick colorPick)
    {
        _animate = true;

        _animator.enabled = true;
        _animator.Play(animHash, 0);
        _animator.Update(0f);
        _animator.enabled = false;

        _animOffset = colorPick.transform.position - colorPick.MakeupTool.TargetPoint.position;
        _rectTransform.position += (Vector3)_animOffset;
        _needAnimOffset = true;
        _animator.enabled = true;

        while (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == animHash)
            yield return null;

        _animator.enabled = false;
        _animate = false;

        _needAnimOffset = false;
        _animOffset = Vector2.zero;
    }

    public void DragToPos(Vector2 worldPos)
    {
        _needDrag = true;
        _dragTargetPos = worldPos - _toolSlotOffset;
    }

    private void Update()
    {
        if (_needDrag)
        {
            _rectTransform.position = Vector2.Lerp(_rectTransform.position, _dragTargetPos, Time.deltaTime * _dragSpeed);
            _needDrag = (Vector2)_rectTransform.position != _dragTargetPos;
        }
    }

    private void LateUpdate()
    {
        if(_needAnimOffset)
            _rectTransform.position += (Vector3)_animOffset;
    }

    private void KillTweens()
    {
        _needDrag = false;
        DOTween.Kill(_rectTransform);
    }

    private void SetIdleState()
    {
        _state = State.Idle;
    }
    private void SetInitialState()
    {
        _state = State.Initial;
    }
}
