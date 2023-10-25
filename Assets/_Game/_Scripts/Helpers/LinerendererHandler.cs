using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

/// <summary>
/// Helper script for Line renderer.
/// </summary>
public class LinerendererHandler : MonoBehaviour
{
    #region Varibales

    [SerializeField] private UILineRenderer _lineRenderer;
    [SerializeField] private CanvasGroup _canvasGroup;
    private List<Vector2> _points = new List<Vector2>();
    #endregion Varibales

    #region Unity Methods
    private void Start()
    {
        if (!_lineRenderer)
            TryGetComponent(out _lineRenderer);
        if (!_canvasGroup)
            TryGetComponent(out _canvasGroup);
    }
    #endregion Unity Methods

    #region Public Methods
    public void Init(Vector2 startPose, Vector2 endPose)
    {
        _points.Add(startPose);
        _points.Add(endPose);
        _lineRenderer.Points = _points.ToArray();
        _lineRenderer.SetAllDirty();
    }
    public void FadeLineRenderer(bool fadeOut = true)
    {
        if (fadeOut)
            _canvasGroup.DOFade(0, .35f);
        else
            _canvasGroup.DOFade(1, .35f);
    }
    public void ChangeColor(Color color)
    {
        _lineRenderer.color = color;
    }
    #endregion Public Methods

    #region Private Methods
    #endregion Private Methods

    #region Callbacks
    #endregion Callbacks
}
