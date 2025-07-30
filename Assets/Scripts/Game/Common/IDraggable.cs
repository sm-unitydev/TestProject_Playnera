using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDraggable
{
    Transform transform { get; }
    void DragToPos(Vector2 pos);
}
