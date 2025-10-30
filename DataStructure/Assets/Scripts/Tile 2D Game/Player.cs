using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

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
    }

    public void MoveToTile(int targetTileId)
    {
        Map currentMap = stage.Map;

        if (currentMap == null || targetTileId < 0 || targetTileId >= currentMap.tiles.Length)
        {
            Debug.Log("유효하지 않은 타일");
            return;
        }        

        Tile targetTile = currentMap.tiles[targetTileId];

        if (!targetTile.CanMove || targetTile == null)
        {
            Debug.Log("이동 못함");
            return;
        }

        var currentTileId = GetCurrentTileId();
        Tile startTile = currentMap.tiles[currentTileId];
        bool pathFound = currentMap.AStar(startTile, targetTile);

        if(pathFound && currentMap.path.Count > 0)
        {
            currentPathIndex = 0;
            isMoving = true;
            Debug.Log($"경로 찾기 성공! 경로 길이: {currentMap.path.Count}");
        }
        else
        {
            Debug.Log("경로 찾기 실패");
        }
    }

    private int  GetCurrentTileId()
    {
        if(stage != null)
        {
            return stage.WorldPosToTileId(transform.position);
        }
        return -1;
    }

    public void Search()
    {
        bool pathFound = map.AStar(map.startTile, map.castleTile);
        if (pathFound && map.path.Count > 0)
        {
            currentPathIndex = 0;
            isMoving = true;
            Debug.Log($"경로 찾기 성공! 경로 길이: {map.path.Count}");
        }
        else
        {
            Debug.Log("경로를 찾을 수 없습니다!");
        }
    }

    private void MovePlayerPosition()
    {
        if (map.path == null || map.path.Count == 0)
        {
            isMoving = false;
            return;
        }

        if (currentPathIndex < map.path.Count)
        {
            var targetTile = map.path[currentPathIndex];
            Vector3 targetPosition;

            if (stage != null)
            {
                targetPosition = stage.GetTilePos(targetTile.id);
            }
            else
            {
                // fallback 좌표 계산
                targetPosition = new Vector3(targetTile.id % map.cols, 0, targetTile.id / map.cols);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                currentPathIndex++;

                if (currentPathIndex >= map.path.Count)
                {
                    isMoving = false;
                    Debug.Log("성에 도착했습니다!");
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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Search();
        }

        if (isMoving)
        {
            MovePlayerPosition();
        }
    }

}
