using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasRenderer))]
public class CharacterMakeupItem : MonoBehaviour
{
    [SerializeField]
    private MakeupType _makeupType;
    [SerializeField]
    private MakeupColorId _makeupColorId;

    public MakeupType MakeupType => _makeupType;
    public MakeupColorId MakeupColorId => _makeupColorId;

    private CanvasRenderer _canvasRenderer;

    private void Start()
    {
        _canvasRenderer = GetComponent<CanvasRenderer>();
        _canvasRenderer.SetAlpha(0);
    }

    public void Apply(float fadeDuration, float fadeDelay)
    {
        DOTween.Kill(_canvasRenderer);
        gameObject.SetActive(true);
        DOTween.To(() => _canvasRenderer.GetAlpha(), a => _canvasRenderer.SetAlpha(a), 1.0f, fadeDuration)
            .SetDelay(fadeDelay).SetTarget(_canvasRenderer);
    }

    public void Remove(float fadeDuration, float fadeDelay)
    {
        DOTween.Kill(_canvasRenderer);
        DOTween.To(() => _canvasRenderer.GetAlpha(), a => _canvasRenderer.SetAlpha(a), 0.0f, fadeDuration)
            .SetDelay(fadeDelay).SetTarget(_canvasRenderer).onComplete += DisableGameObject;
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
