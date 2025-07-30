using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GameMain : MonoBehaviour
{
    [SerializeField]
    private Character _character;
    [SerializeField]
    private Hand _hand;
    [SerializeField]
    private DragZone _dragZone;
    [SerializeField]
    private DropZone _dropZone;
    [SerializeField]
    private CanvasGroup _bookCanvasGroup;

    private IMakeupTask _currentTask;
    private CanvasGroup _mainCanvasGroup;

    private void Start()
    {
        _mainCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void ClickMakeupTool(MakeupTool mTool)
    { 
        switch(mTool)
        {
            case Brush:
                throw new System.InvalidOperationException("MakeupBrush not handled");
            case Lipstick lipstick:
                StartMakeupToolTask(lipstick);
                break;
            case Cream cream:
                StartMakeupToolTask(cream);
                break;
            //case Loofah loofah:
                //StartMakeupToolTask(loofah);
                //break;
        };
    }

    public void ClickMakeupColorPick(MakeupColorPick colorPick)
    {
        switch (colorPick.MakeupType)
        {
            case MakeupType.Blush:
                StartMakeupBrushTask(colorPick.MakeupTool, colorPick);
                break;
            case MakeupType.Eyeshadow:
                StartMakeupBrushTask(colorPick.MakeupTool, colorPick);
                break;
            case MakeupType.Lipstick:
                break;
        }
    }

    public void ClickLoofah()
    {
        ClearAllMakeup();
    }

    private bool ClearAllMakeup()
    {
        if (_currentTask != null)
            return false;

        _character.ClearAllMakeup();

        return true;
    }

    private bool StartMakeupToolTask(Cream mTool)
    {
        if (_currentTask != null)
            return false;

        StartTask(new UseCreamTask(_hand, mTool, _dragZone, _dropZone, _character));

        return true;
    }  

    private bool StartMakeupToolTask(Lipstick mTool)
    {
        if (_currentTask != null)
            return false;

        StartTask(new UseCreamTask(_hand, mTool, _dragZone, _dropZone, _character));

        return true;
    }

    private bool StartMakeupBrushTask(MakeupTool brush, MakeupColorPick colorPick)
    {
        if (_currentTask != null)
            return false;

        StartTask(new UseBrushTask(_hand, brush, colorPick, _dragZone, _dropZone, _character));

        return true;
    }

    private void StartTask(IMakeupTask task) 
    {
        _currentTask = task;
        _currentTask.OnFinish += OnMakeupFinish;
        StartCoroutine(_currentTask.Start());
    }

    private void OnMakeupFinish(bool done)
    {
        _currentTask = null;
    }
}
