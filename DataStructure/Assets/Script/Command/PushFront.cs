using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PushFront : ICommand
{
    private int numberToAdd;
    private List<int> intList = new List<int>();

    public PushFront(List<int> list, int number)
    {
        intList = list;
        numberToAdd = number;
    }

    public void Execute()
    {
        intList.Insert(0, numberToAdd);
    }

    public void Undo()
    {
        if(intList.Count > 0)
        {
            intList.RemoveAt(0);
        }
    }
}
