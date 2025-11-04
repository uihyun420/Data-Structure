using UnityEngine;

public interface ISortContext<T>
{
    void SetStrategy(ISortingStrategy<T> strategy);
    void Sort(T[] array);
}
