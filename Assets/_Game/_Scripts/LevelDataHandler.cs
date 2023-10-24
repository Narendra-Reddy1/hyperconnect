using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class LevelDataHandler
{
    ///11X5 grid
    private static List<LevelData> levelData = new List<LevelData>
    {
        //-1->hides the tile
        new LevelData(1,
            new int[8,5]
            {
                {-1,-1,0,-1,-1},
                {-1,-1,0,-1,-1},
                {-1,2,1,2,-1},
                {-1,-1,1,-1,-1},
                {-1,3,-1,-1,-1},
                {3,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1},
                {-1,-1,-1,-1,-1},
            },
            new List<Edge>
            {
                new Edge(new Vector2Int(0,2),new Vector2Int (1,2)),
                new Edge(new Vector2Int(1,2),new Vector2Int (2,1)),
                new Edge(new Vector2Int(2,1),new Vector2Int (2,2)),
                new Edge(new Vector2Int(2,2),new Vector2Int (2,3)),
                new Edge(new Vector2Int(2,2),new Vector2Int (3,2)),
                new Edge(new Vector2Int(3,2),new Vector2Int (4,1)),
                new Edge(new Vector2Int(4,1),new Vector2Int (5,0)),
            },
            false),

    };
    public static LevelData GetLevelDataByLevelIndex(int levelIndex)
    {
        return levelData.Find(x => x.level == levelIndex);
    }
    public static bool IsLevelDataValid()
    {
        return false;
    }
}
[System.Serializable]
public class LevelData
{
    public int level;
    public int[,] indexData;
    public List<Edge> edgeData;//Vector2 is start and endpoint data.//for line renderer;
    public bool allowDuplicates;
    public LevelData(int level, int[,] indexData, List<Edge> edgeData, bool allowDuplicates = false)
    {
        this.level = level;
        this.indexData = indexData;
        this.edgeData = edgeData;
        this.allowDuplicates = false;
    }
}
[System.Serializable]
public class Edge
{
    public Vector2Int start;
    public Vector2Int end;
    public Edge(Vector2Int start, Vector2Int end)
    {
        this.start = start;
        this.end = end;
    }

}
