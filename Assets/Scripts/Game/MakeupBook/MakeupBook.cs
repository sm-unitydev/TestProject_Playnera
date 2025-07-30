using DG.Tweening;
using UnityEngine;

public class MakeupBook : MonoBehaviour
{
    [SerializeField]
    private MakeupBookPage[] _pages;
    [SerializeField]
    private CanvasGroup _buttonsCanvasGroup;
    [SerializeField]
    private float _pageFadeDuration = 0.3f;

    private int _currPageNum = 0;

    private void Awake()
    {
        if (_pages == null || _pages.Length == 0)
        {
            _pages = GetComponentsInChildren<MakeupBookPage>(true);
        }
    }

    private void Start()
    {
        SetPage(0);
        _buttonsCanvasGroup.DOFade(1f, _pageFadeDuration);
    }

    private void SetPage(int num)
    {
        if (num < 0 || num >= _pages.Length)
            throw new System.ArgumentException($"Invalid page num: {num}");

        _currPageNum = num;

        for (int i = 0; i < _pages.Length; i++)
        {
            if (i == _currPageNum)
                _pages[i].Open(_pageFadeDuration);
            else 
                _pages[i].Close(_pageFadeDuration);
        }
    }

    public void NextPage()
    {
        SetPage(LoopPageNum(_currPageNum + 1));
    }

    public void PrevPage()
    {
        SetPage(LoopPageNum(_currPageNum - 1));
    }

    private int LoopPageNum(int num)
    {
        return num < 0 ? _pages.Length - 1 : num >= _pages.Length ? 0 : num;
    }
}
