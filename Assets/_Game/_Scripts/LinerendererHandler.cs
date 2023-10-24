using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LinerendererHandler : MonoBehaviour
{
    #region Varibales

    [SerializeField] private UILineRenderer _lineRenderer;

    private List<Vector2> _points = new List<Vector2>();
    #endregion Varibales

    #region Unity Methods
    private void Start()
    {
        if (!_lineRenderer)
            TryGetComponent(out _lineRenderer);
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
    #endregion Public Methods

    #region Private Methods
    #endregion Private Methods

    #region Callbacks
    #endregion Callbacks
}
