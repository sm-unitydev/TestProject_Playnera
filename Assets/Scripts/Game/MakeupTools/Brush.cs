using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Brush : MakeupTool
{
    [SerializeField]
    private Image _brushTip;
    [SerializeField]
    private MakeupType _makeupType;
    [SerializeField]
    private float _takeDuration = 0.2f;
    [SerializeField]
    private float _pickColorDuration = 0.2f;

    private MakeupColorId _makeupColorId;

    public override MakeupColorId MakeupColorId => _makeupColorId;
    public override MakeupType MakeupType => _makeupType;

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
        _brushTip.color = new Color(1, 1, 1, 0);
    }

    public override void TakeColor(MakeupColorPick colorPick)
    {
        _makeupColorId = colorPick.ColorId;
        _brushTip.DOColor(colorPick.PickColor, _pickColorDuration);
    }
}
