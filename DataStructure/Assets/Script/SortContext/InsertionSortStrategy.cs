using UnityEngine;
using System.Collections.Generic;


public class InsertionSortStrategy<T> : ISortingStrategy<T>
{

    public void Sort(T[] array)
    {
        int n = array.Length;
        var comparer = Comparer<T>.Default;

        for (int i = 1; i < n; i++)
        {
            T key = array[i];
            int j = i - 1;

            while (j >= 0 && comparer.Compare(array[j], key) > 0)
            {
                array[j + 1] = array[j];
                j--;
            }
            array[j + 1] = key;
        }
    }
}
