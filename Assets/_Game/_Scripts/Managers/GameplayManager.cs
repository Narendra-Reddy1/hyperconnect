using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HyperConnect
{
    public class GameplayManager : MonoBehaviour
    {
        #region Varibales

        //Debug purpose...
        //public UILineRenderer linerenderer;
    
        [Header("Prefab References")]
        [Space(2)]
        [SerializeField] private TileEntity _tileEntityPrefab;
        [SerializeField] private LinerendererHandler _lineRenderer;
        [Space(5)]

        [Header("Parent References")]
        [Space(2)]
        [SerializeField] private Transform _boardTransform;
        [SerializeField] private Transform _lineRendrersParents;
        [Space(5)]

        [SerializeField] private CanvasGroup _fadeBgCanvasGroup;
        [SerializeField] private Coffee.UIExtensions.UIParticle _starParticles;

        private TileEntity _startEntity;
        private TileEntity _endEntity;
        private Stack<TileEntity> _selectedTileStack;
        private List<TileEntity> _pathTrackList;


        ///Replaced 2D array with list for ease of traversal.
        ///performed linq queries at most of the places 
        ///bcz the count of the list doesn't go beyond size of the grid(in this case 11x5=55) objects.
        ///So it is acceptable to perform linq queries on small data sets.
        private List<TileEntity> _spawnedEntityList = new List<TileEntity>();
        //private TileEntity[,] _spawnedEntities;

        private Dictionary<Pair, List<LinerendererHandler>> _pairPathDict = new Dictionary<Pair, List<LinerendererHandler>>();

        /// <summary>
        /// Constant value to check number of tiles to match.
        /// Same value can be used to verify level data is correct with min. tiles or not.
        /// But in this prototype project that logic is not handled.
        /// </summary>
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
        /// <summary>
        /// This method loads the new level.<br></br>
        /// It is responsible for spawning tiles, drawing lines, caching path between same tiles.
        /// </summary>
        /// <returns></returns>
        private IEnumerator _InitLevel()
        {
            _fadeBgCanvasGroup.gameObject.SetActive(true);
            LevelData leveldata = LevelDataHandler.GetLevelDataByLevelIndex(GlobalVariables.HighestUnlockedLevel);
            int[,] indexData = leveldata.indexData;
            //_spawnedEntities = new TileEntity[indexData.GetLength(0), indexData.GetLength(1)];
            List<Edge> edgeData = leveldata.edgeData;
            for (int i = 0, count = indexData.GetLength(0); i < count; i++)
            {
                for (int j = 0, count1 = indexData.GetLength(1); j < count1; j++)
                {
                    TileEntity entity = Instantiate(_tileEntityPrefab, _boardTransform);
                    entity.name = $"Tile_{i}x{j}";
                    int index = indexData[i, j];
                    entity.Init(index, i, j);
                    // _spawnedEntities[i, j] = entity;
                    _spawnedEntityList.Add(entity);
                }
            }
            yield return new WaitForSeconds(.15f);
            foreach (Edge edge in edgeData)
            {
                //TileEntity startEntity = _spawnedEntities[edge.start.x, edge.start.y];
                //TileEntity endEntity = _spawnedEntities[edge.end.x, edge.end.y];
                TileEntity startEntity = _spawnedEntityList.Find(x => x.RowColumn == edge.start);
                TileEntity endEntity = _spawnedEntityList.Find(x => x.RowColumn == edge.end);
                startEntity.AddAsNeighbour(endEntity);
                endEntity.AddAsNeighbour(startEntity);
                // _DrawNeighbourLines(startEntity, endEntity);
            }

            Stack<int> indexTracker = new Stack<int>();
            for (int i = 0, count = indexData.GetLength(0); i < count; i++)
            {
                for (int j = 0, count1 = indexData.GetLength(1); j < count1; j++)
                {
                    for (int k = 0, count2 = indexData.GetLength(0); k < count2; k++)
                    {
                        for (int l = 0, count3 = indexData.GetLength(1); l < count3; l++)
                        {
                            if (i == k && j == l) continue;
                            if (indexData[i, j] == indexData[k, l] && indexData[i, j] != -1 && !indexTracker.Contains(indexData[i, j]))
                            {
                                _pathTrackList.Clear();
                                //_FindPath(_spawnedEntities[i, j], _spawnedEntities[k, l]);
                                _FindPath(_spawnedEntityList.Find(x => x.Row == i && x.Column == j), _spawnedEntityList.Find(x => x.Row == k && x.Column == l));
                                MyUtils.Log($"!!!! :: path count: {_pathTrackList.Count}");
                                List<LinerendererHandler> renderers = new List<LinerendererHandler>();
                                for (int m = 0, count4 = _pathTrackList.Count - 1; m < count4; m++)
                                {
                                    LinerendererHandler renderer = Instantiate(_lineRenderer, _lineRendrersParents);
                                    renderer.Init(_pathTrackList[m].transform.localPosition, _pathTrackList[m + 1].transform.localPosition);
                                    renderers.Add(renderer);
                                }
                                indexTracker.Push(indexData[i, j]);
                                _pairPathDict.Add(new Pair(new Vector2Int(i, j), new Vector2Int(k, l)), renderers);
                            }
                        }
                    }
                }
            }


            _startEntity = null;
            _endEntity = null;
            GlobalEventHandler.OnNewLevelStarted?.Invoke(GlobalVariables.HighestUnlockedLevel);
            _fadeBgCanvasGroup.DOFade(0, 1.25f).onComplete += () =>
            {
                _fadeBgCanvasGroup.gameObject.SetActive(false);
            };
        }
        /// <summary>
        /// This will clear entire board of the level.
        /// Destroys all tiles and line renderers
        /// </summary>
        private void _ClearEntireBoard()
        {
            for (int i = 0, count = _boardTransform.childCount; i < count; i++)
            {
                Destroy(_boardTransform.GetChild(i).gameObject);
            }
            for (int i = 0, count = _lineRendrersParents.childCount; i < count; i++)
            {
                Destroy(_lineRendrersParents.GetChild(i).gameObject);
            }
            _spawnedEntityList.Clear();
            _selectedTileStack.Clear();
            _pairPathDict.Clear();
        }

        /// <summary>
        /// This method enquires the path between the selected two entites is valied or not.<br></br>
        /// If the path is valied invokes Matching logic.<br></br>
        /// else<br></br>
        /// Invokes Can't Match animation logic
        /// </summary>
        private void _CheckForMatch()
        {
            MyUtils.Log($"Check For match...");
            _pathTrackList.Clear();
            _endEntity = _selectedTileStack.Pop();
            _startEntity = _selectedTileStack.Pop();

            _FindPath(_startEntity, _endEntity);

            bool isValidPath = _IsPathValid(_pathTrackList);
            if (isValidPath)
            {
                // _DrawDebugLine();
                _ShowMatchEffect();
            }
            else
            {
                _startEntity.ShowCantMatchEffect();
                _endEntity.ShowCantMatchEffect();
            }
        }
        private void _FindPath(TileEntity start, TileEntity end)
        {
            _startEntity = start;//for level initialization setup
            _endEntity = end;
            if (start.GetMyNeighbours().Contains(end))
            {
                _pathTrackList.Add(start);
                _pathTrackList.Add(end);
            }
            else
                _LoopForDestinationEntity(start, start, end);
        }
        /// <summary>
        /// This method handles the visual representation of the tiles matching.<br></br>
        /// It is responsible for changing color path.<br></br>
        /// Moving the matched tiles to the middle.<br></br>
        /// Making the tiles and the unnecessary path fading out.
        /// </summary>
        private void _ShowMatchEffect()
        {
            int count = _pathTrackList.Count;
            int midPoint = (0 + count - 1) / 2;
            int indexOfStart = _pathTrackList.IndexOf(_startEntity);//count-1
            int indexOfEnd = _pathTrackList.IndexOf(_endEntity);//0
            _spawnedEntityList.Remove(_startEntity);
            _spawnedEntityList.Remove(_endEntity);
            _CheckForLevelComplete();
            if (_pathTrackList.Count == 2)
            {
                Vector2 dir = (_pathTrackList[1].transform.localPosition + _pathTrackList[0].transform.localPosition) / 2;
                _pathTrackList[0].TweenToGivenPose(dir);
                _ShowStarParticles(dir);
                List<LinerendererHandler> path1 = _GetPath(_startEntity, _endEntity);
                _pathTrackList[1].TweenToGivenPose(dir, () =>
                {
                    _FadeOutPath(path1);
                });
                _ChangePathColor(path1);
                return;
            }
            List<Vector3> waypoints = new List<Vector3>();
            List<Vector3> waypoints2 = new List<Vector3>();

            for (int i = indexOfStart - 1; i >= midPoint; i--)
                waypoints.Add(_pathTrackList[i].transform.localPosition);
            for (int i = indexOfEnd + 1; i <= midPoint; i++)
                waypoints2.Add(_pathTrackList[i].transform.localPosition);
            List<LinerendererHandler> path = _GetPath(_startEntity, _endEntity);
            _ChangePathColor(path);
            _startEntity.TweenToThePath(waypoints.ToArray());
            _endEntity.TweenToThePath(waypoints2.ToArray(), () =>
             {
                 _ShowStarParticles(_pathTrackList[midPoint].transform.localPosition);
                 _FadeOutPath(path);
             });
        }
        /// <summary>
        /// This method will check for any occupied tiles are there in the board.
        /// If there are no tiles are occupied then level completed logic will run.<br></br>
        /// It will fade the screen-->clears board-->Starts next new level.
        /// </summary>
        private void _CheckForLevelComplete()
        {
            if (!_spawnedEntityList.Any(x => x.IsOccupied))
            {
                //levelComplete
                MyUtils.DelayedCallback(1f, () =>
                {
                    _fadeBgCanvasGroup.DOFade(1, .45f).onComplete += () =>
                    {
                        _ClearEntireBoard();
                        GlobalVariables.HighestUnlockedLevel++;
                        StartCoroutine(_InitLevel());
                    };
                });
            }
        }
        private void _ChangePathColor(List<LinerendererHandler> path)
        {
            foreach (LinerendererHandler line in path)
            {

                line.transform.SetAsLastSibling();
                line.ChangeColor(Color.green);
            }
        }
        private void _FadeOutPath(List<LinerendererHandler> path)
        {
            foreach (LinerendererHandler line in path)
                line.FadeLineRenderer();
        }

        /// <summary>
        /// This method will return the cached path between given start and end nodes.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<LinerendererHandler> _GetPath(TileEntity start, TileEntity end)
        {
            Pair key = _pairPathDict.Keys.ToList().Find(x => (x.firstTile == start.RowColumn && x.secondTile == end.RowColumn) ||
(x.firstTile == end.RowColumn && x.secondTile == start.RowColumn));
            return _pairPathDict[key];

        }

        /// <summary>
        /// This method will check if there are any blockers in the given path or not.<br></br>
        /// if there are blockers then it is not a valid path=>return false.<br></br>
        /// else<br></br>
        /// returns true.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool _IsPathValid(List<TileEntity> path)
        {
            foreach (TileEntity entity in path)
            {
                if (entity == _startEntity || entity == _endEntity) continue;
                if (entity.IsOccupied)
                    return false;
            }
            return true;
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

        /// <summary>
        /// This method is heart of the mechanic
        /// this will give loop through the neighbours of the given start entity recursively 
        /// until it finds the end node.(bcz end node should be a neighbour to some node right!!)
        /// 
        /// </summary>
        /// <param name="comingFrom"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
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
                _BackTraceToTheStartNode(end);
                return;
            }
            foreach (TileEntity entity in neighbours)
            {
                if (entity == comingFrom) continue;
                entity.parent = start;
                _LoopForDestinationEntity(start, entity, end);
            }
        }

        /// <summary>
        /// This method will loop through the parents of the given entity until it reaches top most parent(we can say Root node)
        /// returns the path from root node to given node.
        /// </summary>
        /// <param name="tileEntity"></param>
        private void _BackTraceToTheStartNode(TileEntity tileEntity)
        {
            _pathTrackList.Clear();
            TileEntity curr = tileEntity;
            while (curr != this._startEntity)
            {
                _pathTrackList.Add(curr);
                curr = curr.parent;
            }
            _pathTrackList.Add(_startEntity);
            MyUtils.Log($"Path Track::: {_pathTrackList.Count}");
        }


        //private void _DrawDebugLine()
        //{
        //    if (_pathTrackList.Count > 0)
        //    {
        //        List<Vector2> points = new List<Vector2>();
        //        foreach (TileEntity entity in _pathTrackList)
        //            points.Add(entity.transform.localPosition);
        //        linerenderer.Points = points.ToArray();
        //        linerenderer.SetAllDirty();
        //    }
        //}

        /// <summary>
        /// This method will reposition the particle system to the given position 
        /// and plays the particle system.
        /// </summary>
        /// <param name="localPose"></param>
        private void _ShowStarParticles(Vector2 localPose)
        {
            _starParticles.transform.localPosition = localPose;
            _starParticles.Play();
        }
        #endregion Private Methods

        #region Callbacks
        ///In this callback 
        ///Adds the selected entity into a stack.
        ///and Invokes Matching logic if the count is equal or greater than MIN_TILES_TO_MATCH value.
        ///if selected entity is not same as the previous selected enity then removes the previous selected entity 
        ///and adds new entity as selected entity into the stack.
        private void Callback_On_TileEntity_Selected(TileEntity selectedEntity)
        {
            if (_selectedTileStack.Count > 0)
                if (selectedEntity.ID != _selectedTileStack.Peek().ID)
                    _selectedTileStack.Pop().UnSelectThisEntity();
            _selectedTileStack.Push(selectedEntity);
            _ShowStarParticles(selectedEntity.transform.localPosition);
            if (_selectedTileStack.Count >= MIN_TILES_TO_MATCH)
                _CheckForMatch();
        }
        //Removes the unseleted entity from the stack
        private void Callback_On_TileEntity_UnSelected(TileEntity unSelectedEntity)
        {
            if (_selectedTileStack.Count > 0)
                _selectedTileStack.Pop();
        }
        #endregion Callbacks
    }
    public class Pair
    {
        public Vector2Int firstTile;
        public Vector2Int secondTile;
        public Pair(Vector2Int first, Vector2Int second)
        {
            firstTile = first;
            secondTile = second;
        }
    }
}