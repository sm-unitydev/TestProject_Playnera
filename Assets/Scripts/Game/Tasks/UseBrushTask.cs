using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBrushTask : IMakeupTask
{
    public event System.Action<bool> OnFinish;//bool - done or canceled

    private Hand _hand;
    private MakeupColorPick _colorPick;
    private MakeupTool _tool;
    private DropZone _dropZone;
    private DragZone _dragZone;
    private Character _character;

    public UseBrushTask(Hand hand, MakeupTool tool, MakeupColorPick colorPick, DragZone dragZone, DropZone dropZone, Character character)
    {
        _hand = hand;        
        _tool = tool;
        _colorPick = colorPick;
        _dragZone = dragZone;
        _dropZone = dropZone;
        _character = character;
    }

    public IEnumerator Start()
    {
        _hand.MoveToTakeTool(_tool);

        while (_hand.IsBusy) yield return null;

        _tool.TakeFromSlot();

        while (_tool.CurrentState != MakeupTool.State.OutSlot)
            yield return null;

        _hand.TakeTool(_tool);
        while (_hand.IsBusy) yield return null;

        _hand.MoveToColorPick(_colorPick);
        while (_hand.IsBusy) yield return null;

        _hand.TakeColor(_colorPick);
        _tool.TakeColor(_colorPick);
        while (_hand.IsBusy || _tool.IsBusy) yield return null;

        _hand.MoveToStartDragPos(_tool);
        while (_hand.IsBusy) yield return null;

        do
        {
            _dragZone.StartDrag(_hand);

            while (!_dragZone.IsDragging)
                yield return null;

            while (_dragZone.IsDragging)
                yield return null;
        }
        while (!RectTransformUtility.RectangleContainsScreenPoint(_dropZone.transform as RectTransform, _tool.TargetPoint.position));

        _hand.UseTool(_tool);
        _character.ApplyMakeupTool(_tool);

        while (_hand.IsBusy)
            yield return null;

        var fin = Finish(true);
        while (fin.MoveNext())
            yield return null;
    }

    private IEnumerator Finish(bool result)
    {
        _hand.MoveToPutTool(_tool);
        while (_hand.IsBusy)
            yield return null;

        _tool.PutToSlot();
        yield return null;

        _hand.MoveToInitialPos();
        while (_hand.IsBusy)
            yield return null;

        OnFinish(result);
        yield break;
    }
}
