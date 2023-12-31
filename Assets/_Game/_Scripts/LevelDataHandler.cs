using UnityEngine;
using System.Collections.Generic;
using System.Linq;


///Based on my observation in the HyperConnect game
///None of the levels have closed path with any tiles (which concludes there would be ony one way to reach any tile.)
///With that conclusion I've created this data set with IndexData and Edge data without any closed paths.
public static class LevelDataHandler
{

    ///8X5 grid
    ///


    ///LevelData is the dataHolder the each level in this game
    ///This has 3 parts
    /// (1)Level Index
    /// 
    /// 
    /// (2)Tiles Index Data[2D Array]
    /////This index data is used to sapwn the tiles image in the game.
    ///// -1 represents the invisible tile.
    /////any other value represents the index the tile. and this INDEX is mapped to SpriteDataBase ScriptableObject.
    /////Spritedatabase scriptable object has a list of the sprites of the tile images.
    /////this INDEX is used as ID to perform match operation.
    ///
    /// 
    /// (3)Edges Data-->This has the data which tile is neighbour to which tile.
    ///// Edge data is Index(ixj) of the 2D Array-->IndexData
    /////Edge Data provide the relationship between tiles.
    /////This data is used to conclude neighbours of the particular tile.
    /////and those neighbours are used to find the path.

    private static readonly List<LevelData> levelData = new List<LevelData>
    {

         new LevelData(1 ,
            new int[8,5] 
            {
                {-1,-1,5,-1,-1 },//0
                {-1,0,1,3,5 },//1
                {-1,1,0,3,-1 },//2
                {-1,-1,-1,-1,-1 },//3
                {-1,-1,-1,-1,-1 },//4
                {-1,-1,-1,-1,-1 },//5
                {-1,-1,-1,-1,-1 },//6
                {-1,-1,-1,-1,-1 },//7
            },
            new List<Edge>
            {
                new Edge(new Vector2Int(0,2),new Vector2Int(1,1)),//5->0
                new Edge(new Vector2Int(0,2),new Vector2Int(1,2)),//5->1
                new Edge(new Vector2Int(0,2),new Vector2Int(1,3)),//5->3
                
                new Edge(new Vector2Int(1,1),new Vector2Int(2,1)),//0->3
                new Edge(new Vector2Int(1,1),new Vector2Int(2,2)),//0->0
                new Edge(new Vector2Int(1,3),new Vector2Int(2,3)),//0->1
                
                new Edge(new Vector2Int(1,3),new Vector2Int(1,4)),//0->1           
            }),

        new LevelData(2  ,
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

          new LevelData(3,
            new int[8,5]
            {
                {-1,-1,5,-1,-1 },//0
                {-1,0,1,3,5 },//1
                {-1,1,0,3,-1 },//2

                {2,4,4,-1,7 },//3
                {-1,6,2,-1,6 },//4
                {-1,7,9,10,-1 },//5
                {9,-1,-1,11,-1 },//6
                {-1,10,11,-1,-1 },//7

            },
            new List<Edge>
            {
                new Edge(new Vector2Int(0,2),new Vector2Int(1,1)),//5->0
                new Edge(new Vector2Int(0,2),new Vector2Int(1,2)),//5->1
                new Edge(new Vector2Int(0,2),new Vector2Int(1,3)),//5->3
                
                new Edge(new Vector2Int(1,1),new Vector2Int(2,1)),//0->3
                new Edge(new Vector2Int(1,1),new Vector2Int(2,2)),//0->0
                new Edge(new Vector2Int(1,3),new Vector2Int(2,3)),//0->1
                
                new Edge(new Vector2Int(1,3),new Vector2Int(1,4)),//3->5
                
                new Edge(new Vector2Int(2,1),new Vector2Int(3,0)),//3->5
                new Edge(new Vector2Int(2,1),new Vector2Int(3,1)),
                new Edge(new Vector2Int(2,1),new Vector2Int(3,2)),

                new Edge(new Vector2Int(2,3),new Vector2Int(3,4)),

                new Edge(new Vector2Int(3,1),new Vector2Int(4,1)),
                new Edge(new Vector2Int(3,1),new Vector2Int(4,2)),

                new Edge(new Vector2Int(4,2),new Vector2Int(5,1)),
                new Edge(new Vector2Int(4,2),new Vector2Int(5,2)),
                new Edge(new Vector2Int(4,2),new Vector2Int(5,3)),

                new Edge(new Vector2Int(5,1),new Vector2Int(6,0)),
                new Edge(new Vector2Int(5,2),new Vector2Int(7,1)),
                new Edge(new Vector2Int(5,2),new Vector2Int(7,2)),
                new Edge(new Vector2Int(5,3),new Vector2Int(6,3)),
                new Edge(new Vector2Int(4,2),new Vector2Int(4,4)),

            }),

    };
    public static LevelData GetLevelDataByLevelIndex(int levelIndex)
    {

        LevelData data = levelData.Find(x => x.level == levelIndex);
        if (data != null)
            return data;
        else
            return data = levelData.Find(x => x.level == GetMappedLevel(levelIndex));
    }

    private static int GetMappedLevel(int level)//dummy Logic
    {
        if (level % 2 == 0)
            return 2;
        else
            return 3;
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
