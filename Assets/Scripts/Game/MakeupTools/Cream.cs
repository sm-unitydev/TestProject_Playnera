using UnityEngine;
using DG.Tweening;

public class Cream : MakeupTool
{
    [SerializeField]
    private float _takeDuration = 0.2f;

    public override MakeupType MakeupType => MakeupType.Cream;
    public override MakeupColorId MakeupColorId => MakeupColorId.None;

    public override bool IsBusy => _tween != null && _tween.active;

    private Tween _tween;

    private void Start()
    {
        Prepare();
    }

    public override void TakeFromSlot()
    {
        _tween = transform.DOScale(1.1f, _takeDuration);
        _tween.onComplete += SetOutSlotState;
    }

    public override void PutToSlot()
    {
        transform.SetParent(_initialParent);
        _tween = transform.DOScale(1.0f, _takeDuration);
        _tween.onComplete += SetInSlotState;
    }
}
