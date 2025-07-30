using DG.Tweening;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _blemishes;
    [SerializeField]
    private float _baseFadeDuration = 0.3f;

    private CharacterMakeupData makeupData = new CharacterMakeupData();

    private CharacterMakeupItem[] _makeupItems;

    private void Start()
    {
        _makeupItems = transform.parent.GetComponentsInChildren<CharacterMakeupItem>(true);
    }

    public void ApplyMakeupTool(MakeupTool makeupTool)
    {
        switch (makeupTool)
        {
            case Cream cream:
                ApplyCream(cream);
                break;
            case Brush brush:
                ApplyBrush(brush);
                break;
            case Lipstick lipstick:
                ApplyLipstick(lipstick);
                break;
        }       
    }

    public void ClearAllMakeup()
    {
        if (!_blemishes.gameObject.activeSelf)
        {
            _blemishes.gameObject.SetActive(true);
            _blemishes.DOFade(1, _baseFadeDuration);
        }

        foreach (var item in _makeupItems)
        {
            item.Remove(_baseFadeDuration, 0f);
        }

        makeupData.Reset();
    }

    private void ApplyCream(Cream cream)
    {
#if UNITY_EDITOR
        Debug.Log("Apply Cream");
#endif
        _blemishes.DOFade(0, _baseFadeDuration).onComplete += DisableBlemishes;
        makeupData.SetData(cream.MakeupType, cream.MakeupColorId);
    }

    private void ApplyBrush(Brush brush)
    {
#if UNITY_EDITOR
        Debug.Log($"Apply Brush: {brush.MakeupType}");
#endif
        var items = _makeupItems.Where(x => x.MakeupType == brush.MakeupType);

        foreach (var item in items)
        {
            if (item.MakeupColorId == brush.MakeupColorId)
            {
                makeupData.SetData(brush.MakeupType, brush.MakeupColorId);
                item.Apply(_baseFadeDuration, _baseFadeDuration * 0.3f);
            }
            else
            {               
                item.Remove(_baseFadeDuration * 0.7f, 0f);//to fade faster
            }
        }
    }

    private void ApplyLipstick(Lipstick lipstick)
    {
#if UNITY_EDITOR
        Debug.Log($"Apply Lipstick: {lipstick.MakeupColorId}");
#endif
        var items = _makeupItems.Where(x => x.MakeupType == lipstick.MakeupType);

        foreach (var item in items)
        {
            if (item.MakeupColorId == lipstick.MakeupColorId)
            {
                makeupData.SetData(lipstick.MakeupType, lipstick.MakeupColorId);
                item.Apply(_baseFadeDuration, _baseFadeDuration * 0.3f);
            }
            else
            {
                item.Remove(_baseFadeDuration * 0.7f, 0f);
            }
        }
    }

    private void DisableBlemishes()
    {
        _blemishes.gameObject.SetActive(false);
    }
}
