using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HyperConnect
{
    /// <summary>
    /// This is basic unit of the game.<br></br>
    /// This handles the Tile entity behaviour.<br></br>
    /// Taking Tile Click Inputs from the player.<br></br>
    /// Like Moving tile to the designated position<br></br>
    /// Animating tile<br></br>
    /// Fading the tile
    /// </summary>
    public class TileEntity : MonoBehaviour, IPointerClickHandler
    {
        #region Variables
        public TileEntity parent;

        [SerializeField] private List<TileEntity> _myNeighbours = new List<TileEntity>();
        [SerializeField] private SpriteDatabase _spriteDatabase;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _tileHighlighter;
        [SerializeField] private Image _fruitImg;
        [SerializeField] private LinerendererHandler _lineHandler;
        [SerializeField] private bool _isOccupied;

        private bool _isSelected;
        private int _id;
        private int _row;
        private int _column;
        private Vector2Int _rowColumn;
        public int ID => _id;
        public int Row => _row;
        public int Column => _column;
        public Vector2Int RowColumn => _rowColumn;
        public bool IsOccupied => _isOccupied;

        #endregion Variables

        #region Unity Methods

        #endregion Unity Methods

        #region Public Methods

        public void Init(int id, int row, int column)
        {
            if (id == -1)
            {
                HideTile();
                return;
            }
            _id = id;
            this._row = row;
            this._column = column;
            _rowColumn = new Vector2Int(_row, _column);
            _isOccupied = true;
            _isSelected = false;
            _fruitImg.sprite = _spriteDatabase.GetSpriteWithID(id);
        }
        public void HideTile()
        {
            _canvasGroup.alpha = 0;
            _isOccupied = false;
            // _linesWithNeighboursList.ForEach(x => x.FadeLineRenderer());
        }

        public void FadeTile(bool hide = true)
        {
            if (hide)
            {
                _canvasGroup.DOFade(0, .45f);
                _isOccupied = false;
            }
            else
            {
                _canvasGroup.DOFade(1, .45f);
                _isOccupied = true;
            }
        }
        public System.Collections.ObjectModel.ReadOnlyCollection<TileEntity> GetMyNeighbours()
        {
            return _myNeighbours.AsReadOnly();
        }
        public void AddAsNeighbour(TileEntity entity)
        {
            if (!_myNeighbours.Contains(entity))
                _myNeighbours.Add(entity);
        }

        public bool IsLeafNode()
        {
            return _myNeighbours.Count == 1;
        }
        public void TweenToGivenPose(Vector3 position, System.Action onComplete = null)
        {
            Vector3 localPose = transform.localPosition;
            transform.DOLocalMove(position, .35f).onComplete += () =>
            {
                HideTile();
                transform.localPosition = localPose;
                onComplete?.Invoke();
            };
        }
        public void TweenToThePath(Vector3[] waypoints, System.Action onComplete = null)
        {
            Vector3 localPose = transform.localPosition;
            transform.DOLocalPath(waypoints, .35f).onComplete += () =>
            {
                HideTile();
                transform.localPosition = localPose;
                onComplete?.Invoke();
            };
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isSelected)
                SelectThisEntity();
            else
                UnSelectThisEntity();
        }
        public void SelectThisEntity()
        {
            _isSelected = true;
            //show particle effect..
            MyUtils.Log($"Selected::{transform.name}");
            _ShowSelectionEffect();
            GlobalEventHandler.OnTileEntitySelected?.Invoke(this);
        }

        public void UnSelectThisEntity()
        {
            _isSelected = false;
            MyUtils.Log($"UnSelected::{transform.name}");
            _tileHighlighter.SetActive(false);
            GlobalEventHandler.OnTileEntityUnSelected?.Invoke(this);
        }
        public void ShowCantMatchEffect()
        {
            transform.DOShakePosition(0.25f, Vector3.right * 10);
            UnSelectThisEntity();
        }
        #endregion Public Methods


        #region Private Methods
        private void _ShowSelectionEffect()
        {
            _tileHighlighter.SetActive(true);
            transform.DOPunchScale(Vector3.one * .1f, .15f);
        }
        #endregion Private Methods

        #region Callbacks

        #endregion Callbacks
    }

    public class Neighbours
    {

    }
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right,
        //##Diagonal##
        TopRight,
        TopLeft,
        BottomRight,
        BottomLeft,
    }
    /*
     * Level data
     * int id,
     * 
     */

}