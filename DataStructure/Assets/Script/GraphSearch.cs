using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GraphSearch : MonoBehaviour
{
    private Graph graph;
    public List<GraphNode> path = new List<GraphNode>();

    public void Init(Graph graph)
    {
        this.graph = graph; 
    }

    public void DFS(GraphNode node)
    {
        path.Clear();
        
        var visited = new HashSet<GraphNode>(); 
        var stack = new Stack<GraphNode>(); // DFS 구조는 스택이 필요함

        stack.Push(node);
        while (stack.Count > 0)
        {
            var currentNode = stack.Pop(); 
            path.Add(currentNode);
            visited.Add(currentNode); // 방문한 노드인지 아닌지 검사하기 위해
            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent) || stack.Contains(adjacent))
                    continue;

                stack.Push(adjacent);
            }
        }
    }
    public void BFS(GraphNode node)
    {
        path.Clear();

        var visited = new HashSet<GraphNode>();
        var queue = new Queue<GraphNode>(); // DFS 구조는 스택이 필요함

        queue.Enqueue(node);
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            path.Add(currentNode);
            visited.Add(currentNode); // 방문한 노드인지 아닌지 검사하기 위해
            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent) || queue.Contains(adjacent))
                    continue;

                queue.Enqueue(adjacent);
            }
        }
    }
}
