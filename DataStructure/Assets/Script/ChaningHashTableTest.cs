using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Net.Http.Headers;
using UnityEditorInternal.Profiling.Memory.Experimental;
using System.Text;

public class ChaningHashTableText : MonoBehaviour
{
    [SerializeField] private TMP_InputField keyInputField;
    [SerializeField] private TMP_InputField valueInputField;

    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button clearButton;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject displayPrefab;

    [SerializeField] private TextMeshProUGUI displayKeyText;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform textTransform;

    private ChainingHashTable<string, string> hashTable;
    private LinkedList<GameObject> displayItems;

    private bool add = false;
    private bool remove = false;
    private bool clear = false;

    private void Start()
    {
        hashTable = new ChainingHashTable<string, string>();
        displayItems = new LinkedList<GameObject>();

        addButton.onClick.AddListener(OnAddButtonClicked);
        removeButton.onClick.AddListener(OnRemoveButtonClicked);
        clearButton.onClick.AddListener(OnClearButtonClicked);

        add = false;
        remove = false;
        clear = false;

        SetInitialDisplay();
    }

    private void OnAddButtonClicked()
    {
        string valueText = valueInputField.text.Trim();
        string keyText = keyInputField.text.Trim();

        if (string.IsNullOrEmpty(valueText) || string.IsNullOrEmpty(keyText))
        {
            Debug.Log("값을 입력해주세요");
            add = false;
            return;
        }

        if (!hashTable.ContainsKey(keyText))
        {
            add = true;
            hashTable.Add(keyText, valueText);

            SetDisplayKeyText();

            valueInputField.text = string.Empty;
            keyInputField.text = string.Empty;

            UpdateDisPlay();
            add = false;
        }

        else
        {
            Debug.Log("중복된 키 입니다.");
        }

    }

    private void OnRemoveButtonClicked()
    {
        string indexText = keyInputField.text.Trim();

        if (string.IsNullOrEmpty(indexText))
        {
            Debug.Log("삭제할 값을 입력해주세요");
            return;
        }

        if (hashTable.Remove(indexText))
        {
            remove = true;
            SetDisplayKeyText();
            keyInputField.text = string.Empty;
            valueInputField.text = string.Empty;
            UpdateDisPlay();
            remove = false;
        }
        else
        {
            Debug.Log("해당 키를 찾을 수 없습니다.");
        }
    }
    private void OnClearButtonClicked()
    {
        if (hashTable != null)
        {
            clear = true;
            hashTable.Clear();
            SetDisplayKeyText();

            valueInputField.text = string.Empty;
            keyInputField.text = string.Empty;

            UpdateDisPlay();
            clear = false;
            Debug.Log("초기화 성공");
        }
        else
        {
            Debug.Log("초기화 실패");
        }
    }

    private void UpdateDisPlay()
    {
        int idx = 0;

        foreach (var item in displayItems)
        {
            var textComponent = item.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                var bucket = hashTable.table[idx];
                if (bucket != null && bucket.Count > 0)
                {
                    var sb = new StringBuilder();
                    sb.Clear();
                    sb.Append($"I : {idx}");

                    foreach (var kvp in bucket)
                    {
                        sb.Append($"K : {kvp.Key}, V : {kvp.Value}");
                    }
                    textComponent.text = sb.ToString();
                }
                else
                {
                    var sb = new StringBuilder();   
                    sb.Clear();
                    sb.Append($"I : {idx}");
                    textComponent.text = sb.ToString();
                }
                idx++;
            }
        }
    }

    private void SetDisplayKeyText()
    {
        string keyText = string.Empty;

        if (keyInputField != null)
        {
            keyText = keyInputField.text.Trim();
        }
        else
        {
            Debug.Log("key 값이 없습니다");
        }
        
        if(add)
        {
            displayKeyText.text = $"ADD : {keyText} -> ";
        }
        if (remove)
        {
            displayKeyText.text = $"REMOVE : {keyText} -> ";
        }
        if (clear)
        {
            displayKeyText.text = $"CLEAR!";
        }

        Instantiate(textPrefab, textTransform);
    }

    private void SetInitialDisplay()
    {
        foreach(var item in displayItems)
        {
            Destroy(item);
        }

        displayItems.Clear();

        var count = hashTable.size;

        for(int i = 0; i < count; i++)
        {
            var newItem = Instantiate(displayPrefab, contentParent);
            var textComponent = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                var sb = new StringBuilder();
                sb.Clear();
                sb.Append($"I : {i}");
                textComponent.text = sb.ToString(); 
            }
            displayItems.AddLast(newItem);
        }
    }
}