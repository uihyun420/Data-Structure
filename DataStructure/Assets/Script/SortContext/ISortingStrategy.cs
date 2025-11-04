using System;

public interface ISortingStrategy<T>
{
    void Sort(T[] array);
}