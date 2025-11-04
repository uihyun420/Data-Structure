using UnityEngine;
using System;

public class BubbleSortStrategy<T> : ISortingStrategy<T> where T : IComparable<T>
{
    public void Sort(T[] array)
    {
        int length = array.Length - 1;

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length - i; j++)
            {
                if (array[j + 1].CompareTo(array[j]) < 0)
                {
                    var temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
    }
}
