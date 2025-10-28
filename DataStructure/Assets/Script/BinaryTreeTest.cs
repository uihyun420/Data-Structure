using UnityEngine;
using UnityEngine.UI;

public class BinaryTreeTest : MonoBehaviour
{
    public BinaryTreeVisualizer treeVisualizer;

    private VisualizableBST<int, string> tree; 

    [SerializeField] private int nodeCount = 10;
    [SerializeField] private int minKey = 1;
    [SerializeField] private int maxKey = 1000;

    [SerializeField] private Button levelOrderButton;
    [SerializeField] private Button inOrderButton;
    [SerializeField] private Button preOrderButton;
    [SerializeField] private Button postOrderButton;

    private void Start()    
    {
        GenerateRandomTree();
        levelOrderButton.onClick.AddListener(() => OnLevelOrderButtonClicked());
        inOrderButton.onClick.AddListener(() => OnInOrderButtonClicked());
    }

    public void GenerateRandomTree()
    {
        tree = new VisualizableBST<int, string>();

        int addedNodes = 0;
        while (addedNodes < nodeCount)
        {
            int key = Random.Range(minKey, maxKey + 1);

            if (!tree.ContainsKey(key))
            {
                string value = $"V-{key}";
                tree.Add(key, value);
                addedNodes++;
            }
        }

        treeVisualizer.VisualizeTree(tree);
    }

    [ContextMenu("Generate New Random Tree")]
    public void RegenerateTree()
    {
        GenerateRandomTree();
    }

    private void OnLevelOrderButtonClicked()
    {
        if (tree == null)
            return;

        Debug.Log("레벨 순회");
        foreach(var kvp in tree.LevelOrderTraversal())
        {
            Debug.Log($"Key: {kvp.Key}, Value : {kvp.Value}");
        }
    }

    private void OnInOrderButtonClicked()
    {
        if (tree == null)
            return;

        Debug.Log("중위 순회");
        foreach(var kvp in tree.InOrderTraversal())
        {
            Debug.Log($"Key: {kvp.Key}, Value : {kvp.Value}");
        }
    }

    private void OnPreOrderButtonClicked()
    {
        if(tree == null)
            return;

        Debug.Log("전위 순회");
        foreach(var kvp in tree.PreOrderTraversal())
        {
            Debug.Log($"Key: {kvp.Key}, Value : {kvp.Value}");
        }
    }

    private void OnPostOrderButtonClicked()
    {
        if (tree == null)
            return;

        Debug.Log("후위 순회");
        foreach (var kvp in tree.PreOrderTraversal())
        {
            Debug.Log($"Key: {kvp.Key}, Value : {kvp.Value}");
        }
    }
}