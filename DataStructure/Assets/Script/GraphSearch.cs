using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GraphSearch
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
        var stack = new Stack<GraphNode>();

        stack.Push(node);
        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();
            path.Add(currentNode);
            visited.Add(currentNode);
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
        var queue = new Queue<GraphNode>();

        queue.Enqueue(node);
        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            path.Add(currentNode);
            visited.Add(currentNode);
            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent) || queue.Contains(adjacent))
                    continue;

                queue.Enqueue(adjacent);
            }
        }
    }

    public void DFSRecursive(GraphNode node)
    {
        path.Clear();
        DFSRecursive(node, new HashSet<GraphNode>());
    }

    protected void DFSRecursive(GraphNode node, HashSet<GraphNode> visited)
    {
        path.Add(node);
        visited.Add(node);
        foreach (var adjacent in node.adjacents)
        {
            if (!adjacent.CanVisit || visited.Contains(adjacent))
                continue;
            DFSRecursive(adjacent, visited);
        }
    }

    public bool PathFindingBFS(GraphNode startNode, GraphNode endNode)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var visited = new HashSet<GraphNode>();
        var queue = new Queue<GraphNode>();

        queue.Enqueue(startNode);
        bool success = false;

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            if (currentNode == endNode)
            {
                success = true;
                break;
            }

            visited.Add(currentNode);
            foreach (var adjacent in currentNode.adjacents)
            {
                if (!adjacent.CanVisit || visited.Contains(adjacent) || queue.Contains(adjacent))
                    continue;

                adjacent.previous = currentNode;
                queue.Enqueue(adjacent);
            }
        }

        if (!success)
        {
            return false;
        }

        GraphNode step = endNode;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }

        path.Reverse();
        return true;
    }

    public bool Dikjstra(GraphNode start, GraphNode goal)
    {
        path.Clear();
        graph.ResetNodePrevious(); // 경로 역추적 초기화

        var visited = new HashSet<GraphNode>(); // 방문한 노드 체크 
        var pQueue = new PriorityQueue<GraphNode, int>(); // 노드들을 담을 우선순위 큐 , 방문 안한 노드들이 담기는 거임 
        var distances = new int[graph.nodes.Length]; 

        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = int.MaxValue; 
        }

        distances[start.id] = start.weight; // 시작 노드까지의 거리를 노드의 가중치로 초기화 
        pQueue.Enqueue(start, distances[start.id]);

        bool success = false;

        while(pQueue.Count > 0)
        {
            var currentNode = pQueue.Dequeue();
            if(visited.Contains(currentNode))
            {
                continue; 
            }

            if(currentNode == goal)
            {
                success = true;
                break;
            }

            visited.Add(currentNode);

            foreach(var adjacent in currentNode.adjacents)
            {
                if(!adjacent.CanVisit || visited.Contains(adjacent))
                {
                    continue;
                }

                var newDistance = distances[currentNode.id] + adjacent.weight; // 새로운 노드 까지 걸리는 비용

                if (distances[adjacent.id] > newDistance) // 새로운 비용이 더 작아야 갱신
                {
                    distances[adjacent.id] = newDistance;
                    adjacent.previous = currentNode;
                    pQueue.Enqueue(adjacent, newDistance);
                }
            }
        }

        if(!success)
        {
            return false;
        }

        GraphNode step = goal;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;   
        }

        path.Reverse();
        return true;    
    }

    //일반적으로 A* 알고리즘으로 길 찾는다 
    protected int Heuristic(GraphNode a, GraphNode b) // 목표까지의 거리
    {
        int ax = a.id % graph.cols;
        int ay = a.id / graph.cols;

        int bx = b.id % graph.cols;
        int by = b.id / graph.cols;

        return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by);
    }

    public bool AStar(GraphNode start, GraphNode goal)
    {
        path.Clear();
        graph.ResetNodePrevious(); // 경로 역추적 초기화

        var visited = new HashSet<GraphNode>(); // 방문한 노드 체크 
        var pQueue = new PriorityQueue<GraphNode, int>(); // 노드들을 담을 우선순위 큐 , 방문 안한 노드들이 담기는 거임 
        var distances = new int[graph.nodes.Length];
        var scores = new int[graph.nodes.Length]; 

        for (int i = 0; i < distances.Length; i++)
        {            
            scores[i] = distances[i] = int.MaxValue;
        }

        distances[start.id] = start.weight;
        scores[start.id] = distances[start.id] + Heuristic(start, goal); // 앞의 인자는 그때그때 달라짐 뒤에 인자는 고정
        pQueue.Enqueue(start, scores[start.id]);

        bool success = false;

        while (pQueue.Count > 0)
        {
            var currentNode = pQueue.Dequeue();

            if(visited.Contains(currentNode))
            {
                continue;
            }

            if(currentNode == goal)
            {
                success = true;
                break;
            }

            visited.Add(currentNode);
           
            foreach(var adjacent in currentNode.adjacents)
            {
                if(!adjacent.CanVisit || visited.Contains(adjacent))
                {
                    continue;
                }

                var newDistance = distances[currentNode.id] + adjacent.weight;
                if (distances[adjacent.id] > newDistance)
                {
                    distances[adjacent.id] = newDistance;
                    scores[adjacent.id] = distances[adjacent.id] + Heuristic(adjacent, goal);
                    adjacent.previous = currentNode;
                    pQueue.Enqueue(adjacent, scores[adjacent.id]);
                }
            }
        }


        if (!success)
        {
            return false;
        }

        GraphNode step = goal;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }

        path.Reverse();
        return true;
    }

}
