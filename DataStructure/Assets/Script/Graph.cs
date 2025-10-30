using UnityEngine;

public class Graph
{
    public int rows = 0;
    public int cols = 0;

    public GraphNode[] nodes;

    public void Init(int[,] grid)
    {
        rows = grid.GetLength(0);   
        cols = grid.GetLength(1);

        nodes = new GraphNode[grid.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new GraphNode();
            nodes[i].id = i;
        }

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int index = r * cols + c; // 2차원 배열의 인덱스를 1차원 인덱스로 변환
                nodes[index].weight = grid[r, c];

                if (grid[r, c] == -1)
                    continue;

                //Up
                if (r - 1 >= 0 && grid[r - 1, c] >= 0)
                {
                    nodes[index].adjacents.Add(nodes[index - cols]);
                }

                //Right
                if (c + 1 < cols && grid[r, c + 1] >= 0)
                {
                    nodes[index].adjacents.Add(nodes[index + 1]);
                }

                //Down
                if (r + 1 < rows && grid[r + 1, c] >= 0)
                {
                    nodes[index].adjacents.Add(nodes[index + cols]);
                }

                //Left
                if (c - 1 >= 0 && grid[r, c - 1] >= 0)
                {
                    nodes[index].adjacents.Add(nodes[index - 1]);
                }
            }
        }
    }


    public void ResetNodePrevious()
    {
        foreach(var node in nodes)
        {
            node.previous = null;
        }
    }
}
