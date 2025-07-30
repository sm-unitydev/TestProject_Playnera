using UnityEngine;

public abstract class MakeupTool : MonoBehaviour
{    
    public enum State
    { 
        InSlot,
        OutSlot
    }

    [SerializeField]
    protected RectTransform _targetPoint;
    [SerializeField]
    protected RectTransform _takePoint;
    [SerializeField]
    protected RectTransform _startDragPoint;

    public RectTransform TargetPoint => _targetPoint;
    public RectTransform StartDragPoint => _startDragPoint;
    public Vector2 SlotPos => _initialPosition + _takePointOffset;
    public State CurrentState => _state;

    public abstract MakeupType MakeupType { get; }
    public abstract MakeupColorId MakeupColorId { get; }
    public abstract bool IsBusy { get; }

    protected State _state;
    protected Transform _initialParent;
    protected Vector2 _initialPosition;
    protected Vector2 _takePointOffset;

    public abstract void TakeFromSlot();
    public abstract void PutToSlot();

    public virtual void TakeColor(MakeupColorPick colorPick) { }

    protected void Prepare()
    {
        _initialParent = transform.parent;
        _initialPosition = transform.position;
        _takePointOffset = _takePoint.position - transform.position;
    }

    protected void SetOutSlotState()
    {
        _state = State.OutSlot;
    }

    protected void SetInSlotState()
    {
        _state = State.InSlot;
    }
}
