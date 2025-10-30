using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum TileTypes
{
    Empty = -1,
    // 0, 14
    Grass = 15,
    Tree = 16,
    Hills = 17,
    Mountains = 18,
    Towns = 19,
    Castle = 20,
    Monster = 21
}

public class Map
{
    public int rows = 0;
    public int cols = 0;

    public Tile[] tiles;

    public Tile castleTile;
    public Tile startTile;

    public List<Tile> path = new List<Tile>(); // Astar 경로 결과를 저장할 리스트 

    public Tile[] CoastTiles
    {

        get
        {
            return tiles.Where(t => t.autoTileId < (int)TileTypes.Grass).ToArray();
        }
    }

    public Tile[] LandTiles
    {
        get
        {
            return tiles.Where(t => t.autoTileId >= (int)TileTypes.Grass).ToArray();
        }
    }

    public void Init(int rows, int cols)   // 0: O 1: X
    {
        this.rows = rows;
        this.cols = cols;

        tiles = new Tile[rows * cols];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new Tile();
            tiles[i].id = i;
        }

        for (var r = 0; r < rows; ++r)
        {
            for (var c = 0; c < cols; ++c)
            {
                var index = r * cols + c;

                var indexU = (r - 1) * cols + c;
                var indexR = r * cols + c + 1;
                var indexD = (r + 1) * cols + c;
                var indexL = r * cols + c - 1;

                if ((r - 1) >= 0)
                {
                    tiles[index].adjacents[(int)Sides.Top] = tiles[indexU];
                }
                if (c + 1 < cols)
                {
                    tiles[index].adjacents[(int)Sides.Right] = tiles[indexR];
                }
                if (r + 1 < rows)
                {
                    tiles[index].adjacents[(int)Sides.Bottom] = tiles[indexD];
                }
                if (c - 1 >= 0)
                {
                    tiles[index].adjacents[(int)Sides.Left] = tiles[indexL];
                }
            }
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].UpdateAuotoTileId();
            tiles[i].UpdateAuotoFowId();
        }
    }

    public bool CreateIsland(
        float erodePercent,
        int erodeIterations,
        float lakePercent,
        float treePercent,
        float hillPercent,
        float mountainPercent,
        float townPercent,
        float monsterPercent)
    {
        DecorateTiles(LandTiles, lakePercent, TileTypes.Empty);

        for (int i = 0; i < erodeIterations; ++i)
            DecorateTiles(CoastTiles, erodePercent, TileTypes.Empty);

        DecorateTiles(LandTiles, treePercent, TileTypes.Tree);
        DecorateTiles(LandTiles, hillPercent, TileTypes.Hills);
        DecorateTiles(LandTiles, mountainPercent, TileTypes.Mountains);
        DecorateTiles(LandTiles, townPercent, TileTypes.Towns);
        DecorateTiles(LandTiles, monsterPercent, TileTypes.Monster);

        var towns = tiles.Where(x => x.autoTileId == (int)TileTypes.Towns).ToArray();


        if (towns.Length > 0) // 성 추가
        {
            ShuffleTiles(towns);
            startTile = towns[0];

            castleTile = null;
            for(int i = 0; i < towns.Length; i++)
            {
                var randomIndex = Random.Range(0, towns.Length);
                var canCreateCastle = towns[randomIndex];
                castleTile = canCreateCastle;
                castleTile.autoTileId = (int)TileTypes.Castle;

                if(FindPathToCastle() && startTile != castleTile)
                {
                    break;
                }

                else
                {
                    canCreateCastle.autoTileId = (int)TileTypes.Towns;
                    castleTile = null;
                }
            }
        }
        else
        {
            startTile = null;
            castleTile = null;
        }

        return true;
    }

    public void DecorateTiles(Tile[] tiles, float percent, TileTypes tileType)
    {
        int total = Mathf.FloorToInt(tiles.Length * percent);

        ShuffleTiles(tiles);

        for (int i = 0; i < total; ++i)
        {
            if (tileType == TileTypes.Empty)
                tiles[i].ClearAdjacents();

            tiles[i].autoTileId = (int)tileType;
        }
    }

    public void ShuffleTiles(Tile[] tiles)
    {
        // Fisher-Yates 셔플 알고리즘 구현
        for (int i = tiles.Length - 1; i > 0; i--)
        {
            // 0과 i 사이의 무작위 인덱스 선택
            int randomIndex = Random.Range(0, i + 1);

            // i번째 요소와 무작위로 선택된 요소 교환
            Tile temp = tiles[i];
            tiles[i] = tiles[randomIndex];
            tiles[randomIndex] = temp;
        }
    }

    public void ResetTilePrevious()
    {
        foreach (var tile in tiles)
        {
            tile.previous = null;
        }
    }

    protected int Heuristic(Tile a, Tile b)
    {
        int ax = a.id % cols;
        int ay = a.id / cols;

        int bx = b.id % cols;
        int by = b.id / cols;

        return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by);
    }


    public bool AStar(Tile start, Tile goal)
    {
        path.Clear();
        ResetTilePrevious();

        var visited = new HashSet<Tile>();
        var pQueue = new PriorityQueue<Tile, int>();
        var distances = new int[tiles.Length];
        var scores = new int[tiles.Length];

        for (int i = 0; i < distances.Length; i++)
        {
            scores[i] = distances[i] = int.MaxValue;
        }

        distances[start.id] = start.Weight;
        scores[start.id] = distances[start.id] + Heuristic(start, goal);
        pQueue.Enqueue(start, scores[start.id]);

        bool success = false;

        while (pQueue.Count > 0)
        {
            var currentTile = pQueue.Dequeue();

            if (visited.Contains(currentTile))
                continue;

            if (currentTile == goal)
            {
                success = true;
                break;
            }

            visited.Add(currentTile);

            foreach (var adj in currentTile.adjacents)
            {
                if (adj == null || !adj.CanMove || visited.Contains(adj))
                {
                    continue;
                }

                var newDistances = distances[currentTile.id] + adj.Weight;

                if (distances[adj.id] > newDistances)
                {
                    distances[adj.id] = newDistances;
                    scores[adj.id] = distances[adj.id] + Heuristic(adj, goal);
                    adj.previous = currentTile;
                    pQueue.Enqueue(adj, scores[adj.id]);
                }
            }
        }

        if (!success)
        {
            return false;
        }

        Tile step = goal;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }

        path.Reverse();
        return true;
    }

    public bool FindPathToCastle()
    {
        if (startTile == null || castleTile == null)
        {
            Debug.Log("시작 타일 또는  성 타일이 없습니다.");
            return false;
        }

        return AStar(startTile, castleTile);
    }
}


