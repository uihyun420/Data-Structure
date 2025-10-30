using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private Map map;
    private Stage stage;
    private float speed = 50f;
    int currentPathIndex = 0;
    bool isMoving = false;

    public void SetStage(Stage stage)
    {
        this.stage = stage;
    }

    public void SetMap(Map map)
    {
        this.map = map;
        MoveToValidPosition();
    }

    private void MoveToValidPosition()
    {
        if (map == null || stage == null) return;

        var currentTileId = GetCurrentTileId();
        if (currentTileId >= 0 && currentTileId < map.tiles.Length)
        {
            var currentTile = map.tiles[currentTileId];
            if (currentTile.CanMove) return;
        }

        Tile closestValidTile = FindClosestValidTile(currentTileId);

        if (closestValidTile != null)
        {
            Vector3 validPosition = stage.GetTilePos(closestValidTile.id);
            transform.position = validPosition;
        }
        else if (map.startTile != null)
        {
            Vector3 startPosition = stage.GetTilePos(map.startTile.id);
            transform.position = startPosition;
        }
    }

    private Tile FindClosestValidTile(int currentTileId)
    {
        if (map == null) return null;

        Tile closestTile = null;
        float minDistance = float.MaxValue;

        int currentX = currentTileId % map.cols;
        int currentY = currentTileId / map.cols;

        foreach (var tile in map.tiles)
        {
            if (tile.CanMove)
            {
                int tileX = tile.id % map.cols;
                int tileY = tile.id / map.cols;

                float distance = Mathf.Abs(currentX - tileX) + Mathf.Abs(currentY - tileY);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTile = tile;
                }
            }
        }

        return closestTile;
    }

    public void MoveToTile(int targetTileId)
    {
        Map currentMap = stage.Map;

        if (currentMap == null || targetTileId < 0 || targetTileId >= currentMap.tiles.Length)
            return;

        Tile targetTile = currentMap.tiles[targetTileId];
        if (targetTile == null || !targetTile.CanMove)
            return;

        var currentTileId = GetCurrentTileId();
        if (currentTileId < 0 || currentTileId >= currentMap.tiles.Length)
            return;

        Tile startTile = currentMap.tiles[currentTileId];
        if (startTile == null)
            return;

        if (!startTile.CanMove)
        {
            MoveToValidPosition();
            var newCurrentTileId = GetCurrentTileId();
            if (newCurrentTileId < 0 || newCurrentTileId >= currentMap.tiles.Length)
                return;
            startTile = currentMap.tiles[newCurrentTileId];
        }

        bool pathFound = currentMap.AStar(startTile, targetTile);

        if (pathFound && currentMap.path.Count > 0)
        {
            currentPathIndex = 0;
            isMoving = true;
        }
    }

    private int GetCurrentTileId()
    {
        if (stage != null)
        {
            return stage.WorldPosToTileId(transform.position);
        }
        return -1;
    }

    private void MovePlayerPosition()
    {
        Map currentMap = stage.Map;

        if (currentMap?.path == null || currentMap.path.Count == 0)
        {
            isMoving = false;
            return;
        }

        if (currentPathIndex < currentMap.path.Count)
        {
            var targetTile = currentMap.path[currentPathIndex];
            Vector3 targetPosition = stage.GetTilePos(targetTile.id);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                currentPathIndex++;

                if (currentPathIndex >= currentMap.path.Count)
                {
                    isMoving = false;
                }
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            MovePlayerPosition();
        }
    }
}