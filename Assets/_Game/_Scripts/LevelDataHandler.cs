using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class LevelDataHandler
{
    ///11X5 grid
    private static List<LevelData> levelData = new List<LevelData>
    {

#region dat
        //-1->hides the tile
        //0,1,2,3,4,5,6,7,8
        //new LevelData(1,
        //    new int[8,5]
        //    {
        //        {4,8,0,5,4},//0
        //        {-1,-1,3,5,-1},//1
        //        {-1,2,1,2,-1},//2
        //        {-1,8,1,-1,-1},//3
        //        {-1,3,-1,6,-1},//4
        //        {0,-1,-1,7,-1},//5
        //        {-1,7,-1,-1,-1},//6
        //        {6,-1,-1,-1,-1},//7
        //    },
        //    new List<Edge>
        //    {
        //        new Edge(new Vector2Int(0,2),new Vector2Int (1,2)),
        //        new Edge(new Vector2Int(0,2),new Vector2Int (0,3)),
        //        new Edge(new Vector2Int(1,2),new Vector2Int (1,3)),
        //        new Edge(new Vector2Int(1,2),new Vector2Int (2,1)),
        //        new Edge(new Vector2Int(2,1),new Vector2Int (2,2)),
        //        new Edge(new Vector2Int(2,2),new Vector2Int (2,3)),
        //        new Edge(new Vector2Int(2,2),new Vector2Int (3,2)),
        //        new Edge(new Vector2Int(3,2),new Vector2Int (4,1)),
        //        new Edge(new Vector2Int(4,1),new Vector2Int (5,0)),
        //        new Edge(new Vector2Int(4,1),new Vector2Int (4,3)),

        //        new Edge(new Vector2Int(4,1),new Vector2Int (5,3)),
        //        new Edge(new Vector2Int(5,0),new Vector2Int (6,1)),
        //        new Edge(new Vector2Int(5,0),new Vector2Int (7,0)),

        //        new Edge(new Vector2Int(0,0),new Vector2Int (0,2)),
        //        new Edge(new Vector2Int(0,3),new Vector2Int (0,4)),

        //    }),

        //0,1, 2,3,4,5,6,7,8,9,10
#endregion dat

        new LevelData(1  ,
            new int[8,5]
            {
                {0,1,8,-1,4 },//0
                {-1,0,3,4,1 },//1
                {-1,2,5,6,-1 },//2
                {2,-1,5,-1,6 },//3
                {-1,7,-1,-1,7 },//4
                {8,3,9,-1,10 },//5
                {-1,9,10,-1,-1 },//6
                {-1,-1,-1,-1,-1 },//7
            },
            new List<Edge>
            {
                new Edge(new Vector2Int(0,0),new Vector2Int(0,1)),//0->1
                new Edge(new Vector2Int(0,1),new Vector2Int(0,2)),//1->3
                new Edge(new Vector2Int(0,1),new Vector2Int(1,1)),//1->0
                new Edge(new Vector2Int(0,2),new Vector2Int(0,4)),//3->4
                new Edge(new Vector2Int(0,2),new Vector2Int(1,2)),//3->3
                new Edge(new Vector2Int(1,2),new Vector2Int(1,3)),//3->4
                new Edge(new Vector2Int(1,3),new Vector2Int(1,4)),//4->1
                
                new Edge(new Vector2Int(1,2),new Vector2Int(2,2)),//3->5
                new Edge(new Vector2Int(2,2),new Vector2Int(2,1)),//5->2
                new Edge(new Vector2Int(2,2),new Vector2Int(2,3)),//5->6
                
                new Edge(new Vector2Int(2,2),new Vector2Int(3,2)),//5->5
                new Edge(new Vector2Int(3,2),new Vector2Int(3,0)),//5->2
                new Edge(new Vector2Int(3,2),new Vector2Int(3,4)),//5->6

                new Edge(new Vector2Int(3,2),new Vector2Int(4,1)),//5->7
                new Edge(new Vector2Int(3,4),new Vector2Int(4,4)),//6->7
                
                new Edge(new Vector2Int(4,1),new Vector2Int(5,0)),//7->8
                new Edge(new Vector2Int(4,1),new Vector2Int(5,1)),//7->8
                
                new Edge(new Vector2Int(5,1),new Vector2Int(5,2)),//8->9
                new Edge(new Vector2Int(5,2),new Vector2Int(5,4)),//9->10
                new Edge(new Vector2Int(5,1),new Vector2Int(6,1)),//8->9
                new Edge(new Vector2Int(6,1),new Vector2Int(6,2)),//9->10



            }),

         new LevelData(2  ,
            new int[8,5]
            {
                {11,1,0,-1,4 },//0
                {-1,11,3,4,1 },//1
                {-1,2,5,6,-1 },//2
                {2,-1,5,-1,6 },//3
                {-1,7,-1,-1,7 },//4
                {0,3,9,-1,10 },//5
                {-1,9,10,-1,-1 },//6
                {-1,-1,-1,-1,-1 },//7
            },
            new List<Edge>
            {
                new Edge(new Vector2Int(0,0),new Vector2Int(0,1)),//0->1
                new Edge(new Vector2Int(0,1),new Vector2Int(0,2)),//1->3
                new Edge(new Vector2Int(0,1),new Vector2Int(1,1)),//1->0
                new Edge(new Vector2Int(0,2),new Vector2Int(0,4)),//3->4
                new Edge(new Vector2Int(0,2),new Vector2Int(1,2)),//3->3
                new Edge(new Vector2Int(1,2),new Vector2Int(1,3)),//3->4
                new Edge(new Vector2Int(1,3),new Vector2Int(1,4)),//4->1
                
                new Edge(new Vector2Int(1,2),new Vector2Int(2,2)),//3->5
                new Edge(new Vector2Int(2,2),new Vector2Int(2,1)),//5->2
                new Edge(new Vector2Int(2,2),new Vector2Int(2,3)),//5->6
                
                new Edge(new Vector2Int(2,2),new Vector2Int(3,2)),//5->5
                new Edge(new Vector2Int(3,2),new Vector2Int(3,0)),//5->2
                new Edge(new Vector2Int(3,2),new Vector2Int(3,4)),//5->6

                new Edge(new Vector2Int(3,2),new Vector2Int(4,1)),//5->7
                new Edge(new Vector2Int(3,4),new Vector2Int(4,4)),//6->7
                
                new Edge(new Vector2Int(4,1),new Vector2Int(5,0)),//7->8
                new Edge(new Vector2Int(4,1),new Vector2Int(5,1)),//7->8
                
                new Edge(new Vector2Int(5,1),new Vector2Int(5,2)),//8->9
                new Edge(new Vector2Int(5,2),new Vector2Int(5,4)),//9->10
                new Edge(new Vector2Int(5,1),new Vector2Int(6,1)),//8->9
                new Edge(new Vector2Int(6,1),new Vector2Int(6,2)),//9->10



            }),
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
    public LevelData(int level, int[,] indexData, List<Edge> edgeData)
    {
        this.level = level;
        this.indexData = indexData;
        this.edgeData = edgeData;
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
