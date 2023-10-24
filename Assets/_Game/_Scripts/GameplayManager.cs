using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace HyperConnect
{
    public class GameplayManager : MonoBehaviour
    {
        #region Varibales

        public UILineRenderer linerenderer;


        [SerializeField] private TileEntity _tileEntityPrefab;
        [SerializeField] private LinerendererHandler _lineRenderer;
        [SerializeField] private Transform _boardTransform;
        [SerializeField] private Transform _lineRendrersParents;
        [SerializeField] private CanvasGroup _fadeBgCanvasGroup;

        private Stack<TileEntity> _selectedTileStack;
        private List<TileEntity> _pathTrackList;

        private TileEntity[,] _spawnedEntities;
        private const byte MIN_TILES_TO_MATCH = 2;
        #endregion Varibales

        #region Unity Methods
        private void OnEnable()
        {
            GlobalEventHandler.OnTileEntitySelected += Callback_On_TileEntity_Selected;
            GlobalEventHandler.OnTileEntityUnSelected += Callback_On_TileEntity_UnSelected;
        }
        private void OnDisable()
        {
            GlobalEventHandler.OnTileEntitySelected -= Callback_On_TileEntity_Selected;
            GlobalEventHandler.OnTileEntityUnSelected -= Callback_On_TileEntity_UnSelected;
        }
        private IEnumerator Start()
        {
            _selectedTileStack = new Stack<TileEntity>();
            _pathTrackList = new List<TileEntity>();
            yield return _InitLevel();
        }
        #endregion Unity Methods

        #region Public Methods

        #endregion Public Methods

        #region Private Methods
        private IEnumerator _InitLevel()
        {
            _fadeBgCanvasGroup.gameObject.SetActive(true);
            LevelData leveldata = LevelDataHandler.GetLevelDataByLevelIndex(1);
            int[,] indexData = leveldata.indexData;
            _spawnedEntities = new TileEntity[indexData.GetLength(0), indexData.GetLength(1)];
            List<Edge> edgeData = leveldata.edgeData;
            for (int i = 0, count = indexData.GetLength(0); i < count; i++)
            {
                for (int j = 0, count1 = indexData.GetLength(1); j < count1; j++)
                {
                    TileEntity entity = Instantiate(_tileEntityPrefab, _boardTransform);
                    int index = indexData[i, j];
                    entity.Init(index, i, j);
                    _spawnedEntities[i, j] = entity;
                    if (index != -1)
                        MyUtils.Log($"_spawned::{i},{j}");
                }
            }
            yield return new WaitForEndOfFrame();
            foreach (Edge edge in edgeData)
            {
                LinerendererHandler lineRenderer = Instantiate(_lineRenderer, _lineRendrersParents);
                TileEntity startEntity = _spawnedEntities[edge.start.x, edge.start.y];
                TileEntity endEntity = _spawnedEntities[edge.end.x, edge.end.y];
                startEntity.AddAsNeighbour(endEntity);
                endEntity.AddAsNeighbour(startEntity);
                lineRenderer.Init(startEntity.transform.localPosition, endEntity.transform.localPosition);
            }
            _fadeBgCanvasGroup.DOFade(0, 1.75f).onComplete += () =>
            {
                _fadeBgCanvasGroup.gameObject.SetActive(false);
            };
        }
        TileEntity startEntity;
        TileEntity endEntity;
        private void _CheckForMatch()
        {
            MyUtils.Log($"Check For match...");
            _pathTrackList.Clear();
            endEntity = _selectedTileStack.Pop();
            startEntity = _selectedTileStack.Pop();

            _pathTrackList.Add(startEntity);
            if (startEntity.GetMyNeighbours().Contains(endEntity))
                _pathTrackList.Add(endEntity);
            else
                _LoopForDestinationEntity(startEntity, startEntity, endEntity);
            MyUtils.Log($"path:: {_pathTrackList.Count}");
        }

        //private void _LoopThroughNeighbours(TileEntity startEntity, TileEntity dest)
        //{
        //    if (startEntity.IsLeafNode() && startEntity != this.startEntity)
        //    {
        //        if (startEntity != dest && _pathTrackList.Contains(startEntity))
        //            _pathTrackList.Remove(startEntity);
        //        return;
        //    }
        //    if (!_pathTrackList.Contains(startEntity))
        //        _pathTrackList.Add(startEntity);
        //    foreach (TileEntity entity in startEntity.GetMyNeighbours())
        //    {
        //        if (entity == startEntity || _pathTrackList.Contains(entity)) continue;
        //        if (entity.GetMyNeighbours().Contains(dest))
        //        {
        //            if (!_pathTrackList.Contains(entity))
        //                _pathTrackList.Add(entity);
        //            _pathTrackList.Add(dest);
        //            return;
        //        }
        //        if (_pathTrackList.Contains(dest)) return;
        //        _LoopThroughNeighbours(entity, dest);
        //    }
        //}

        private void _LoopForDestinationEntity(TileEntity comingFrom, TileEntity start, TileEntity end)
        {
            var neighbours = start.GetMyNeighbours();
            if (neighbours.Count == 1)
            {
                if ((neighbours[0] == comingFrom) && neighbours[0] != end) return;
            }
            if (neighbours.Contains(end))
            {
                end.parent = start;
                _BackTraceToTheParent(end);
                return;
            }
            foreach (TileEntity entity in neighbours)
            {
                if (entity == comingFrom) continue;
                entity.parent = start;
                _LoopForDestinationEntity(start, entity, end);
            }
        }

        private void _BackTraceToTheParent(TileEntity tileEntity)
        {
            //Stack<TileEntity> path = new Stack<TileEntity>();
            _pathTrackList.Clear();
            TileEntity curr = tileEntity;
            while (curr != this.startEntity)
            {
                _pathTrackList.Add(curr);
                // path.Push(curr);
                curr = curr.parent;
            }
            //path.Push(startEntity);
            _pathTrackList.Add(startEntity);

            if (_pathTrackList.Count > 0)
            {
                List<Vector2> points = new List<Vector2>();
                foreach (TileEntity entity in _pathTrackList)
                    points.Add(entity.transform.localPosition);
                linerenderer.Points = points.ToArray();
                linerenderer.SetAllDirty();
            }
            MyUtils.Log($"Path Track::: {_pathTrackList.Count}");
        }

        //private List<List<int>> edgesArrayList = new List<List<int>>();

        //// An utility function to do 
        //// DFS of graph recursively 
        //// from a given vertex x. 
        // void DFS(bool[] visitedArray, int x, int y, List<int> stack)
        //{
        //    stack.Add(x);
        //    if (x == y)
        //    {

        //        // print the path and return on 
        //        // reaching the destination node 

        //        // printPath(stack);
        //        return;
        //    }
        //    visitedArray[x] = true;

        //    // if backtracking is taking place

        //    if (edgesArrayList[x].Count > 0)
        //    {
        //        for (int j = 0; j < edgesArrayList[x].Count; j++)
        //        {

        //            // if the node is not visited 
        //            if (visitedArray[edgesArrayList[x][j]] == false)
        //            {
        //                DFS(visitedArray, edgesArrayList[x][j], y, stack);
        //            }
        //        }
        //    }
        //    stack.RemoveAt(stack.Count - 1);
        //}

        //// A utility function to initialise 
        //// visited for the node and call 
        //// DFS function for a given vertex x. 
        // void DFSCall(int x, int y, int n,
        //                    List<int> stack)
        //{

        //    // visited array 
        //    bool[] vis = new bool[n + 1];
        //    System.Array.Fill(vis, false);

        //    // memset(vis, false, sizeof(vis)) 

        //    // DFS function call 
        //    DFS(vis, x, y, stack);
        //}








        private void _GetPathWithNeighbours(TileEntity startEntity, TileEntity dest)
        {

        }

        #endregion Private Methods

        #region Callbacks
        private void Callback_On_TileEntity_Selected(TileEntity selectedEntity)
        {
            if (selectedEntity.ID != _selectedTileStack.Peek().ID)
                _selectedTileStack.Pop().UnSelectThisEntity();
            _selectedTileStack.Push(selectedEntity);

            if (_selectedTileStack.Count >= MIN_TILES_TO_MATCH)
                _CheckForMatch();
        }
        private void Callback_On_TileEntity_UnSelected(TileEntity unSelectedEntity)
        {
            if (_selectedTileStack.Count > 0)
                _selectedTileStack.Pop();
        }
        #endregion Callbacks
    }
}