using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HyperConnect
{
    public class TileEntity : MonoBehaviour, IPointerClickHandler
    {
        #region Variables
        public TileEntity parent;

        //[SerializeField] SerializedDictionary<Direction, TileEntity> _myNeighbours;
        [SerializeField] private List<TileEntity> _myNeighbours = new List<TileEntity>();
        [SerializeField] private SpriteDatabase _spriteDatabase;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _fruitImg;
        [SerializeField] private bool _isOccupied;
        private bool _isSelected;
        private int _id;
        private int _row;
        private int _column;
        public int ID => _id;
        public int Row => _row;
        public int Column => _column;
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
            _isOccupied = true;
            _isSelected = false;
            _fruitImg.sprite = _spriteDatabase.GetSpriteWithID(id);
        }
        public void HideTile()
        {
            _canvasGroup.alpha = 0;
        }
        public void FadeTile(bool hide = true)
        {
            if (hide)
                _canvasGroup.DOFade(0, .45f);
            else
                _canvasGroup.DOFade(1, .45f);
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
            GlobalEventHandler.OnTileEntitySelected?.Invoke(this);
        }
        public void UnSelectThisEntity()
        {
            _isSelected = false;
            MyUtils.Log($"UnSelected::{transform.name}");
            GlobalEventHandler.OnTileEntityUnSelected?.Invoke(this);
        }
        #endregion Public Methods


        #region Private Methods

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