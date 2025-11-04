using NUnit.Framework;
using System.Collections.Generic;

public class PushBackCommand : ICommand
{
    private List<int> intList = new List<int>();
    public int numberToAdd;

    public PushBackCommand(List<int> list, int number)
    {
        intList = list;
        numberToAdd = number;
    }

    public void Execute()
    {
        intList.Add(numberToAdd);
    }

    public void Undo()
    {
        if(intList.Count > 0)
        {
            intList.RemoveAt(intList.Count - 1);
        }
    }
}

