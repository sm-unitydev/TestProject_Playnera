using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MakeupColorPick : MonoBehaviour
{
    [SerializeField]
    private Color _pickColor;
    [SerializeField]
    private MakeupColorId _colorId;
    [SerializeField]
    private MakeupType _makeupType;
    [SerializeField]
    private MakeupTool _makeupTool;

    public Color PickColor => _pickColor;
    public MakeupColorId ColorId => _colorId;   
    public MakeupType MakeupType => _makeupType;
    public MakeupTool MakeupTool => _makeupTool;
}
