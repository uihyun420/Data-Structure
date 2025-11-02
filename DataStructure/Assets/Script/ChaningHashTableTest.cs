using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Net.Http.Headers;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class ChaningHashTableText : MonoBehaviour
{
    [SerializeField] private TMP_InputField indexInputField;
    [SerializeField] private TMP_InputField keyInputField;

    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button clearButton;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject displayPrefab;

    private ChainingHashTable<string, string> hashTable;
    private LinkedList<GameObject> displayItems;

    private void Start()
    {
        hashTable = new ChainingHashTable<string, string>();
        displayItems = new LinkedList<GameObject>();

        addButton.onClick.AddListener(OnAddButtonClicked);
        removeButton.onClick.AddListener(OnRemoveButtonClicked);
        clearButton.onClick.AddListener(OnClearButtonClicked);
    }

    private void OnAddButtonClicked()
    {
        string indexText = indexInputField.text.Trim();
        string keyValue = keyInputField.text.Trim();

        if (string.IsNullOrEmpty(indexText) || string.IsNullOrEmpty(keyValue))
        {
            Debug.Log("값을 입력해주세요");
            return;
        }

        if (!int.TryParse(indexText, out int index))
        {
            Debug.Log("인덱스는 숫자여야 합니다.");
            return;
        }

        if (index < 0 || index >= hashTable.size)
        {
            Debug.Log($"인덱스는 0부터 {hashTable.size - 1} 사이여야 합니다.");
            return;
        }

        try
        {
            hashTable.AddAtIndex(index, keyValue, keyValue);
            indexInputField.text = string.Empty;
            keyInputField.text = string.Empty;
            UpdateDisPlay();
            Debug.Log($"인덱스 {index}에 키 '{keyValue}' 추가됨");
        }
        catch (System.Exception ex)
        {
            Debug.Log($"추가 실패: {ex.Message}");
        }
    }
    private void OnRemoveButtonClicked()
    {
        string indexText = indexInputField.text.Trim();

        if (string.IsNullOrEmpty(indexText))
        {
            Debug.Log("삭제할 값을 입력해주세요");
            return;
        }

        if(hashTable.Remove(indexText))
        {
            indexInputField.text = string.Empty;
            keyInputField.text = string.Empty;
            UpdateDisPlay();
        }
        else
        {
            Debug.Log("해당 키를 찾을 수 없습니다.");
        }
    }
    private void OnClearButtonClicked()
    {
        if(hashTable != null)
        {
            hashTable.Clear();
            indexInputField.text = string.Empty;
            keyInputField.text = string.Empty;
            UpdateDisPlay();
            Debug.Log("초기화 성공");
        }
        else
        {
            Debug.Log("초기화 실패");
        }
    }

    private void UpdateDisPlay()
    {
        foreach (var item in displayItems)
        {
            if (item != null)
                Destroy(item);
        }

        displayItems.Clear();

        for (int i = 0; i < hashTable.size; i++)
        {
            var bucket = hashTable.table[i];
            if (bucket != null)
            {
                foreach (var kvp in bucket)
                {
                    var newItem = Instantiate(displayPrefab, contentParent);
                    TextMeshProUGUI textComponent = newItem.GetComponentInChildren<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.text = $"I: {i} K: {kvp.Key} V: {kvp.Value}";
                    }
                    displayItems.AddLast(newItem);
                }
            }
        }
    }
}