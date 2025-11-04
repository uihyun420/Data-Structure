using System.Collections.Generic;
using UnityEngine;

public class IntListManager : MonoBehaviour
{
    [Header("테스트 설정")]
    [SerializeField] private int numberToAdd = 10;

    private List<int> intList = new List<int>();
    private CommandInvoker invoker = new CommandInvoker();

    private void Start()
    {
        Debug.Log("=== 커맨드 패턴 Undo/Redo 연습 시작 ===");
        PrintList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.W))
        {
            ExecutePushBack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.S))
        {
            ExecutePopBack();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.D))
        {
            PrintList();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Z))
        {
            ExecuteUndo();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.X))
        {
            ExecuteRedo();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ExecuteFrontPush();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ExecuteFrontPop();
        }
    }

    private void ExecutePushBack()
    {
        var command = new PushBackCommand(intList, numberToAdd);
        invoker.ExecuteCommand(command);
        Debug.Log($"추가됨 : {numberToAdd}");
    }

    private void ExecutePopBack()
    {
        var command = new PopBackCommand(intList);
        invoker.ExecuteCommand(command);
        Debug.Log($"삭제(뒤)");
    }

    private void ExecuteUndo()
    {
        if (invoker.CanUndo())
        {
            invoker.Undo();
            Debug.Log("<color=cyan>↶ Undo</color>");
            PrintList();
        }
        else
        {
            Debug.LogWarning("Undo할 명령이 없습니다!");
        }
    }

    private void ExecuteRedo()
    {
        if (invoker.CanRedo())
        {
            invoker.Redo();
            Debug.Log("<color=magenta>↷ Redo</color>");
            PrintList();
        }
        else
        {
            Debug.LogWarning("Redo할 명령이 없습니다!");
        }
    }

    private void PrintList()
    {
        Debug.Log($"현재 리스트: [{string.Join(", ", intList)}] (개수: {intList.Count})");
    }

    private void ExecuteFrontPush()
    {
        var command = new PushFront(intList, numberToAdd);
        invoker.ExecuteCommand(command);
        Debug.Log($"앞에 추가됨 : {numberToAdd}");
    }

    private void ExecuteFrontPop()
    {
        var command = new PopFront(intList);
        invoker.ExecuteCommand(command);
        Debug.Log($"삭제(앞)");
    }

}
