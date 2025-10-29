using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GraphTest : MonoBehaviour
{
    public enum Algorithm
    {
        DFS,
        BFS,
        DFSRecursive,
        PathFindingBFS,
    }

    public UiGraphNode nodePrefab;
    public List<UiGraphNode> uiNodes;
    public Transform uiNodeRoot;

    private Graph graph;
    private void Start()
    {
        int[,] map = new int[5, 5]
        {
            { 1,-1,1,1,1},
            { 1,-1,1,1,1},
            { 1,-1,1,1,1},
            { 1,-1,1,1,1},
            { 1,1,1,1,1}
        };

        graph = new Graph();
        graph.Init(map);
        InitUiNodes(graph);
    }

    public int startIndex;
    public int endIndex;


    public Algorithm algorithm;

    [ContextMenu("Search")]
    public void Search()
    {
        var search = new GraphSearch();
        search.Init(graph);

        switch (algorithm)
        {
            case Algorithm.DFS:
                search.DFS(graph.nodes[startIndex]);
                break;
            case Algorithm.BFS:
                search.BFS(graph.nodes[startIndex]);
                break;
            case Algorithm.DFSRecursive:
                break;
            case Algorithm.PathFindingBFS:
                break;
        }

        ResetUiNodes();

        for(int i = 0; i < search.path.Count; i++)
        {
            var node = search.path[i];
            var color = Color.Lerp(Color.red, Color.green, (float)i / (search.path.Count - 1));
            uiNodes[node.id].SetColor(color);
            uiNodes[node.id].SetText($"ID: {node.id}\nWeight: {node.weight}\nPath: {i}");
        }

    }
    private void InitUiNodes(Graph graph)
    {
        foreach(var node in graph.nodes)
        {
            var uiNode = Instantiate(nodePrefab, uiNodeRoot);
            uiNode.SetNode(node);
            uiNode.Reset();
            uiNodes.Add(uiNode);
        }
    }

    private void ResetUiNodes()
    {
        foreach (var uiNode in uiNodes)
        {
            uiNode.Reset(); 
        }
    }
}
