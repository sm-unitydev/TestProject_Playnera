using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class MakeupBookPage : MonoBehaviour
{
    private CanvasGroup _cachedCanvasGroup;
    private CanvasGroup _canvasGroup => _cachedCanvasGroup != null ? _cachedCanvasGroup : _cachedCanvasGroup = GetComponent<CanvasGroup>();

    public void Open(float fadeDuration)
    {
        DOTween.Kill(_canvasGroup);
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1, fadeDuration);       
    }

    public void Close(float fadeDuration) 
    {
        DOTween.Kill(_canvasGroup);
        _canvasGroup.DOFade(0, fadeDuration).onComplete += DisableGameObject;
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
