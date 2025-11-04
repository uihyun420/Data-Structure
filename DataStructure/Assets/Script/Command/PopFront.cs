using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PopFront : ICommand
{
    private List<int> intList = new List<int>();   
    private int removedNumber;
    public  PopFront(List<int> intList)
    {
        this.intList = intList;
    }

    public void Execute()
    {
        removedNumber = intList[0];
        intList.RemoveAt(0);
    }

    public void Undo()
    {
        intList.Insert(0, removedNumber);
    }
}
