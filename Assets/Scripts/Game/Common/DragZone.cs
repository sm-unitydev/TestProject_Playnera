using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class DragZone : Graphic, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool IsDragging => _isDragging;
   
    private Canvas _canvas;  

    private Transform _initialDraggableParent;
    private IDraggable _draggable;

    private bool _isDragging;
    private int _pointerId = -1;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
    }

    protected override void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void StartDrag(IDraggable draggable)
    {
        _draggable = draggable;
        raycastTarget = true;
    }

    public void FinishDrag()
    {
        if (_draggable == null)
            return;

        raycastTarget = false;
        _isDragging = false;        
        _pointerId = -1;

        _draggable.transform.SetParent(_initialDraggableParent);
        _draggable = null;
        _initialDraggableParent = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isDragging || _draggable == null)
            return;

        _isDragging = true;
        _pointerId = eventData.pointerId;
        _initialDraggableParent = _draggable.transform.parent;
        _draggable.transform.SetParent(_canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging || _draggable == null || eventData.pointerId != _pointerId)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 position
        ))
        {
            var worldPos = canvas.transform.TransformPoint(position);
            _draggable.DragToPos(worldPos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging || _draggable == null || eventData.pointerId != _pointerId)
            return;

        FinishDrag();
    }
}