using UnityEngine;

public class SortContext<T> : ISortContext<T>
{
    private ISortingStrategy<T> sortStrategy;
    public void SetStrategy(ISortingStrategy<T> strategy)
    {
        sortStrategy = strategy;
    }

    public void Sort(T[] array)
    {
        sortStrategy.Sort(array);
    }
}
